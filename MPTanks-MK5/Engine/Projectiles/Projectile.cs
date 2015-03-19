using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Projectiles
{
    public abstract class Projectile : GameObject
    {
        public abstract int DamageAmount { get; }

        public Tanks.Tank Owner { get; private set; }

        public Projectile(Tanks.Tank owner, GameCore game, bool authorized, float density = 1,
            float bounciness = 0.1f, Vector2 position = default(Vector2), float rotation = 0)
            : base(game, authorized, density, bounciness, position, rotation)
        {
            Owner = owner;
        }

        protected override void CreateInternal()
        {
            Body.IsBullet = true;
            base.CreateInternal();
        }

        abstract public void CollidedWithTank(Tanks.Tank tank);
    }
}
