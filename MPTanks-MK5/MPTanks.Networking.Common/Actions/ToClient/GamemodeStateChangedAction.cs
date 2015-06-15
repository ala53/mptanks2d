using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class GamemodeStateChangedAction : ActionBase
    {
        public static  GamemodeStateChangedAction Get()
        {
            return Pool.Get<GamemodeStateChangedAction>();
        }
    }
}
