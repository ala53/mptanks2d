using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common
{
    public abstract class MessageBase
    {

        public NetIncomingMessage MessageFrom { get; private set; }
        public MessageBase()
        {
        }

        protected abstract void DeserializeInternal(NetIncomingMessage message);
        public void Deserialize(NetIncomingMessage message)
        {
            MessageFrom = message;
            DeserializeInternal(message);
        }
        public abstract void Serialize(NetOutgoingMessage message);
        
    }
}
