using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions
{
    public class ObjectStateChangedAction
    {
        public static ObjectStateChangedAction Get()
        {
            return Pool.Get<ObjectStateChangedAction>();
        }
    }
}
