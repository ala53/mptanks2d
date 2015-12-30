using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server
{
    public partial class Server
    {
        public bool GameStartTimeoutHasEnded => GameStartRemainingTimeout <= TimeSpan.Zero;
        public TimeSpan GameStartRemainingTimeout { get; private set; } = ServerSettings.Instance.TimeToWaitForPlayersReady;
        public event EventHandler<TimeSpan> OnCountdownStarted = delegate { };
        public event EventHandler<TimeSpan> OnCountdownStopped = delegate { };

        public enum ServerGameStatus
        {
            WaitingForPlayers,
            InCountdown,
            InGame,
            GameEnded
        }

        public ServerGameStatus GameStatus
        {
            get
            {
                if (!Game.HasEnoughPlayersToStart)
                    return ServerGameStatus.WaitingForPlayers;
                if (!GameStartTimeoutHasEnded)
                    return ServerGameStatus.InCountdown;
                if (Game.Ended)
                    return ServerGameStatus.GameEnded;

                return ServerGameStatus.InGame;
            }
        }

        public bool CanStartGame
        {
            get
            {
                foreach (var plr in Players)
                    if (!GameStartTimeoutHasEnded)
                        if (!plr.IsReady) return false;

                if (!Game.HasEnoughPlayersToStart) return false;
                if (Game.HasStarted) return false; //can't start

                return true;
            }
        }
        public void StartGame()
        {
            if (!CanStartGame) return;

            Game.BeginGame();
        }

        public void TickGameStartCountdown(GameTime gameTime)
        {
            if (!Game.HasEnoughPlayersToStart)
            {
                var ttw = ServerSettings.Instance.TimeToWaitForPlayersReady;
                if (GameStartRemainingTimeout != ttw)
                {
                    //Send a countdown notification
                    GameStartRemainingTimeout = ttw;
                    OnCountdownStopped(this, ttw);
                }
                return;
            }
            else
            {
                if (GameStartRemainingTimeout == ServerSettings.Instance.TimeToWaitForPlayersReady)
                    OnCountdownStarted(this, GameStartRemainingTimeout);

                bool allPlayersReady = true;
                foreach (var plr in Players)
                    if (!plr.Player.IsReady) allPlayersReady = false;
                if (allPlayersReady)
                    GameStartRemainingTimeout = TimeSpan.Zero;

                if (!GameStartTimeoutHasEnded)
                {
                    var msg = MessagePool.Retrieve<Common.Actions.ToClient.CountdownStartedAction>();
                    msg.CountdownTime = GameStartRemainingTimeout;
                    MessageProcessor.SendMessage(msg);
                }
                GameStartRemainingTimeout -= gameTime.ElapsedGameTime;

                if (GameStartTimeoutHasEnded) StartGame();
            }
        }
    }
}
