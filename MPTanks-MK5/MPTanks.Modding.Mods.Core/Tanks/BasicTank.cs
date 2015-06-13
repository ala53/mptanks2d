using MPTanks.Engine.Core.Timing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Engine.Tanks;
using MPTanks.Engine;
using MPTanks.Engine.Gamemodes;
using MPTanks.Engine.Assets;

namespace MPTanks.Modding.Mods.Core.Tanks
{
    [Tank("BasicTankMP", "assets/components/basictank.json",
        DisplayName = "Basic Tank", Description = "Basic Tank",
        RequiresMatchingOnOtherTeam = false)]
    public class BasicTank : Tank
    {
        private string[] _explosions = { "explosion1", "explosion2", "explosion3" };

        protected override float RotationSpeed
        {
            get { return 0.05f; }
        }

        protected override float MovementSpeed
        {
            get { return 50; }
        }
        public BasicTank(GamePlayer player, Team team, GameCore game, bool authorized = false)
            : base(player, team, game, authorized)
        {
            Health = 150;
            Size = new Vector2(3, 5);
        }

        private bool canFirePrimary = true;
        private bool canFireSecondary = true;
        private bool canFireTertiary = true;
        protected override void UpdateInternal(GameTime time)
        {
            //handle turret rotation
            Components["turret"].Rotation = InputState.LookDirection - Rotation;
            Components["turretBase"].Rotation = InputState.LookDirection - Rotation;
            Components["turretDoor"].Rotation = InputState.LookDirection - Rotation;

            if (Alive && Authoritative)
            {
                if (InputState.FirePressed && InputState.WeaponNumber == 0)
                    FirePrimary();
                if (InputState.FirePressed && InputState.WeaponNumber == 1)
                    FireSecondary();
            }
            base.UpdateInternal(time);
        }

        private void FirePrimary()
        {
            if (!canFirePrimary)
                return;
            const float velocity = 60f;
            var rotation = InputState.LookDirection;
            //Spawn a projectile
            SpawnProjectile("BasicTankMPMainProjectile",
                TransformPoint(new Vector2(1.5f, -1.1f), rotation, true), rotation,
                velocity * new Vector2((float)Math.Sin(rotation), -(float)Math.Cos(rotation)));

            //Reload timer
            canFirePrimary = false;
            Game.TimerFactory.CreateTimer((timer) => canFirePrimary = true, 500);
        }
        private void FireSecondary()
        {
            if (!canFireSecondary)
                return;
            const float velocity = 60f;
            var rotation = InputState.LookDirection;
            //Spawn a projectile
            SpawnProjectile("BasicTankMPMainProjectile",
                TransformPoint(new Vector2(1.5f, -1.1f), rotation, true), rotation,
                velocity * new Vector2((float)Math.Sin(rotation), -(float)Math.Cos(rotation)));

            //Reload timer
            canFireSecondary = false;
            Game.TimerFactory.CreateTimer((timer) => canFireSecondary = true, 500);
        }

        private void FireTertiary()
        {
            if (!canFireTertiary)
                return;
            if (Game.Authoritative) // If we are able to be create game objects AKA we're authoritative, make the projectile
            {
                const float velocity = 60f;
                var rotation = InputState.LookDirection;
                //Spawn a projectile
                SpawnProjectile("BasicTankMPMainProjectile",
                    TransformPoint(new Vector2(1.5f, -1.1f), rotation, true), rotation,
                    velocity * new Vector2((float)Math.Sin(rotation), -(float)Math.Cos(rotation)));
            }

            //Reload timer
            canFireTertiary = false;
            Game.TimerFactory.CreateTimer((timer) => canFireTertiary = true, 500);
        }

        protected override bool DestroyInternal(GameObject destructor = null)
        {
            var si = AnimatedSprites[Helpers.ChooseRandom(_explosions)];
            var anim = new Engine.Rendering.Animations.Animation(
                    si.AnimationName, Position, new Vector2(10), si.SheetName);

            Game.AnimationEngine.AddAnimation(anim);

            Game.TimerFactory.CreateReccuringTimer((timer) =>
            {
                if (timer.ElapsedMilliseconds > 500)
                    Game.TimerFactory.RemoveTimer(timer);

                anim.Position = Position;
            }, 1);

            return true;
        }
    }
}
