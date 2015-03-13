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
        /// The number of milliseconds this projectile lives
        /// </summary>
        const float lifespan = 2000;

        /// <summary>
        /// The amount of damage this projectile does.
        /// </summary>
        public override int DamageAmount
        {
            get { return 60; }
        }

        private Rendering.Particles.ParticleEngine.Emitter _trailEmitter;
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

            //Create the trail emitter
            _trailEmitter = Game.ParticleEngine.CreateEmitter(0.15f, Assets.BasicTank.MainGunSparks, Color.Orange,
                new Core.RectangleF(Position.X - 0.05f, Position.Y - 0.05f, 0.1f, 0.1f),
                new Vector2(0.25f), true, 0, 100, 150, Vector2.Zero, Vector2.Zero, 0, 0.15f, 240, lifespan + 100);

            //Add a timer for so we don't exist forever
            _timeoutTimer = Game.TimerFactory.CreateTimer((timer) => Destroy(), lifespan);
        }

        public override void CollidedWithTank(Tanks.Tank tank)
        {
            Destroy(tank);
        }

        private void Destroy(GameObject destroyer = null)
        {
            //Make sure to kill the emitter
            _trailEmitter.Kill();

            //Spawn the destruction sparks - a bit chaotic and random, much like their source code
            var rect = new Core.RectangleF(Position.X - 0.15f, Position.Y - 0.15f, 0.3f, 0.3f);
            //Generate the particle emitter
            var explosion = Game.ParticleEngine.CreateEmitter(
                //         Asset                          Fade in    Fade out
                new[] { Assets.BasicTank.MainGunSparks }, 20f, 40f, 30, 75,
                //Lifespan|Area             Velocity
                150, 350, rect, new Vector2(0.25f), new Vector2(4),
                //       Accel            A & V relate to Rotation
                new Vector2(0), new Vector2(0), true,
                //           Size
                new Vector2(0.2f), new Vector2(0.3f),
                //                 Colors
                new Color(Color.Yellow, 0.5f), new Color(Color.DarkOrange, 1f),
                //    Rotation
                0, (float)Math.PI * 2,
                // R Vel      Rate      Emitter life
                0.05f, 0.5f, 75, 150, 200);

            Game.TimerFactory.RemoveTimer(_timeoutTimer);
            Game.RemoveGameObject(this, destroyer);
        }

        public override Microsoft.Xna.Framework.Vector2 Size
        {
            get { return new Vector2(0.25f); }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            //Move the particle emitter
            _trailEmitter.EmissionArea = new Core.RectangleF(Position.X - 0.05f, Position.Y - 0.05f, 0.1f, 0.1f);
        }
    }
}
