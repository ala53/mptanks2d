using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class GameObjectDestroyedAction : ActionBase
    {
        public GameObjectDestroyedAction(NetIncomingMessage message) : base(message)
        {

        }
        public override void Serialize(NetOutgoingMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
