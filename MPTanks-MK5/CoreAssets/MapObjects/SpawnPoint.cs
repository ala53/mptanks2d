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
        InstanceSettingNames = new[] { "TeamNumber" },
        InstanceSettingDefaults = new[] { "1" },
        DisplayName = "Spawn Point",
        Description = "A spawn point for a team number")]
    public class SpawnPoint : MapObject
    {
        public SpawnPoint(Engine.GameCore game, bool authorized)
            : base(game, authorized)
        {

        }
    }
}
