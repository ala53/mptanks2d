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

        private Core.Timing.Timer _timeoutTimer;
        public MainGunProjectile(Tanks.Tank owner, GameCore game, bool authorized = false,
            Vector2 position = default(Vector2), float rotation = 0)
            : base(owner, game, authorized, 1, 0f, position, rotation)
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
            _timeoutTimer = Game.TimerFactory.CreateTimer((timer) => Destroy(), 5000);
        }

        public override void CollidedWithTank(Tanks.Tank tank)
        {
            Destroy(tank);
        }

        private void Destroy(GameObject destroyer = null)
        {

            for (var i = 0; i < 50; i++)
            {
                var dir = (float)(Game.SharedRandom.NextDouble() - 0.5f) * 2 - Rotation;
                var vx = (float)Math.Sin(dir) * (float)(Game.SharedRandom.NextDouble() * 2);
                var vy = (float)-Math.Cos(dir) * (float)(Game.SharedRandom.NextDouble() * 2);
                var particle = new Rendering.Particles.Particle()
                {
                    Acceleration = Vector2.Zero,
                    LifespanMs = Game.SharedRandom.Next(15, 150),
                    Velocity = new Vector2(vx, vy),
                    Size = new Vector2(0.25f, 0.25f),
                    Rotation = dir,
                    RotationVelocity = (float)(Game.SharedRandom.NextDouble() / 2),
                    ColorMask = new Color(Color.Yellow, 0.5f),
                    AssetName = Assets.BasicTank.MainGunSparks.SpriteName,
                    SheetName = Assets.BasicTank.MainGunSparks.SheetName,
                    Position = Position + new Vector2(vx, vy)
                };
                Game.ParticleEngine.AddParticle(particle);
            }
            Game.TimerFactory.RemoveTimer(_timeoutTimer);
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
