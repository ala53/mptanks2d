using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        public bool Running { get; private set; }
        public bool Ended { get; private set; }

        public event EventHandler<Events.GameEnded> OnGameEnded;
        public event EventHandler<Events.MapObjectDamaged> OnMapObjectDamaged;
        public event EventHandler<Events.PlayerDamaged> OnPlayerDamaged;
        public event EventHandler<Events.PlayerKilled> OnPlayerKilled;
        public event EventHandler<Events.WeaponFired> OnWeaponFired;

        public event EventHandler<Events.FatalError> OnFatalError;
        public event EventHandler<Events.Warning> OnWarning;
        public event EventHandler<Events.Chat> OnChat;

        public Server(int port)
        {

        }

        public void Start()
        {

        }

        public void Update()
        {
            try
            {

            }
            catch (Exception ex)
            {
                //The outermost error loop - when all else fails, we crash
                OnFatalError(this, new Events.FatalError()
                {
                    FriendlyMessage = "Something very bad happened.",
                    Exception = ex,
                    StackTrace = ex.StackTrace
                });
            }
        }

        /// <summary>
        /// Instructs the server to start a new round, instead of closing the server.
        /// </summary>
        public void StartNewRound()
        {

        }

        public void Close(string reason)
        {

        }
    }
}
