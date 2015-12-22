using Microsoft.Xna.Framework;
using MPTanks.Engine;
using MPTanks.Engine.Maps.MapObjects;
using MPTanks.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics.Contacts;

namespace MPTanks.CoreAssets.MapObjects
{
    [MapObject("largehouse.json", IsStatic = true, MinHeightBlocks = 3, MinWidthBlocks = 3,
        DisplayName = "Multi level house")]
    public class MultilevelHouse : MapObject
    {
        public MultilevelHouse(GameCore game, bool authorized = false, Vector2 position = default(Vector2), float rotation = 0)
            : base(game, authorized, position, rotation)
        {
        }

        protected override bool CollideInternal(GameObject other, Contact contact)
        {
            return base.CollideInternal(other, contact);
        }

        protected override void UpdateInternal(GameTime gameTime)
        {
            if (Health < 50) Emitters["chimney_smoke"].Paused = true;
        }
    }
}
