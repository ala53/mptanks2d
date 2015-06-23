﻿using MPTanks.Engine.Core.Timing;
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
using MPTanks.Engine.Helpers;

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
        public BasicTank(GamePlayer player, GameCore game, bool authorized = false)
            : base(player, game, authorized)
        {
            Health = 150;
            Size = new Vector2(3, 5);

            PrimaryWeapon = new Weapon(this)
            {
                AddedRotation = 0,
                FireRotationIsRelativeToTankLookDirection = true,
                FireRotationIsRelativeToTankRotation = false,
                MaxActiveProjectileCount = 5,
                MaxDistance = 50,
                ProjectileOffset = new Vector2(1.5f, -1.1f),
                ProjectileVelocity = new Vector2(0, -60),
                ProjectileReflectionName = "BasicTankMPMainProjectile",
                TargetingType = Weapon.WeaponTargetingType.Directional,
                TransformPositionAndVelocityByRotation = true,
                WeaponName = "120mm cannon",
                WeaponDisplaySprite = Sprites["tank_cannon"],
                WeaponRechargeTimeMs = 500
            };
            SecondaryWeapon = new Weapon(this)
            {
                AddedRotation = 0,
                FireRotationIsRelativeToTankLookDirection = true,
                FireRotationIsRelativeToTankRotation = false,
                MaxActiveProjectileCount = 5,
                MaxDistance = 50,
                ProjectileOffset = new Vector2(1.5f, -1.1f),
                ProjectileVelocity = new Vector2(0, -60),
                ProjectileReflectionName = "BasicTankMPMainProjectile",
                TargetingType = Weapon.WeaponTargetingType.Directional,
                TransformPositionAndVelocityByRotation = true,
                WeaponName = "Duct Tape and Magical Caterpillars",
                WeaponDisplaySprite = Sprites["tank_cannon"],
                WeaponRechargeTimeMs = 2000
            };
            TertiaryWeapon = new Weapon(this)
            {
                AddedRotation = 0,
                FireRotationIsRelativeToTankLookDirection = true,
                FireRotationIsRelativeToTankRotation = false,
                MaxActiveProjectileCount = 5,
                MaxDistance = 50,
                ProjectileOffset = new Vector2(1.5f, -1.1f),
                ProjectileVelocity = new Vector2(0, -60),
                ProjectileReflectionName = "BasicTankMPMainProjectile",
                TargetingType = Weapon.WeaponTargetingType.Directional,
                TransformPositionAndVelocityByRotation = true,
                WeaponName = "Quantum Teleporter With Space Monkeys",
                WeaponDisplaySprite = Sprites["tank_cannon"],
                WeaponRechargeTimeMs = 3000
            };
        }
        private Timer primaryTimer;
        private Timer secondaryTimer;
        private Timer tertiaryTimer;

        protected override void UpdateInternal(GameTime time)
        {
            //handle turret rotation
            ComponentGroups["turret"].Rotation = InputState.LookDirection - Rotation;

            if (Alive && Authoritative)
            {
            }
            base.UpdateInternal(time);
        }

        private void FirePrimary()
        {
            if (!primaryTimer.Completed)
                return;
            const float velocity = 60f;
            var rotation = InputState.LookDirection;
            //Spawn a projectile
            SpawnProjectile("BasicTankMPMainProjectile",
                TransformPoint(new Vector2(1.5f, -1.1f), rotation, true), rotation,
                velocity * new Vector2((float)Math.Sin(rotation), -(float)Math.Cos(rotation)));

            //and reload
            primaryTimer.Reset();

            RaiseStateChangeEvent("weapon 1 fired");
        }
        private void FireSecondary()
        {
            if (!secondaryTimer.Completed)
                return;
            const float velocity = 60f;
            var rotation = InputState.LookDirection;
            //Spawn a projectile
            SpawnProjectile("BasicTankMPMainProjectile",
                TransformPoint(new Vector2(1.5f, -1.1f), rotation, true), rotation,
                velocity * new Vector2((float)Math.Sin(rotation), -(float)Math.Cos(rotation)));

            //reload
            secondaryTimer.Reset();

            RaiseStateChangeEvent("weapon 2 fired");
        }

        private void FireTertiary()
        {
            if (!tertiaryTimer.Completed)
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

            //and reload
            tertiaryTimer.Reset();

            RaiseStateChangeEvent("weapon 3 fired");
        }

        protected override bool DestroyInternal(GameObject destructor = null)
        {
            var si = AnimatedSprites[BasicHelpers.ChooseRandom(_explosions)];
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

        protected override void ReceiveStateDataInternal(string state)
        {
            if (state == "weapon 1 fired")
                primaryTimer.Reset();
            else if (state == "weapon 2 fired")
                secondaryTimer.Reset();
            else if (state == "weapon 3 fired")
                tertiaryTimer.Reset();
        }

    }
}
