using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToServer
{
    public class InputChangedAction : Action
    {
        public static InputChangedAction Get()
        {
            return Pool.Get<InputChangedAction>();
        }
    }
}
