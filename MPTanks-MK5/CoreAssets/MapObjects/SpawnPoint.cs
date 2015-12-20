using Microsoft.Xna.Framework;
using MPTanks.Engine.Maps.MapObjects;
using MPTanks.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.CoreAssets.MapObjects
{
    [MapObject("spawnpoint.json",
        InstanceSettingNames = new[] { "Team Number" },
        InstanceSettingDefaults = new[] { "1" },
        DisplayName = "Spawn Point",
        Description = "A spawn point for a team number")]
    public class SpawnPoint : MapObject
    {
        public short Team { get; private set; } = 0;
        public SpawnPoint(Engine.GameCore game, bool authorized, Vector2 position, float rotation)
            : base(game, authorized, position, rotation)
        {

        }

        public override bool ValidateInstanceSetting(string setting, string value)
        {
            if (value == null) return false;
            if (setting == "Team Number")
            {
                short asInt;
                if (!short.TryParse(value, out asInt))
                    return false;

                if (asInt < 0) return false; //Team must be greater than or equal to 0
                Team = asInt;
            }
            return true;
        }
    }
}
