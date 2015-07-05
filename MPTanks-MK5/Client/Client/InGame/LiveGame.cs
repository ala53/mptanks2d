using Microsoft.Xna.Framework;
using MPTanks.Client.GameSandbox;
using MPTanks.Engine.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MPTanks.Client
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
        private bool _clearedToRun;
        private Action<LiveGame> _exitCallback = (game) => { };
        private ClientCore _client;
        public LiveGame(ClientCore client, Networking.Common.Connection.ConnectionInfo connectionInfo, string[] modsToInject)
        {
            _client = client;
            if (!ClientSettings.Instance.SandboxGames)
            {
                //In debug mode, don't do domain wrapping
                DomainProxy = CrossDomainObject.Instance;
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
                        while (!_clearedToRun) Thread.Sleep(50);
                        SetStartWindowParams();
                        _domain.ExecuteAssemblyByName(typeof(CrossDomainObject).Assembly.FullName);
                    }
                    catch (Exception ex)
                    {
                        ConnectionFailed = true;
                        FailureReason = Strings.ClientMenus.GameCrashedUnknownCause(ex.Message);
                        Logger.Error("Live game crashed!", ex);
                    }
                    Unload();
                });
                _mtTask.Start();
            }
        }

        private void SetStartWindowParams()
        {
            DomainProxy.WindowPositionX = _client.Window.Position.X;
            DomainProxy.WindowPositionY = _client.Window.Position.Y;
            DomainProxy.WindowWidth = _client.WindowSize.X;
            DomainProxy.WindowHeight = _client.WindowSize.Y;
        }

        private void SetReturnWindowParams()
        {
            _client.QueuePositionAndSizeSet(DomainProxy.WindowPositionX, DomainProxy.WindowPositionY,
            DomainProxy.WindowWidth, DomainProxy.WindowHeight);
        }

        public void Run()
        {
            if (!ClientSettings.Instance.SandboxGames)
            {
                SetStartWindowParams();
                Client.GameSandbox.Program.Main(new string[] { });
            }
            _clearedToRun = true;
        }

        public void WaitForExit()
        {
            if (ClientSettings.Instance.SandboxGames) _mtTask.Join();
            Close();
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

            if (ClientSettings.Instance.SandboxGames && _mtTask.IsAlive)
                _mtTask.Abort();

            Unload();
        }

        private void Unload()
        {
            if (_closed) return;
            _closed = true;

            _clearedToRun = false;

            SetReturnWindowParams();

            if (ClientSettings.Instance.SandboxGames) AppDomain.Unload(_domain);

            _exitCallback(this);
            Connected = false;
            ConnectionFailed = true;
            FailureReason = Strings.ClientMenus.GameForciblyClosedByWatchDog;
        }
    }
}
