using Microsoft.Xna.Framework;
using MPTanks.Engine;
using MPTanks.Networking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Client
{
    public partial class Client
    {
        public enum ClientStatus
        {
            NotStarted,
            Connecting,
            ConnectionFailed,
            Errored,
            Connected,
            LoggingIn,
            DownloadingMods,
        }
        public ClientStatus Status { get; private set; } = ClientStatus.NotStarted;

        public Lidgren.Network.NetClient NetworkClient { get; private set; }
        public bool Connected =>
            Status == ClientStatus.Connected ||
            Status == ClientStatus.LoggingIn ||
            Status == ClientStatus.DownloadingMods;
        public NetworkedGame GameInstance { get; private set; }
        public ushort PlayerId { get; set; }
        public ClientNetworkProcessor MessageProcessor { get; private set; }
        public Chat.ChatClient Chat { get; private set; }
        public GameCore Game => GameInstance.Game;
        public GamePlayer Player
        {
            get
            {
                return
                    GameInstance.Game.PlayersById.ContainsKey(PlayerId) ?
                    GameInstance.Game.PlayersById[PlayerId] : null;
            }
        }
        public bool NeedsToSelectTank { get; internal set; }
        public bool IsInCountdown { get; internal set; }
        public TimeSpan RemainingCountdownTime { get; internal set; }

        public string Host { get; set; }
        public ushort Port { get; set; }

        public bool GameRunning { get { return Connected && GameInstance != null; } }
        public Client(string connection, ushort port, string password = null, bool connectOnInit = true)
        {
            //connect to server

            MessageProcessor = new ClientNetworkProcessor(this);
            GameInstance = new NetworkedGame(null);
            Chat = new Chat.ChatClient(this);
            Host = connection;
            Port = port;
            NetworkClient = new Lidgren.Network.NetClient(
                new Lidgren.Network.NetPeerConfiguration("MPTANKS"));
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
            Status = ClientStatus.Connecting;
            if (!string.IsNullOrWhiteSpace(Host) && Port != 0)
            {
                NetworkClient.Start();
                NetworkClient.Connect(Host, Port);
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

        public void Update(GameTime gameTime)
        {
            Game.Update(gameTime);
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
