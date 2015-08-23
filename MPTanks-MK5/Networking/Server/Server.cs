using Microsoft.Xna.Framework;
using MPTanks.Engine;
using MPTanks.Engine.Logging;
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
        public ILogger Logger { get; set; }
        public NetworkedGame GameInstance { get; private set; }
        public Lidgren.Network.NetServer NetworkServer { get; private set; }
        public LoginManager Login { get; private set; }
        public ConnectionManager Connections { get; set; }
        public ServerNetworkProcessor MessageProcessor { get; private set; }
        public InitializedConfiguration Configuration { get; private set; }
        public Engine.Core.Timing.Timer.Factory Timers { get; private set; }
        internal List<ServerPlayer> _players = new List<ServerPlayer>();
        public IReadOnlyList<ServerPlayer> Players => _players;
        public enum ServerStatus
        {
            NotInitialized,
            Starting,
            Open,
            Closed,
            Errored
        }
        public ServerStatus Status { get; private set; } = ServerStatus.NotInitialized;
        public Server(Configuration configuration, GameCore game, bool openOnInit = true, ILogger logger = null)
        {
            GameInstance = new NetworkedGame(FullGameState.Create(game), logger, game.Settings);
            GameInstance.Game.Authoritative = true;
            Logger = logger ?? new NullLogger();
            Login = new LoginManager(this);
            Connections = new ConnectionManager(this);
            Configuration = new InitializedConfiguration(configuration);
            MessageProcessor = new ServerNetworkProcessor();
            Timers = new Engine.Core.Timing.Timer.Factory();
            if (openOnInit) Open();
        }
        public void Open()
        {
            Status = ServerStatus.Starting;
            NetworkServer = new Lidgren.Network.NetServer(new Lidgren.Network.NetPeerConfiguration("MPTANKS")
            {
                AutoFlushSendQueue = false,
                Port = Configuration.Port
            });
            NetworkServer.Start();
        }

        public void Update(GameTime gameTime)
        {
            GameInstance.Game?.Update(gameTime);

            Timers.Update(gameTime);

            //Send all the wideband messages
            if (MessageProcessor.MessageQueue.Count > 0)
            {
                var msg = NetworkServer.CreateMessage();
                MessageProcessor.WriteMessages(msg);
                NetworkServer.SendMessage(msg, Connections.ActiveConnections,
                    Lidgren.Network.NetDeliveryMethod.ReliableOrdered,
                    Channels.GamePlayData);
            }

            //As well as narrowband ones
            foreach (var plr in Players)
            {
                if (MessageProcessor.HasPrivateMessages(plr))
                {
                    var msg = NetworkServer.CreateMessage();
                    MessageProcessor.WritePrivateMessages(plr, msg);
                    plr.Connection.SendMessage(msg,
                        Lidgren.Network.NetDeliveryMethod.ReliableOrdered,
                        Channels.GamePlayData);
                }
            }

            FlushMessages();
        }
        public void Close(string reason = "Server closed")
        {
            NetworkServer.Shutdown(reason);
        }
        public void AddPlayer(ServerPlayer player)
        {
            GameInstance.Game.AddPlayer(player);
            _players.Add(player);

            //Create a state sync loop
            Timers.CreateReccuringTimer(t =>
            {
                if (Players.Contains(player))
                {
                //do state sync
                    MessageProcessor.SendPrivateMessage(
                        player, new Common.Actions.ToClient.FullGameStateSentAction(GameInstance.Game));
                }
                else
                {
                    //Disconnect
                    Timers.RemoveTimer(t);
                }

            }, Configuration.StateSyncRate);
        }

        public void RemovePlayer(ServerPlayer player)
        {
            _players.Remove(player);
            GameInstance.Game.RemovePlayer(player);
        }

        public void SetGame(GameCore game)
        {
            GameInstance.FullGameState = FullGameState.Create(game);
            GameInstance.Game.Authoritative = true;
        }
    }
}
