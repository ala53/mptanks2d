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

namespace MPTanks.Modding.Mods.Core.Projectiles.BasicTank
{
    [MPTanks.Modding.Projectile(Name = "Main gun projectile for Basic Tank", 
        OwnerReflectionName = "BasicTankMP")]
    public class MainGunProjectile : Projectile
    {
        const string reflectionName = "BasicTankMPMainProjectile";
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
        public static string ReflectionTypeName
        {
            get
            {
                return reflectionName;
            }
        }

        private MPTanks.Engine.Rendering.Particles.ParticleEngine.Emitter _trailEmitter;
        private MPTanks.Engine.Core.Timing.Timer _timeoutTimer;
        public MainGunProjectile(Tank owner, GameCore game, bool authorized = false,
            Vector2 position = default(Vector2), float rotation = 0)
            : base(owner, game, authorized, 1, 1.2f, position, rotation)
        {
            Size = new Vector2(0.25f);
            ColorMask = owner.ColorMask;
        }

        protected override void AddComponents()
        {

            Components.Add("bullet", new MPTanks.Engine.Rendering.RenderableComponent()
            {
                SpriteSheetName = Assets.BasicTank.MainProjectile.SheetName,
                AssetName = MPTanks.Engine.Assets.AssetHelper.
                    AnimationToString(Assets.BasicTank.MainProjectile, 0, true),
                Size = new Vector2(0.5f),
                Rotation = MathHelper.ToRadians(45),
                RotationOrigin = new Vector2(0.25f),
                Offset = new Vector2(-0.125f),
                Mask = new Color(Color.White, 0.8f)
            });

            // Create the trail emitter
            _trailEmitter = Game.ParticleEngine.CreateEmitter(0.15f, MPTanks.Engine.Assets.SmokePuffs.SmokePuffSprites,
                new Color(new Color(255, 200, 255, 127).ToVector4() * ColorMask.ToVector4()),
                new RectangleF(Position.X - 0.125f, Position.Y - 0.125f, 0.25f, 0.25f),
                new Vector2(0.5f), true, 35, 100, 50, Vector2.Zero, 
                    Vector2.Zero, Vector2.Zero, 0, 0.15f, 250, lifespan + 100, true, true, true);
            _trailEmitter.MinSize = new Vector2(0.1f);
            _trailEmitter.MaxSize = new Vector2(0.75f);
            ////Add a timer for so we don't exist forever
            _timeoutTimer = Game.TimerFactory.CreateTimer((timer) => Game.RemoveGameObject(this, null), lifespan);
        }

        public override void CollidedWithTank(Tank tank)
        {
            Game.RemoveGameObject(this, tank);
        }

        protected override bool DestroyInternal(GameObject destroyer = null)
        {
            var cMask = ColorMask;
            if (destroyer != null && destroyer.ColorMask != Color.Black)
                cMask = destroyer.ColorMask;
            //Make sure to kill the emitter
            if (_trailEmitter != null)
                _trailEmitter.Kill();

            //Spawn the destruction sparks - a bit chaotic and random, much like their source code
            var rect = new RectangleF(Position.X - 0.15f, Position.Y - 0.15f, 0.3f, 0.3f);
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
                // R Vel      Rate      Emitter life          Emitter velocity
                0.05f, 0.5f, 75, 150, 200, false, false, true, Vector2.Zero);

            Game.TimerFactory.RemoveTimer(_timeoutTimer);

            return false;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            //We update the position before physics and the velocity after
            //or we end up drawing the smoke in front of the bullet
            if (_trailEmitter != null)
                _trailEmitter.EmissionArea = new RectangleF(Position.X - 0.05f, Position.Y - 0.05f, 0.1f, 0.1f);

        }
        public override void UpdatePostPhysics(GameTime gameTime)
        {
            //Move the particle emitter
            if (_trailEmitter != null)
                _trailEmitter.EmitterVelocity = LinearVelocity;
        }
    }
}
