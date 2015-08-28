using Lidgren.Network;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class ReceivedChatMessageAction : ActionBase
    {
        public string Message { get; private set; }
        public ushort SenderId { get; private set; }
        public ReceivedChatMessageAction(NetIncomingMessage message) : base(message)
        {
            Message = message.ReadString();
            SenderId = (ushort)message.ReadUInt32(GameCore.PlayerIdNumberOfBits);
        }

        public ReceivedChatMessageAction(string message, ushort senderId)
        {
            Message = message;
            SenderId = senderId;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(Message);
            message.Write(SenderId, GameCore.PlayerIdNumberOfBits);
        }
    }
}
