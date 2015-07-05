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
    }
}
