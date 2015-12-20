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
        public static BasicGameObjectInfo GetGameObjectInfo(string reflectionName)
        {
            Type type = null;
            if (Tanks.Tank.AvailableTypes.ContainsKey(reflectionName))
                type = Tanks.Tank.AvailableTypes[reflectionName];
            if (GameObject.AvailableTypes.ContainsKey(reflectionName))
                type = GameObject.AvailableTypes[reflectionName];
            if (Projectiles.Projectile.AvailableTypes.ContainsKey(reflectionName))
                type = Projectiles.Projectile.AvailableTypes[reflectionName];
            if (Maps.MapObjects.MapObject.AvailableTypes.ContainsKey(reflectionName))
                type = Maps.MapObjects.MapObject.AvailableTypes[reflectionName];

            if (type == null)
                return new BasicGameObjectInfo
                {
                    Exists = false,
                    DisplayName = "ERR_TYPE_INVALID",
                    DisplayDescription = "ERR_TYPE_INVALID"
                };

            //Get the attribute
            var attrib = ((Modding.GameObjectAttribute[])type.GetCustomAttributes(typeof(Modding.GameObjectAttribute), true))[0];

            return new BasicGameObjectInfo
            {
                Exists = true,
                DisplayName = attrib.DisplayName ?? "",
                DisplayDescription = attrib.Description ?? ""
            };
        }
    }
}
