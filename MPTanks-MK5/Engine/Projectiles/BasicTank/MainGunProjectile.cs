using Engine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Projectiles.BasicTank
{
    public class MainGunProjectile : Projectile
    {
        /// <summary>
        /// The amount of damage this projectile does.
        /// </summary>
        public override int DamageAmount
        {
            get { return 60; }
        }

        public MainGunProjectile(Tanks.Tank owner, GameCore game,
            Vector2 position = default(Vector2), float rotation = 0)
            : base(owner, game, 1, 0f, position, rotation)
        {
            Components.Add("bullet", new Rendering.RenderableComponent()
            {
                SpriteSheetName = Assets.BasicTank.MainProjectile.SheetName,
                AssetName = Assets.AssetHelper.AnimationToString(Assets.BasicTank.MainProjectile, 0, true),
                Size = new Vector2(0.5f),
                Rotation = MathHelper.ToRadians(45),
                RotationOrigin = new Vector2(0.25f),
                Offset = new Vector2(-0.125f),
                Mask = new Color(Color.Red, 0.8f)
            });

            //Add a timer for so we don't exist forever
            Game.TimerFactory.CreateTimer((timer) => Destroy(), 5000);
        }

        public override void CollidedWithTank(Tanks.Tank tank)
        {
            Destroy(tank);
        }

        private void Destroy(GameObject destroyer = null)
        {
            Game.RemoveGameObject(this, destroyer);
        }

        public override Microsoft.Xna.Framework.Vector2 Size
        {
            get { return new Vector2(0.25f); }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
        }
    }
}
