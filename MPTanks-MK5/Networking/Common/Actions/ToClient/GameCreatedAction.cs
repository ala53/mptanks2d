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
        public GameCreatedAction(NetIncomingMessage message) : base(message)
        {

        }

        public GameCreatedAction() { }

        public override void Serialize(NetOutgoingMessage message)
        {
        }
    }
}
