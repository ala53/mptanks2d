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
using MPTanks.Engine.Helpers;
using MPTanks.Modding;

namespace MPTanks.CoreAssets.Tanks
{
    [Tank("BasicTankMP", "basictank.json",
        DisplayName = "Basic Tank", Description = "Basic Tank")]
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
            
        }

        protected override void CreateInternal()
        {
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
                WeaponRechargeTimeMs = 500,
                FireCallback = (w, p) => InvokeTrigger("primary_fired")
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
                WeaponRechargeTimeMs = 2000,
                FireCallback = (w, p) => InvokeTrigger("secondary_fired")
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
                WeaponRechargeTimeMs = 3000,
                FireCallback = (w, p) => InvokeTrigger("tertiary_fired")
            };
            Animations["death_explosion"].Mask = ColorMask;
        }

        protected override void UpdateInternal(GameTime time)
        {
            //handle turret rotation
            ComponentGroups["turret"].Rotation = InputState.LookDirection - Rotation;

            if (Alive && Authoritative)
            {
            }
            base.UpdateInternal(time);
        }
        protected override void ReceiveStateDataInternal(string state)
        {
        }

        public void OnPrimary_Fired()
        {
            Game.SoundEngine.PlaySound(Assets["primary_fire_sound"], true, Position, 0);
        }
    }
}
