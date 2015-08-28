using MPTanks.Engine;
using MPTanks.Networking.Common;
using MPTanks.Networking.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server
{
    public partial class Server
    {
        public ServerPlayer AddPlayer(ServerPlayer player)
        {
            if (Players.Contains(player)) return player;

            player.Player.Id = Game.AvailablePlayerId;
            Game.AddPlayer(player.Player);
            _players.Add(player);

            //Queue the game state for them
            MessageProcessor.SendPrivateMessage(player,
                new Common.Actions.ToClient.GameCreatedAction());
            MessageProcessor.SendPrivateMessage(player,
                new Common.Actions.ToClient.FullGameStateSentAction(Game));

            //Announce that they joined
            ChatHandler.SendMessage(Strings.Server.PlayerJoined(player.Player.Username));
            MessageProcessor.SendMessage(new Common.Actions.ToClient.PlayerJoinedAction(player.Player));

            player.LastSentState = PseudoFullGameWorldState.Create(Game);
            player.Player.OnPropertyChanged -= Player_PropertyChanged;
            player.Player.OnPropertyChanged += Player_PropertyChanged;

            //Create a state sync loop
            Timers.CreateReccuringTimer(t =>
            {
                if (Players.Contains(player))
                {
                    var message = new Common.Actions.ToClient.PartialGameStateUpdateAction(Game, player.LastSentState);
                    player.LastSentState = message.StatePartial;
                    //do state sync
                    MessageProcessor.SendPrivateMessage(player, message);
                }
                else
                {
                    //Disconnect
                    Timers.RemoveTimer(t);
                }

            }, Configuration.StateSyncRate);

            return player;
        }

        public void RemovePlayer(ServerPlayer player)
        {
            _players.Remove(player);
            Game.RemovePlayer(player.Player.Id);

            player.Player.OnPropertyChanged -= Player_PropertyChanged;

            MessageProcessor.SendMessage(new Common.Actions.ToClient.PlayerLeftAction(player.Player));
        }

        private void Player_PropertyChanged(object sender, NetworkPlayer.NetworkPlayerPropertyChanged e)
        {
            var player = (NetworkPlayer)sender;
            if (e == NetworkPlayer.NetworkPlayerPropertyChanged.Tank)
            {
                MessageProcessor.SendMessage(new Common.Actions.ToClient.PlayerTankAssignedAction(
                    player, player?.Tank.ObjectId ?? ushort.MaxValue));
            }
            else if (e == NetworkPlayer.NetworkPlayerPropertyChanged.SelectedTankReflectionName)
            {
                //Validate their selection
                if (player.TankSelectionIsValid)
                {
                    //It's ok: let everyone know
                    MessageProcessor.SendMessage(
                        new Common.Actions.ToClient.PlayerSelectedTankAction(
                            player, player.SelectedTankReflectionName));

                    MessageProcessor.SendPrivateMessage(GetPlayer(player.Id),
                        new Common.Actions.ToClient.PlayerTankSelectionAcknowledgedAction(true));
                }
                else
                {
                    //It's not ok: make them switch
                    MessageProcessor.SendPrivateMessage(GetPlayer(player.Id),
                        new Common.Actions.ToClient.PlayerTankSelectionAcknowledgedAction(false));
                }
            }
        }

        public ServerPlayer GetPlayer(ushort id) => Players.FirstOrDefault(a => a.Player.Id == id);

        private void UnhookPlayers(GameCore game)
        {
            foreach (var plr in game.Players)
                ((NetworkPlayer)plr).OnPropertyChanged -= Player_PropertyChanged;
        }

        private void HookPlayers(GameCore game)
        {
            foreach (var plr in game.Players)
            {
                ((NetworkPlayer)plr).OnPropertyChanged -= Player_PropertyChanged;
                ((NetworkPlayer)plr).OnPropertyChanged += Player_PropertyChanged;
            }
        }
    }
}
