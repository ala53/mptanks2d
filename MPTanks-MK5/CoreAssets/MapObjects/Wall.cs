using Microsoft.Xna.Framework;
using MPTanks.Engine;
using MPTanks.Engine.Maps.MapObjects;
using MPTanks.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.CoreAssets.MapObjects
{
    [MapObject("wall_generic.json", IsStatic = true, DisplayName = "Generic Wall",
        Description = "A generic, bland wall type.")]
    public class Wall : MapObject
    {
        public Wall(GameCore game, bool authorized,
            Vector2 position = default(Vector2), float rotation = 0) :
                base(game, authorized, position, rotation)
        {

        }
    }
}
