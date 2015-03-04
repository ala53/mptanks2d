using Engine.Core.Timing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Tanks
{
    public class BasicTank : Tank
    {
        public BasicTank(Guid playerId, GameCore game)
            : base(playerId, game)
        {
            AddComponents();
            SetUpBody();
        }

        public BasicTank(Guid playerId, GameCore game, byte[] savedState)
            : base(playerId, game)
        {


            AddComponents();
            SetUpBody();
            //TODO rebuild the state here
        }
        private void SetUpBody()
        {
            //        Body.LinearDamping = 10;
            //      Body.AngularDamping = 10;
            Health = 150;
        }

        private void AddComponents()
        {
            Components.Add("base", new Rendering.RenderableComponent()
            {
                Size = new Vector2(3, 5),
                Mask = new Color(Color.DarkGreen, 200)
            });
            Components.Add("tankFront", new Rendering.RenderableComponent()
            {
                Size = new Vector2(3f, 0.25f),
                Mask = new Color(Color.DarkGreen, 200),
                Offset = new Vector2(0, 0),
                AssetName = "grillmask",
                SpriteSheetName = "assets/tanks/basictank2d.png",
                Rotation = 0
            });
            Components.Add("turret", new Rendering.RenderableComponent()
            {
                Size = new Vector2(0.5f, 2.35f),
                Mask = new Color(Color.Red, 150),
                Offset = new Vector2(1.25f, -1f),
                RotationOrigin = new Vector2(0.25f, 3.5f),
                Rotation = 0
            });
            Components.Add("turretBase", new Rendering.RenderableComponent()
            {
                Size = new Vector2(2, 2.5f),
                Mask = new Color(Color.Green, 200),
                Offset = new Vector2(0.5f, 1.5f),
                RotationOrigin = new Vector2(1f, 1f),
                AssetName = "turretbase",
                SpriteSheetName = "assets/tanks/basictank2d.png",
                Rotation = 0
            });
            Components.Add("turretDoor", new Rendering.RenderableComponent()
            {
                Size = new Vector2(0.65f, 0.65f),
                Mask = new Color(Color.Yellow, 100),
                Offset = new Vector2(0.75f, 1.75f),
                RotationOrigin = new Vector2(0.75f, 0.75f),
                Rotation = 0
            });
        }

        private bool canFirePrimary = true;
        public override void Update(GameTime time)
        {
            //handle turret rotation
            Components["turret"].Rotation = InputState.LookDirection - Rotation;
            Components["turretBase"].Rotation = InputState.LookDirection - Rotation;
            Components["turretDoor"].Rotation = InputState.LookDirection - Rotation;


            if (InputState.FireState && InputState.WeaponNumber == 0 && canFirePrimary)
            {
                FirePrimary();
                //Mark the primary as fired are reload
                canFirePrimary = false;
                Game.TimerFactory.CreateTimer((timer) => canFirePrimary = true, 500);
            }
            base.Update(time);
        }

        private void FirePrimary()
        {
            var rotation = InputState.LookDirection;
            const float velocity = 0.2f;
            var x = velocity * (float)Math.Sin(rotation);
            var y = velocity * -(float)Math.Cos(rotation);
            //Spawn a projectile
            var projectile = new Projectiles.BasicTank.MainGunProjectile(
                this, Game,
                TransformPoint(new Vector2(1.5f, -0f), rotation, true), rotation);
            projectile.Body.ApplyForce(new Vector2(x, y));

            Game.AddGameObject(projectile, this);
        }

        protected override void TankKilled(GameObject obj)
        {
            var anim = new Rendering.Animation(
                "explosionAnim", Position, new Vector2(10), "assets/animations/explosion.png");
            Game.Animations.AddAnimation(anim);

            Game.TimerFactory.CreateReccuringTimer((Action<Timer>)((timer) =>
            {
                if (timer.ElapsedMilliseconds > 500)
                    Game.TimerFactory.RemoveTimer(timer);

                anim.Position = Position;
            }), 1);

            Game.TimerFactory.CreateTimer((timer) => Game.RemoveGameObject(this, obj), 500);
        }

        public override byte[] GetData()
        {
            return null;
        }

        public override Vector2 Size
        {
            get { return new Vector2(3, 5); }
        }

        protected override float RotationSpeed
        {
            get { return 0.05f; }
        }

        protected override float MovementSpeed
        {
            get { return 4; }
        }
    }
}
