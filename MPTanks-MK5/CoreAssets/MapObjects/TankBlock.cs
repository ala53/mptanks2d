using Microsoft.Xna.Framework;
using MPTanks.Engine;
using MPTanks.Engine.Maps.MapObjects;
using MPTanks.Engine.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.CoreAssets.MapObjects
{
    [Modding.MapObject("tankblocks.json", IsStatic = true,
        DisplayName = "Tank Block", 
        Description = "A tank block, through which a projectile can pass but not a tank.")]
    public class TankBlock : MapObject
    {
        public TankBlock(GameCore game, bool authorized, Vector2 position, float rotation)
            : base(game, authorized, position, rotation)
        {

        }

        protected override void CreateInternal()
        {
            base.CreateInternal();
        }

        protected override bool CollideInternal(GameObject other, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (other.GetType().IsSubclassOf(typeof(Projectile))) return false; //let projectiles go through
            if (other.Flags.Contains("PassesThroughTankBlocks")) return false; //take advantage of flags so some tanks
                                                                               //can go through, if explicitly allowed
            return base.CollideInternal(other, contact);
        }
    }
}
