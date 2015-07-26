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
    [Modding.MapObject("TankBlock", "tankblocks.json", IsStatic = false)]
    public class TankBlocks : MapObject
    {
        public TankBlocks(GameCore game, bool authorized, Vector2 position, float rotation)
            : base(game, authorized, position, rotation, 10000, 0)
        {
            
        }

        protected override void CreateInternal()
        {
            Body.LinearDamping = 5;
            Body.AngularDamping = 5;
        }

        protected override bool CollideInternal(GameObject other, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (other.GetType().IsSubclassOf(typeof(Projectile))) return false; //let projectiles go through
            if (other.Flags.Contains("goes_through_tank_blocks")) return false; //take advantage of flags so some tanks
                                                                                //can go through, if explicitly allowed
            return base.CollideInternal(other, contact);
        }
    }
}
