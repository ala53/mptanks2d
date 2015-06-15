using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Game
{
    public class DeltaGameState
    {
        public List<DeltaObjectState> DeltaObjectStates { get; set; }
        public ActionQueue ActionsSinceStateChanged { get; set; }

        public void Apply(GameCore game, FullGameState baseState)
        {
            var fullState = new FullGameState();
        }

        public static void Create(FullGameState baseState, FullGameState newState)
        {

        }
    }
}
