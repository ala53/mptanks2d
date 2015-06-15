using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common
{
    public abstract class MessageBase
    {
        public MessageBase(byte[] data)
        {

        }

        public MessageBase()
        {

        }

        public abstract byte[] Serialize();
        
    }
}
