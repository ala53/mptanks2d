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
        public MainGunProjectile(Tanks.Tank owner, GameCore game, Color colorMask, bool authorized = false,
            Vector2 position = default(Vector2), float rotation = 0)
            : base(owner, game, authorized, 1, 1.2f, position, rotation)
        {
            ColorMask = colorMask;

            Components.Add("bullet", new Rendering.RenderableComponent()
            {
                SpriteSheetName = Assets.BasicTank.MainProjectile.SheetName,
                AssetName = Assets.AssetHelper.AnimationToString(Assets.BasicTank.MainProjectile, 0, true),
                Size = new Vector2(0.5f),
                Rotation = MathHelper.ToRadians(45),
                RotationOrigin = new Vector2(0.25f),
                Offset = new Vector2(-0.125f),
                Mask = new Color(Color.White, 0.8f)
            });

            // Create the trail emitter
            _trailEmitter = Game.ParticleEngine.CreateEmitter(0.15f, Assets.SmokePuffs.SmokePuffSprites,
                new Color(new Color(255, 200, 255).ToVector4() * ColorMask.ToVector4()),
                new Core.RectangleF(Position.X - 0.125f, Position.Y - 0.125f, 0.25f, 0.25f),
                new Vector2(0.5f), true, 35, 100, 50, Vector2.Zero, Vector2.Zero, 0, 0.15f, 60, lifespan + 100, true, true);
            _trailEmitter.MinSize = new Vector2(0.1f);
            _trailEmitter.MaxSize = new Vector2(0.75f);
            ////Add a timer for so we don't exist forever
            _timeoutTimer = Game.TimerFactory.CreateTimer((timer) => CollidedWithTank(null), lifespan);
        }

        public override void CollidedWithTank(Tanks.Tank tank)
        {
            Game.RemoveGameObject(this, tank);
        }

        protected override bool DestroyInternal(GameObject destroyer = null)
        {
            var cMask = ColorMask;
            if (destroyer != null && destroyer.ColorMask != Color.Black)
                cMask = destroyer.ColorMask;
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
                new Color(cMask, 0.5f),
                new Color(cMask.ToVector3() * new Vector3(0.7f, 0.9f, 0.8f)),
                //    Rotation
                0, (float)Math.PI * 2,
                // R Vel      Rate      Emitter life
                0.05f, 0.5f, 75, 150, 200, false, false);

            Game.TimerFactory.RemoveTimer(_timeoutTimer);

            return false;
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
