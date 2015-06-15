using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class GameStartedAction : Action
    {
        public static GameStartedAction Get()
        {
            return Pool.Get<GameStartedAction>();
        }
    }
}
