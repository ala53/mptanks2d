using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class ObjectStateChangedAction : ActionBase
    {
        public static ObjectStateChangedAction Get()
        {
            return Pool.Get<ObjectStateChangedAction>();
        }
    }
}
