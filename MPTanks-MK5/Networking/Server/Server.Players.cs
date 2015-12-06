using MPTanks.Engine;
using MPTanks.Engine.Core.Timing;
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

            //Queue the game state for them, delaying 1sec
            Timers.CreateTimer(a =>
            {
                MessageProcessor.SendPrivateMessage(player,
                    new Common.Actions.ToClient.GameCreatedAction());
                MessageProcessor.SendPrivateMessage(player,
                    new Common.Actions.ToClient.FullGameStateSentAction(Game));
            }, TimeSpan.FromSeconds(1));
            //Announce that they joined
            ChatHandler.SendMessage($"{player.Player.Username} joined the server");

            player.LastSentState = PseudoFullGameWorldState.Create(Game);
            player.Player.OnPropertyChanged -= Player_PropertyChanged;
            player.Player.OnPropertyChanged += Player_PropertyChanged;
            
            MessageProcessor.SendMessage(new Common.Actions.ToClient.PlayerUpdateAction(player.Player, Game));

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

        public void RemovePlayer(ServerPlayer player, string reason = "")
        {
            _players.Remove(player);
            Game.RemovePlayer(player.Player.Id);

            player.Player.OnPropertyChanged -= Player_PropertyChanged;

            ChatHandler.SendMessage($"Player {player.Player.Username} left.");

            //Try to disconnect them
            player?.Connection?.Disconnect(reason);
            
            MessageProcessor.SendMessage(new Common.Actions.ToClient.PlayerLeftAction(player.Player, Game));
        }

        private bool _disablePropertyNotifications = false;
        private void Player_PropertyChanged(object sender, NetworkPlayer.NetworkPlayerPropertyChanged e)
        {
            if (_disablePropertyNotifications) return;
            var player = (NetworkPlayer)sender; //Send them an update
            MessageProcessor.SendMessage(new Common.Actions.ToClient.PlayerUpdateAction(player, Game));
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
