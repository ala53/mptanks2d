using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions
{
    public class ProjectileCreatedAction
    {
        public static ProjectileCreatedAction Get()
        {
            return Pool.Get<ProjectileCreatedAction>();
        }
    }
}
