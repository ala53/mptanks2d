using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Assets
{
   public static class MapObjects
    {
        public static class SatelliteDish
        {
            const string assetName = "assets/mapobjects/moving/satellite_dish.png";
            public static readonly SpriteInfo Base = new SpriteInfo("baseblock", assetName);
            public static readonly SpriteInfo DishAndRevolver = new SpriteInfo("dishandrevolver", assetName);
        }
    }
}
