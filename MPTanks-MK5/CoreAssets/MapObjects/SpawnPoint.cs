using MPTanks.Engine.Maps.MapObjects;
using MPTanks.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.CoreAssets.MapObjects
{
    [MapObject("spawnpoint",
        InstanceSettingNames = new[] { "TeamNumber" },
        InstanceSettingDefaults = new[] { "1" })]
    public class SpawnPoint : MapObject
    {
        public SpawnPoint(Engine.GameCore game, bool authorized)
            : base(game, authorized)
        {

        }
    }
}
