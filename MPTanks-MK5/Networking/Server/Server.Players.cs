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
        public void AddPlayer(ServerPlayer player)
        {
            if (Players.Contains(player)) return;

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
            if (e == NetworkPlayer.NetworkPlayerPropertyChanged.Tank)
            {
                MessageProcessor.SendMessage(new Common.Actions.ToClient.PlayerTankAssignedAction(
                    (NetworkPlayer)sender, ((NetworkPlayer)sender)?.Tank.ObjectId ?? ushort.MaxValue));
            }
        }

        public ServerPlayer GetPlayer(Guid id) => Players.First(a => a.Player.Id == id);

        private void UnhookPlayers(GameCore game)
        {
            foreach (var plr in game.Players)
                ((NetworkPlayer)plr).OnPropertyChanged -= Player_PropertyChanged;
        }

        private void HookPlayers (GameCore game)
        {
            foreach (var plr in game.Players)
            {
                ((NetworkPlayer)plr).OnPropertyChanged -= Player_PropertyChanged;
                ((NetworkPlayer)plr).OnPropertyChanged += Player_PropertyChanged;
            }
        }
    }
}
