using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MPTanks.Clients.GameClient.Menus.InGame
{
    /// <summary>
    /// A running game. That is, a game with mods loaded, etc. Sandboxed from the menus.
    /// </summary>
    public class LiveGame
    {
        /// <summary>
        /// Whether the game is trying to connect
        /// </summary>
        public bool Connecting { get; private set; }
        public bool Connected { get; private set; }
        public bool ConnectionFailed { get; private set; }
        public string FailureReason { get; private set; }

        private AppDomain _domain;
        private Thread _mtTask;
        private Action<LiveGame> _exitCallback = (game) => { };
        public LiveGame(Networking.Common.Connection.ConnectionInfo connectionInfo, string[] modsToInject)
        {
            _domain = AppDomain.CreateDomain("Live game: " + connectionInfo.FriendlyServerName, null);
            _mtTask = new Thread(() =>
            {
                _domain.ExecuteAssembly(Settings.FilePathForInGameAssembly);
                Unload();
            });
            _mtTask.Start();
        }

        public void WaitForExit()
        {
            while (_mtTask.IsAlive)
                Thread.Sleep(50);
            Unload();
        }

        public void RegisterExitCallback(Action<LiveGame> callback)
        {
            _exitCallback = callback;
        }

        private bool _closed = false;
        public void Close()
        {
            if (_closed) return;
            _closed = true;

            if (_mtTask.IsAlive)
                _mtTask.Abort();

            Unload();
        }

        private void Unload()
        {
            if (_closed) return;
            _closed = true;

            AppDomain.Unload(_domain);
            _exitCallback(this);
            Connected = false;
            ConnectionFailed = true;
            FailureReason = Strings.ClientMenus.GameForciblyClosedByWatchDog;
        }
    }
}
