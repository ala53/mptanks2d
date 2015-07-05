using Microsoft.Xna.Framework;
using MPTanks.Engine;
using MPTanks.Engine.Maps.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Modding;

namespace MPTanks.CoreAssets.MapObjects
{
    [MapObject("SatelliteDishLarge", "assets/components/satellitedish.json", IsStatic = true, MinHeightBlocks = 2, MinWidthBlocks = 2, 
        DisplayName = "Satellite Dish (large)")]
    public class SatelliteDishLarge : MapObject
    {
        public SatelliteDishLarge(GameCore game, bool authorized, Vector2 position = default(Vector2), float rotation = 0)
            : base(game, authorized, position, rotation)
        {
            Size = new Vector2(4);
        }
        
        protected override void UpdateInternal(GameTime time)
        {
        }
    }
}
