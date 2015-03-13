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
            : base(owner, game, authorized, 1, 1.2f, position, rotation)
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
            _timeoutTimer = Game.TimerFactory.CreateTimer((timer) => Destroy(), 2000);
        }

        private int totalHits = 0;

        public override void CollidedWithTank(Tanks.Tank tank)
        {
            totalHits++;
            if (totalHits >= 2) Destroy(tank);
        }

        private void Destroy(GameObject destroyer = null)
        {
            //Spawn the destruction sparks - a bit chaotic and random, much like their source code
            for (var i = 0; i < 50; i++)
            {
                var dir = (float)(Game.SharedRandom.NextDouble() * 2 * Math.PI);
                var vx = (float)Math.Sin(dir) * (float)(Game.SharedRandom.NextDouble() * 4);
                var vy = (float)-Math.Cos(dir) * (float)(Game.SharedRandom.NextDouble() * 4);
                var particle = new Rendering.Particles.Particle()
                {
                    Acceleration = Vector2.Zero,
                    LifespanMs = Game.SharedRandom.Next(15, 350),
                    FadeOutMs = (float)Game.SharedRandom.NextDouble() * 75,
                    Velocity = new Vector2(vx, vy),
                    Size = new Vector2(0.25f, 0.25f),
                    Rotation = dir,
                    RotationVelocity = (float)(Game.SharedRandom.NextDouble() / 2),
                    ColorMask = new Color(Color.Yellow, 0.5f),
                    AssetName = Assets.BasicTank.MainGunSparks.SpriteName,
                    SheetName = Assets.BasicTank.MainGunSparks.SheetName,
                    Position = Position + new Vector2(vx, vy) - new Vector2(vx, vy)
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
            var linVel = Body.LinearVelocity / Settings.PhysicsScale;
            //Each tick, we create a small particle trail
            for (var i = 0; i < 3; i++)
            {
                var particle = new Rendering.Particles.Particle()
                    {
                        AssetName = Assets.BasicTank.MainGunSparks.SpriteName,
                        SheetName = Assets.BasicTank.MainGunSparks.SheetName,
                        RotationVelocity = 0.15f,
                        LifespanMs = 10,
                        FadeOutMs = 500,
                        ColorMask = Color.Red,
                        Position = Position + ((linVel / i) * (float)time.ElapsedGameTime.TotalSeconds),
                        Size = new Vector2(0.25f),

                    };
                Game.ParticleEngine.AddParticle(particle);
            }
        }
    }
}
