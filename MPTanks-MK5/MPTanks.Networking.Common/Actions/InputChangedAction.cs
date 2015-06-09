using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions
{
    public class InputChangedAction
    {
        public static InputChangedAction Get()
        {
            return Pool.Get<InputChangedAction>();
        }
    }
}
