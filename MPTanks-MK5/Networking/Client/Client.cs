using Microsoft.Xna.Framework;
using MPTanks.Engine;
using MPTanks.Engine.Logging;
using MPTanks.Engine.Settings;
using MPTanks.Engine.Tanks;
using MPTanks.Networking.Common;
using MPTanks.Networking.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSB.Drm.Client.Exceptions;

namespace MPTanks.Networking.Client
{
    public partial class Client
    {
        public enum ClientStatus
        {
            NotStarted,
            Authenticating,
            Connecting,
            Disconnected,
            Errored,
            Connected,
            DownloadingMods,
        }
        public ClientStatus Status { get; private set; } = ClientStatus.NotStarted;
        public string Message { get; private set; }
        public Lidgren.Network.NetClient NetworkClient { get; private set; }
        public bool Connected =>
            Status == ClientStatus.Connected ||
            Status == ClientStatus.Authenticating ||
            Status == ClientStatus.DownloadingMods;
        public NetworkedGame GameInstance { get; private set; }
        public ushort? PlayerId => Player?.Id;
        private InputState _input;
        public InputState Input
        {
            get { return _input; }
            set
            {
                if (_input != value)
                    MessageProcessor.SendMessage(new Common.Actions.ToServer.InputChangedAction(
                        Player?.Tank?.Position ?? Vector2.Zero, value));
                _input = value;
                //if (PlayerId != null)
                //    Game.InjectPlayerInput(PlayerId.Value, value);
            }
        }
        public ClientNetworkProcessor MessageProcessor { get; private set; }
        public Chat.ChatClient Chat { get; private set; }
        public GameCore Game => GameInstance.Game;
        public GamePlayer Player
        {
            get
            {
                return
                    GameInstance.Game.Players.FirstOrDefault(
                        a => (a as NetworkPlayer)?.UniqueId == ZSB.DrmClient.User?.UniqueId);
            }
        }
        public bool NeedsToSelectTank { get; internal set; }
        public bool IsInCountdown { get; internal set; }
        public TimeSpan RemainingCountdownTime { get; internal set; }

        public string Host { get; private set; }
        public ushort Port { get; private set; }
        public string Password { get; private set; }

        public ILogger Logger { get; set; }

        public bool GameRunning { get { return Connected && GameInstance != null; } }
        public bool IsInGame => GameRunning && Game.HasStarted;
        public Client(string connection, ushort port, ILogger logger = null,
            string password = null, bool connectOnInit = true)
        {
            if (logger != null)
                Logger = new ModuleLogger(logger, "CLIENT");
            else Logger = new NullLogger();

            MessageProcessor = new ClientNetworkProcessor(this);
            GameInstance = new NetworkedGame(null);
            Chat = new Chat.ChatClient(this);

            Host = connection;
            Port = port;
            Password = password;

            NetworkClient = new Lidgren.Network.NetClient(
                new Lidgren.Network.NetPeerConfiguration("MPTANKS")
                {
                    ConnectionTimeout = GlobalSettings.Debug ? (float)Math.Pow(2, 16) : 15,
                    AutoFlushSendQueue = false
                });
            SetupNetwork();

            if (connectOnInit)
                Connect();
        }

        private void TickCountdown(GameTime gameTime)
        {
            if (!IsInCountdown) return;

            RemainingCountdownTime -= gameTime.ElapsedGameTime;
            if (RemainingCountdownTime < TimeSpan.Zero)
                IsInCountdown = false;
        }
        private bool _hasConnected;
        public void Connect()
        {
            if (_hasConnected == true) return;
            _hasConnected = true;
            if (!string.IsNullOrWhiteSpace(Host) && Port != 0)
            {
                //Do a deferred web request to get a token
                Logger.Trace("Doing web request for auth token");
                Task.Run(async () =>
                {
                    Status = ClientStatus.Authenticating;
                    Message = "Authenticating with ZSB servers...";
                    string token = "OFFLINE";
                    try
                    {
                        token = await ZSB.DrmClient.Multiplayer.GetServerTokenAsync();
                    }
                    //Catch issues that force us offline
                    catch (UnableToAccessAccountServerException)
                    { } //Offline mode
                    catch (AccountServerException)
                    { if (GlobalSettings.Trace) throw; }
                    catch (InvalidAccountServerResponseException)
                    { if (GlobalSettings.Trace) throw; }
                    //And send connection message

                    Status = ClientStatus.Connecting;
                    Message = $"Connecting to server (" + (ZSB.DrmClient.Offline ? "Offline" : "Online") + " mode)...";

                    var msg = NetworkClient.CreateMessage();
                    msg.Write(ZSB.DrmClient.User.UniqueId.ToByteArray());
                    msg.Write(ZSB.DrmClient.User.Username);
                    msg.Write(token);
                    msg.Write(Password ?? ""); //Write an empty password just in case
                    NetworkClient.Start();
                    NetworkClient.Connect(Host, Port, msg);
                    Logger.Trace("Connection message sent");
                });

            }
        }

        /// <summary>
        /// Waits until it connects to the server and downloads the game state or returns false if the connection
        /// timed out;
        /// </summary>
        /// <returns></returns>
        public bool WaitForConnection()
        {
            while (NetworkClient.ConnectionStatus == Lidgren.Network.NetConnectionStatus.InitiatedConnect ||
                NetworkClient.ConnectionStatus == Lidgren.Network.NetConnectionStatus.ReceivedInitiation ||
                NetworkClient.ConnectionStatus == Lidgren.Network.NetConnectionStatus.RespondedAwaitingApproval ||
                NetworkClient.ConnectionStatus == Lidgren.Network.NetConnectionStatus.RespondedConnect)
                ProcessMessages();

            return NetworkClient.ConnectionStatus == Lidgren.Network.NetConnectionStatus.Connected;
        }

        internal PseudoStateInterpolator _interpolator = new PseudoStateInterpolator();
        public void Update(GameTime gameTime)
        {
            if (RemainingCountdownTime > TimeSpan.Zero)
                RemainingCountdownTime -= gameTime.ElapsedGameTime;
            ProcessMessages();
            _interpolator.Apply(gameTime);
            TickCountdown(gameTime);
            Game.Update(gameTime);
            if (MessageProcessor.MessageQueue.Count > 0 &&
                NetworkClient.ConnectionStatus == Lidgren.Network.NetConnectionStatus.Connected)
            {
                var msg = NetworkClient.CreateMessage();
                MessageProcessor.WriteMessages(msg);
                NetworkClient.SendMessage(msg, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
            }
            MessageProcessor.ClearQueue();
            NetworkClient.FlushSendQueue();
        }

        private bool _hasDisconnected = false;
        public void Disconnect()
        {
            if (!_hasConnected || _hasDisconnected) return;
            _hasDisconnected = true;
            NetworkClient.Disconnect("Leaving");
        }

    }
}
