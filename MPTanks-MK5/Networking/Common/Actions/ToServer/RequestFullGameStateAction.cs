using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToServer
{
    public class RequestFullGameStateAction : ActionBase
    {
        //Nothing needs to go here because the message type speaks for itself
        public RequestFullGameStateAction()
        {

        }
        protected override void DeserializeInternal(NetIncomingMessage message)
        {
        }
        public override void Serialize(NetOutgoingMessage message)
        {
        }
    }
}
