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
        public MessageBase(NetIncomingMessage message)
        {
            MessageFrom = message;
        }

        public MessageBase()
        {

        }

        public abstract void Serialize(NetOutgoingMessage message);
        
    }
}
