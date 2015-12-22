using MPTanks.Engine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Engine.Projectiles;
using MPTanks.Engine.Tanks;
using MPTanks.Engine;
using MPTanks.Engine.Core;
using MPTanks.Engine.Assets;
using MPTanks.Modding;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace MPTanks.CoreAssets.Projectiles.BasicTank
{
    [Projectile("basictankmainprojectile.json", DisplayName = "Main gun projectile for Basic Tank")]
    public class MainGunProjectile : Projectile
    {
        /// <summary>
        /// The amount of damage this projectile does.
        /// </summary>
        public override int DamageAmount => 60;
        public override bool DamagesMapObjects => false;
        public MainGunProjectile(Tank owner, GameCore game, bool authorized = false,
            Vector2 position = default(Vector2), float rotation = 0)
            : base(owner, game, authorized, 1, 1.1f, position, rotation)
        {
            ColorMask = owner.ColorMask;
        }

        protected override Body CreateBody(World world, Vector2 size, Vector2 position, float rotation)
        {
            return BodyFactory.CreateCircle(world, size.Length() / 2, 1, position, BodyType.Dynamic, this);
        }
        protected override void CreateInternal()
        {
            base.CreateInternal();
        }

        protected override bool DestroyInternal(GameObject destroyer = null)
        {
            var cMask = ColorMask;
            if (destroyer != null && destroyer.ColorMask != Color.Black)
                cMask = destroyer.ColorMask;

            //Set the color of the spark emitter to the color of the gameObject we hit.
            Emitters["explosion_spark_emitter"].MinColorMask =
               new Color(Emitters["explosion_spark_emitter"].MinColorMask.ToVector4() * cMask.ToVector4());
            Emitters["explosion_spark_emitter"].MaxColorMask =
               new Color(Emitters["explosion_spark_emitter"].MaxColorMask.ToVector4() * cMask.ToVector4());

            return false;
        }
    }
}
