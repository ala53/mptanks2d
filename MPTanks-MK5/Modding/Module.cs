using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Modding
{
    public class Module
    {
        public Assembly Assembly { get; internal set; }
        /// <summary>
        /// A list of the types of tanks
        /// </summary>
        public TankType[] Tanks { get; internal set; }
    }

    public class TankType
    {
        public TankAttribute.TankCategory Category { get; internal set; }
        public Type Type { get; internal set; }
        public string RegisterType { get; set; }
        public string DisplayName { get; internal set; }
        public string DisplayDescription { get; internal set; }
    }

    public class ProjectileType
    {

    }
}
