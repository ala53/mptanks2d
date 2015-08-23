using Lidgren.Network;
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
        public Guid SenderId { get; private set; }
        public ReceivedChatMessageAction(NetIncomingMessage message) : base(message)
        {
            Message = message.ReadString();
            SenderId = new Guid(message.ReadBytes(16));
        }

        public ReceivedChatMessageAction(string message, Guid senderId)
        {
            Message = message;
            SenderId = senderId;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(Message);
            message.Write(SenderId.ToByteArray());
        }
    }
}
