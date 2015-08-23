using MPTanks.Networking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using MPTanks.Networking.Common.Actions;
using MPTanks.Networking.Common.Actions.ToServer;

namespace MPTanks.Networking.Server
{
    public class ServerNetworkProcessor : NetworkProcessorBase
    {
        public Server Server { get; private set; }

        public ServerNetworkProcessor(Server server)
        {
            Server = server;
        }

        public override void ProcessToServerAction(ActionBase action)
        {
            if (action is InputChangedAction)
            {

            }

            if (action is PlayerTankTypeSelectedAction)
            {

            }

            if (action is RequestFullGameStateAction)
            {

            }
            if (action is SentChatMessageAction)
            {
                var act = action as SentChatMessageAction;
                Server.ChatHandler.ForwardMessage(act.Message,
                    Server.Connections.PlayerTable[action.MessageFrom.SenderConnection],
                    act.Targets.Select(a=>Server.GetPlayer(a)).ToArray());
            }
        }


        public override void ProcessToServerMessage(MessageBase message)
        {
        }

        private Dictionary<ServerPlayer, List<MessageBase>> _privateQueue =
            new Dictionary<ServerPlayer, List<MessageBase>>();
        public IReadOnlyDictionary<ServerPlayer, List<MessageBase>> PrivateMessageQueues => _privateQueue;

        public void SendPrivateMessage(ServerPlayer player, MessageBase message)
        {
            if (!_privateQueue.ContainsKey(player))
                _privateQueue.Add(player, new List<MessageBase>());

            _privateQueue[player].Add(message);
        }

        public void WritePrivateMessages(ServerPlayer player, NetOutgoingMessage message)
        {
            if (!_privateQueue.ContainsKey(player))
                _privateQueue.Add(player, new List<MessageBase>());

            var queue = _privateQueue[player];
            message.Write((ushort)queue.Count);
            foreach (var msg in queue)
                message.Write(TypeIndexTable[msg.GetType()]);
            foreach (var msg in queue)
                msg.Serialize(message);

            queue.Clear();
        }

        public bool HasPrivateMessages(ServerPlayer player)
        {
            if (!_privateQueue.ContainsKey(player))
                _privateQueue.Add(player, new List<MessageBase>());

            return _privateQueue[player].Count > 0;
        }

        public void ClearPrivateQueues()
        {
            _privateQueue.Clear();
        }
    }
}
