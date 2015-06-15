using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class GameEndedAction : ActionBase
    {
        public static GameEndedAction Get()
        {
            return Pool.Get<GameEndedAction>();
        }
    }
}
