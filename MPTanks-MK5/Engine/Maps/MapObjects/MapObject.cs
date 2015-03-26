using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Maps.MapObjects
{
    public abstract class MapObject : GameObject
    {
        public MapObject(GameCore game,  bool authorized,
            Vector2 position = default(Vector2), float rotation = 0)
            : base(game, authorized, 100, 0, position, rotation)
        {
        }

        protected override void CreateInternal()
        {
            Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
            base.CreateInternal();
        }
    }
}
