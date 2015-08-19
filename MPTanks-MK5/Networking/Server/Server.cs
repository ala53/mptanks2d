using Microsoft.Xna.Framework;
using MPTanks.Engine;
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
        public Lidgren.Network.NetServer NetworkServer { get; private set; }
        public NetworkedGame GameInstance { get; private set; }
        public ServerNetworkProcessor MessageProcessor { get; private set; }
        public ushort Port { get; private set; }
        public string Password { get; set; }
        private List<ServerPlayer> _players;
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
        public Server(ushort port, string password, GameCore game, bool openOnInit = true)
        {
            Password = password;
            Port = port;
            SetGame(game);
            MessageProcessor = new ServerNetworkProcessor();
        }
        public void Open()
        {
            Status = ServerStatus.Starting;
            NetworkServer = new Lidgren.Network.NetServer(new Lidgren.Network.NetPeerConfiguration("MPTANKS")
            {
             AutoFlushSendQueue = false,
             Port = Port
            });
            
        }

        public void Update(GameTime gameTime)
        {
            GameInstance.Game?.Update(gameTime);
            var msg = NetworkServer.CreateMessage();

            MessageProcessor.SendMessage(new Common.Actions.ToClient.FullGameStateSentAction(GameInstance.Game));
            MessageProcessor.WriteMessages(msg);
            foreach (var player in Players)
            {
                player.Connection.SendMessage(msg, Lidgren.Network.NetDeliveryMethod.ReliableOrdered, 0);
            }
            
        }
        public void Close()
        {

        }
        public void SetGame(GameCore game)
        {
            GameInstance.InitialGameState = FullGameState.Create(game);
        }
    }
}
