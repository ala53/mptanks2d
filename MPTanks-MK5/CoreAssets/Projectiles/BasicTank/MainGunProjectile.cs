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

namespace MPTanks.CoreAssets.Projectiles.BasicTank
{
    [Projectile("BasicTankMPMainProjectile", "basictankmainprojectile.json", "BasicTankMP", DisplayName = "Main gun projectile for Basic Tank")]
    public class MainGunProjectile : Projectile
    {
        /// <summary>
        /// The amount of damage this projectile does.
        /// </summary>
        public override int DamageAmount
        {
            get { return 60; }
        }
        public MainGunProjectile(Tank owner, GameCore game, bool authorized = false,
            Vector2 position = default(Vector2), float rotation = 0)
            : base(owner, game, authorized, 1, 1.2f, position, rotation)
        {
            ColorMask = owner.ColorMask;
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
