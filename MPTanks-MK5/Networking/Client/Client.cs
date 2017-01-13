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
    public partial class NetClient
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
                        a => (a as NetworkPlayer)?.UniqueId == _playerId);
            }
        }
        public bool NeedsToSelectTank { get; internal set; }
        public bool IsInCountdown { get; internal set; }
        public bool IsInTankSelection
        {
            get
            {
                if (IsInCountdown) return true;
                if (Player == null) return false;
                if (PlayerIsReady) return false;
                if (Player.HasTank) return false;
                if (Player.IsSpectator) return false;
                if (Player.AllowedTankTypes == null) return false;
                if (Game.Ended) return false;
                if (!Connected) return false;
                if (Game.HasStarted && !Game.Gamemode.HotJoinEnabled) return false;

                return true;
            }
        }
        public TimeSpan RemainingCountdownTime { get; internal set; }

        public string Host { get; private set; }
        public ushort Port { get; private set; }
        public string Password { get; private set; }

        public ILogger Logger { get; set; }

        /// <summary>
        /// Whether the player has chosen their tank and is ready to play the game
        /// </summary>
        public bool PlayerIsReady
        {
            get
            {
                if (Player as NetworkPlayer == null)
                    return false;
                return (Player as NetworkPlayer).IsReady;
            }
            set
            {
                if (Player as NetworkPlayer == null)
                    return;
                (Player as NetworkPlayer).IsReady = value;
                MessageProcessor.SendMessage(new Common.Actions.ToServer.PlayerReadyChangedAction(value));
            }
        }

        private Guid _playerId;
        private string _playerName;
        public bool IsInGame => GameInstance != null && Game.HasStarted && Connected;
        public NetClient(string connection, ushort port, ILogger logger = null,
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
                    ConnectionTimeout = 15,
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
                        if (!GlobalSettings.Instance.NoLoginMode)
                            token = await ZSB.DrmClient.Multiplayer.GetServerTokenAsync();
                    }
                    //Catch issues that force us offline
                    catch (UnableToAccessAccountServerException)
                    { } //Offline mode
                    catch (AccountServerException)
                    { if (GlobalSettings.Trace) throw; }
                    catch (InvalidAccountServerResponseException)
                    { if (GlobalSettings.Trace) throw; }
                    //Get the username and UID
                    _playerName = GlobalSettings.Instance.NoLoginMode ? GenerateOfflineUsername() : ZSB.DrmClient.User.Username;
                    _playerId = GlobalSettings.Instance.NoLoginMode ? Guid.NewGuid() : ZSB.DrmClient.User.UniqueId;

                    //And send connection message

                    Status = ClientStatus.Connecting;

                    Logger.Info($"Connecting to {Host}:{Port} (DRM Status: (" + (ZSB.DrmClient.Offline ? "Offline" : "Online") + ")");
                    Logger.Info($"Client version {StaticSettings.VersionMajor}.{StaticSettings.VersionMinor}");
                    Logger.Info($"Name: {_playerName}.");

                    Message = $"Connecting to {Host}:{Port} in " + (ZSB.DrmClient.Offline ? "Offline" : "Online") + " mode...";

                    var msg = NetworkClient.CreateMessage();
                    msg.Write(_playerId.ToByteArray());
                    msg.Write(_playerName);
                    msg.Write(token);
                    msg.Write(Password ?? ""); //Write an empty password just in case
                    msg.Write(StaticSettings.VersionMajor);
                    msg.Write(StaticSettings.VersionMinor);
                    NetworkClient.Start();
                    NetworkClient.Connect(Host, Port, msg);
                    Logger.Trace("Connection message sent");
                });

            }
        }

        private static Random _random = new Random();
        private string GenerateOfflineUsername()
        {
            return "_OFFLINEUSER_" + _random.Next();
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
            {
                RemainingCountdownTime -= gameTime.ElapsedGameTime;
            }
            ProcessMessages();
            _interpolator.Apply(gameTime);
            TickCountdown(gameTime);
            GameInstance.Tick(gameTime);
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
