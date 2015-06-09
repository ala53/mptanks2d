using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions
{
    public class MapObjectCreatedAction
    {
        public static MapObjectCreatedAction Get()
        {
            return Pool.Get<MapObjectCreatedAction>();
        }
    }
}
