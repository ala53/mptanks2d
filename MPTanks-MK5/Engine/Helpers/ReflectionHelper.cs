using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Helpers
{
    public static class ReflectionHelper
    {
        public struct BasicGameObjectInfo
        {
            public bool Exists { get; set; }
            public string DisplayName { get; set; }
            public string DisplayDescription { get; set; }
        }
        public static BasicGameObjectInfo GetTankInfo(string reflectionName)
        {
            if (!Tanks.Tank.AvailableTypes.ContainsKey(reflectionName))
            {
                return new BasicGameObjectInfo
                {
                    Exists = false,
                    DisplayName = "ERR_TANK_TYPE_INVALID",
                    DisplayDescription = "ERR_TANK_TYPE_INVALID"
                };
            }

            var type = Tanks.Tank.AvailableTypes[reflectionName];
            //Get the attribute
            var attrib = ((Modding.TankAttribute[])type.GetCustomAttributes(typeof(Modding.TankAttribute), true))[0];

            return new BasicGameObjectInfo
            {
                Exists = true,
                DisplayName = attrib.DisplayName ?? "",
                DisplayDescription = attrib.Description ?? ""
            };
        }
    }
}
