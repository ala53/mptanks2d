using MPTanks.Engine.Settings;
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
        public CrossDomainObject DomainProxy { get; private set; }

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
            if (GlobalSettings.Debug)
            {
                //In debug mode, don't do domain wrapping
                DomainProxy = CrossDomainObject.Instance;
                Clients.GameClient.Program.Main(new string[] { });
                Unload();
            }
            else
            {
                _domain = AppDomain.CreateDomain("Live game: " + connectionInfo.FriendlyServerName, null);
                _mtTask = new Thread(() =>
                {
                    try
                    {
                        _domain.Load(typeof(CrossDomainObject).Assembly.FullName);
                        DomainProxy = (CrossDomainObject)_domain.CreateInstanceAndUnwrap(
                            typeof(CrossDomainObject).Assembly.FullName,
                            typeof(CrossDomainObject).FullName);

                        _domain.ExecuteAssemblyByName(typeof(CrossDomainObject).Assembly.FullName);
                    }
                    catch (Exception ex)
                    {
                        ConnectionFailed = true;
                        FailureReason = Strings.ClientMenus.GameCrashedUnknownCause(ex.Message);

                    }
                    Unload();
                });
                _mtTask.Start();
            }
        }

        public void WaitForExit()
        {
            if (GlobalSettings.Debug) Unload();
            else _mtTask.Join();
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

            if (!GlobalSettings.Debug && _mtTask.IsAlive)
                _mtTask.Abort();

            Unload();
        }

        private void Unload()
        {
            if (_closed) return;
            _closed = true;

            if (!GlobalSettings.Debug) AppDomain.Unload(_domain);

            _exitCallback(this);
            Connected = false;
            ConnectionFailed = true;
            FailureReason = Strings.ClientMenus.GameForciblyClosedByWatchDog;
        }
    }
}
