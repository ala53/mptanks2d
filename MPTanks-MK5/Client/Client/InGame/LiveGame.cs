using EmptyKeys.UserInterface;
using Microsoft.Xna.Framework;
using MPTanks.Client.GameSandbox;
using MPTanks.Engine;
using MPTanks.Engine.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MPTanks.Client
{
    /// <summary>
    /// A running game. That is, a game with mods loaded, players joined, etc. Sandboxed from the menus.
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

        private Process _prc;
        private Action<LiveGame> _exitCallback = (game) => { };
        private ClientCore _client;
        private bool _sandboxed = ClientSettings.Instance.SandboxGames;
        private string _serializedArgs;
        public LiveGame(ClientCore client, Networking.Common.Connection.ConnectionInfo connectionInfo, string[] modsToInject)
        {
            _client = client;

            var dp = new CrossProcessStartData();
            dp.SandboxingEnabled = _sandboxed;

            dp.Ip = connectionInfo.ServerAddress;
            dp.Port = connectionInfo.ServerPort;
            dp.Password = connectionInfo.Password;
            dp.IsGameHost = connectionInfo.IsHost;
            dp.WindowPositionX = _client.Window.Position.X;
            dp.WindowPositionY = _client.Window.Position.Y;
            dp.WindowWidth = _client.WindowSize.X;
            dp.WindowHeight = _client.WindowSize.Y;
            dp.ServerEngineSettingsJSON = EngineSettings.GetInstance().Save();

            _serializedArgs = Newtonsoft.Json.JsonConvert.SerializeObject(dp);

            if (_sandboxed)
                _prc = new Process
                {
                    StartInfo = new ProcessStartInfo(
                        typeof(GameClient).Assembly.Location,
                        EncodeParameterArgument(_serializedArgs))
                };
        }
        /// <summary>
        /// Encodes an argument for passing into a program
        /// </summary>
        /// <param name="original">The value that should be received by the program</param>
        /// <returns>The value which needs to be passed to the program for the original value 
        /// to come through</returns>
        private static string EncodeParameterArgument(string original)
        {
            if (string.IsNullOrEmpty(original))
                return original;
            string value = Regex.Replace(original, @"(\\*)" + "\"", @"$1\$0");
            value = Regex.Replace(value, @"^(.*\s.*?)(\\*)$", "\"$1$2$2\"");
            return value;
        }
        public void Run()
        {
            if (!_sandboxed)
            {
                GameSandbox.Program.Main(new string[] { _serializedArgs });
                Logger.Error("Closing program to prevent crashes (Debug mode without sandboxing causes severe issues when " +
                    "attempting to run multiple games through the same process)");
                Environment.Exit(-2); //Force close because, well, it will die anyway and now, we can be graceful
            }
            else
            {
                _prc.Start();
                //Exit polling
                Task.Run(async () =>
                {
                    while (!_prc.HasExited)
                        await Task.Delay(50);

                    Unload();
                });
            }
        }

        public void WaitForExit(int? ms = null)
        {
            if (_sandboxed && !_prc.HasExited) _prc.WaitForExit(ms ?? -1);
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

            if (_sandboxed && !_prc.HasExited)
            {
                _prc.CloseMainWindow();
                WaitForExit();
            }

            Unload(true);
        }

        private void Unload(bool force = false)
        {
            if (_closed && !force) return;
            _closed = true;

            Connected = false;
            ConnectionFailed = true;
            if (force)
                FailureReason = Strings.ClientMenus.GameForciblyClosedByWatchDog;
            _exitCallback(this);
        }

    }
}
