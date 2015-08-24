using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    /// <summary>
    /// Used for synchronization: lets the client know that the game has ended and that
    /// it should go into the post game physics phase.
    /// </summary>
    public class GameEndedAction : ActionBase
    {

        public GameEndedAction(NetIncomingMessage message) : base(message)
        {

        }

        public GameEndedAction() { }

        public override void Serialize(NetOutgoingMessage message)
        {
        }
    }
}
