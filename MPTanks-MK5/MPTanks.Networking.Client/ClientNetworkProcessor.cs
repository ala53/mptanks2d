using MPTanks.Networking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using MPTanks.Networking.Common.Actions;

namespace MPTanks.Networking.Client
{
    public class ClientNetworkProcessor : NetworkProcessorBase
    {
        public override void ProcessToClientAction(NetConnection client, ActionBase action)
        {
            throw new NotImplementedException();
        }

        public override void ProcessToClientMessage(NetConnection client, MessageBase message)
        {
            throw new NotImplementedException();
        }

        public override void ProcessToServerAction(ActionBase action)
        {
            throw new NotImplementedException();
        }

        public override void ProcessToServerMessage(MessageBase message)
        {
            throw new NotImplementedException();
        }
    }
}
