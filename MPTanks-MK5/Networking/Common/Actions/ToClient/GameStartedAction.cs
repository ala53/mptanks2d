using Lidgren.Network;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class GameStartedAction : ActionBase
    {
        public GameStartedAction(NetIncomingMessage message) : base(message)
        {
        }

        public GameStartedAction()
        {
        }

        public override void Serialize(NetOutgoingMessage message)
        {
        }
    }
}
