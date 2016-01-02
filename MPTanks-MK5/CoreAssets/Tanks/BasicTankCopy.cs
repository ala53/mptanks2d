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
    [Tank("basictank.json",
        DisplayName = "Basic Tank (Copy)", Description = "Copy of the \"Basic Tank\" for testing purposes")]
    public class BasicTankCopy : Tank
    {
        private string[] _explosions = { "explosion1", "explosion2", "explosion3" };

        public override float FireDirection => Components["turret"].Rotation + Rotation;

        protected override float RotationSpeed
        {
            get { return 0.03f; }
        }

        protected override float MovementSpeed
        {
            get { return 30; }
        }
        public BasicTankCopy(GamePlayer player, GameCore game, bool authorized = false)
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
                ProjectileOffset = new Vector2(0f, -0f),
                ProjectileVelocity = new Vector2(0, -60),
                ProjectileType = typeof(Projectiles.BasicTank.MainGunProjectile),
                TargetingType = Weapon.WeaponTargetingType.Directional,
                TransformPositionAndVelocityByRotation = true,
                WeaponName = "120mm cannon",
                WeaponDisplaySprite = Sprites["tank_cannon"],
                WeaponRechargeTime = TimeSpan.FromMilliseconds(750),
                FireCallback = (w, p) =>
                {
                    InvokeTrigger("primary_fired");
                    Game.SoundEngine.PlaySound(Assets["primary_fire_sound"], true, Position, 0);
                }
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
                ProjectileType = typeof(Projectiles.BasicTank.MainGunProjectile),
                TargetingType = Weapon.WeaponTargetingType.Directional,
                TransformPositionAndVelocityByRotation = true,
                WeaponName = "Duct Tape and Magical Caterpillars",
                WeaponDisplaySprite = Sprites["tank_cannon"],
                WeaponRechargeTime = TimeSpan.FromMilliseconds(2000),
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
                ProjectileType = typeof(Projectiles.BasicTank.MainGunProjectile),
                TargetingType = Weapon.WeaponTargetingType.Directional,
                TransformPositionAndVelocityByRotation = true,
                WeaponName = "Quantum Teleporter With Space Monkeys",
                WeaponDisplaySprite = Sprites["tank_cannon"],
                WeaponRechargeTime = TimeSpan.FromMilliseconds(3000),
                FireCallback = (w, p) => InvokeTrigger("tertiary_fired")
            };
        }

        private float _lastStateChangeRotation;
        private float _uncorrectedRot;
        protected override void UpdateInternal(GameTime time)
        {
            var turretRotation = BasicHelpers.NormalizeAngle(TankHelper.ConstrainTurretRotation(
                null, null,
                Components["turret"].Rotation + Rotation,
                InputState.LookDirection,
                1.5f * (float)time.ElapsedGameTime.TotalSeconds
                ) - Rotation);

            ColorMask = Color.MonoGameOrange;

            //Network optimization - check if turret rotation changed without accounting for object rotation
            var uncorrected = BasicHelpers.NormalizeAngle(TankHelper.ConstrainTurretRotation(
                null, null,
                _uncorrectedRot,
                InputState.LookDirection,
                1.5f * (float)time.ElapsedGameTime.TotalSeconds
                ));
            _uncorrectedRot = uncorrected;
            ComponentGroups["turret"].Rotation = turretRotation;

            if (Authoritative && MathHelper.Distance(_lastStateChangeRotation, uncorrected) > 0.05)
            {
                RaiseStateChangeEvent(a => a.Write(turretRotation));
                _lastStateChangeRotation = uncorrected;
            }
            Animations["death_explosion"].Mask = ColorMask;
            base.UpdateInternal(time);
        }

        protected override void ReceiveStateDataInternal(ByteArrayReader reader)
        {
            //state is the rotation
            ComponentGroups["turret"].Rotation = reader.ReadFloat();
        }

    }
}
