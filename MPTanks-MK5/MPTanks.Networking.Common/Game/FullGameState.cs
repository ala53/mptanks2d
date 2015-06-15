using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Game
{
    public class FullGameState
    {
        public List<FullObjectState> ObjectStates { get; set; }
        public ActionQueue ActionsSinceStateChanged { get; set; }
        public byte[] GamemodeState { get; set; }
        public int StateId { get; set; }

        public void Apply(GameCore game)
        {

        }

        public static void Create(GameCore game)
        {

        }
    }
}
