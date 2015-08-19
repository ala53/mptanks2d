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
    public class FullGameStateSentAction : ActionBase
    {
        public FullGameState State { get; private set; }
        public FullGameStateSentAction(GameCore game) { State = FullGameState.Create(game); }
        public FullGameStateSentAction(NetIncomingMessage message) : base(message)
        {
            State = FullGameState.Read(message);
        }
        public override void Serialize(NetOutgoingMessage message)
        {
            State.Write(message);
        }
    }
}
