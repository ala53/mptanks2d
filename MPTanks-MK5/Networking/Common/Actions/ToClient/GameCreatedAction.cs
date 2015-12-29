using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using MPTanks.Networking.Common.Game;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    /// <summary>
    /// Notifies the client that the FullGameStateSentAction to follow should not
    /// be applied to the current GameCore instance, but rather it should be used
    /// to create a brand-new GameCore instance.
    /// </summary>
    public class GameCreatedAction : ActionBase
    {
        public GameCreatedAction() { }
        protected override void DeserializeInternal(NetIncomingMessage message)
        {
        }

        public override void Serialize(NetOutgoingMessage message)
        {
        }

        public override string ToString()
        {
            return "(GameCreated)";
        }
    }
}
