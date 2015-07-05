using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common
{
    public abstract class MessageBase
    {
        public MessageBase(Lidgren.Network.NetIncomingMessage message)
        {

        }

        public MessageBase()
        {

        }

        public abstract void Serialize(Lidgren.Network.NetOutgoingMessage message);
        
    }
}
