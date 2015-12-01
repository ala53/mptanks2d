using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using MPTanks.Engine;
using MPTanks.Networking.Common.Game;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    /// <summary>
    /// Used to send PseudoFullGameWorldStates, which update the positional information of the objects
    /// (but only those that have changed) while avoiding much of the overhead of a full state serialization.
    /// </summary>
    public class PartialGameStateUpdateAction : ActionBase
    {
        public PseudoFullGameWorldState StatePartial { get; private set; }
        public PartialGameStateUpdateAction()
        {
        }

        public PartialGameStateUpdateAction(GameCore game, PseudoFullGameWorldState last = null)
        {
            if (last != null)
                StatePartial = PseudoFullGameWorldState.Create(game).MakeDelta(last);
            else StatePartial = PseudoFullGameWorldState.Create(game);
        }

        protected override void DeserializeInternal(NetIncomingMessage message)
        {
            StatePartial = PseudoFullGameWorldState.Read(message);
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            StatePartial.Write(message);
        }
    }
}
