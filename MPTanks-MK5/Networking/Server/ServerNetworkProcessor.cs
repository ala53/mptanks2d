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
        public Server Server { get; set; }

        public override void ProcessToServerAction(dynamic action)
        {
            if (action is FireProjectileAction)
            {

            }

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

            }
        }


        public override void ProcessToServerMessage(dynamic message)
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
    }
}
