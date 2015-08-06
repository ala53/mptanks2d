using Microsoft.Xna.Framework;
using MPTanks.Engine.Assets;
using MPTanks.Engine.Core.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Engine.Helpers;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace MPTanks.Engine.Tanks
{
    public class Weapon
    {
        public static Weapon Null
        { get; private set; }
        = new Weapon
        {
            MaxActiveProjectileCount = 0,
            MaxDistance = 0,
            ProjectileReflectionName = "NULL",
            TargetingType = WeaponTargetingType.Directional,
            WeaponName = "NO WEAPONS AVAILABLE",
            WeaponRechargeTimeMs = float.PositiveInfinity,
            _isNullWeapon = true
        };

        public enum WeaponTargetingType
        {
            /// <summary>
            /// The projectile is fired in a direction with a nondeterminate final position.
            /// </summary>
            Directional,
            /// <summary>
            /// The projectile is fired and spawned at a specific point on the map (x/y position)
            /// </summary>
            Targeted,
        }

        public WeaponTargetingType TargetingType { get; set; } = WeaponTargetingType.Directional;

        /// <summary>
        /// The distance of the directional line or the maximum distance from the tank that the projectile
        /// can be spawned at. (UI bounding)
        /// </summary>
        public float MaxDistance { get; set; }
        /// <summary>
        /// The reflection name of the projectile / the name to pass to GameCore::AddProjectile()
        /// </summary>
        public string ProjectileReflectionName { get; set; }
        /// <summary>
        /// The velocity to fire the projectile at.
        /// </summary>
        public Vector2 ProjectileVelocity { get; set; }
        /// <summary>
        /// The offset to fire the projectile from, relative to the tank.
        /// </summary>
        public Vector2 ProjectileOffset { get; set; }
        /// <summary>
        /// The initial rotation to fire the projectile at.
        /// </summary>
        public float ProjectileRotation { get; set; }
        /// <summary>
        /// The initial angular velocity to give to the projectile.
        /// </summary>
        public float ProjectileRotationVelocity { get; set; }
        /// <summary>
        /// The maximum number of projectiles allowed to be spawned at any one point.
        /// </summary>
        public ushort MaxActiveProjectileCount { get; set; } = ushort.MaxValue; //no reasonable max (lag sets in before we hit this)
        /// <summary>
        /// Whether the computed rotation for the projectile is relative to the rotation of the tank object. 
        /// </summary>
        public bool FireRotationIsRelativeToTankRotation { get; set; } = true;
        /// <summary>
        /// Whether the computed rotation for the projectile is relative to the turret look direction for the
        /// tank.
        /// </summary>
        public bool FireRotationIsRelativeToTankLookDirection { get; set; } = true;
        /// <summary>
        /// The amount of radians of rotation to add to transformation computations.
        /// </summary>
        public float AddedRotation { get; set; }
        /// <summary>
        /// Whether to use the computed rotation to recalculate velocity and position information. For example, 
        /// a rotated tank or barrel will need this.
        /// </summary>
        public bool TransformPositionAndVelocityByRotation { get; set; } = true;
        /// <summary>
        /// The number of milliseconds needed to recharge the weapon.
        /// </summary>
        public float WeaponRechargeTimeMs { get; set; } = 1000;
        /// <summary>
        /// The recharge percentage from 0-1 inclusive of the weapon.
        /// </summary>
        public float CurrentRechargePercentage
        {
            get { return MathHelper.Clamp(TimeRechargedMs / WeaponRechargeTimeMs, 0, 1); }
        }
        /// <summary>
        /// The picture of the sprite to display on screen. (recommended 256x256 image or animation)
        /// </summary>
        public SpriteInfo WeaponDisplaySprite { get; set; } = new SpriteInfo("null.png", "null");
        /// <summary>
        /// The OSD name for the weapon.
        /// </summary>
        public string WeaponName { get; set; } = "NULL";
        /// <summary>
        /// The tank that owns the weapon.
        /// </summary>
        public Tank Owner { get; private set; }
        /// <summary>
        /// The game instance that this weapon is bound to.
        /// </summary>
        public GameCore Game { get; private set; }
        /// <summary>
        /// Whether the weapon is ready to fire or if it is still recharging.
        /// </summary>
        public bool Recharged { get { return CurrentRechargePercentage >= 0.999; } }
        /// <summary>
        /// Called when a targeted weapon needs the targeting UI shown
        /// </summary>
        public event EventHandler<TargetingUIArgs> OnShowTargetingUIRequested = delegate { };
        private TargetingUIArgs _targetingUIInstance = new TargetingUIArgs();

        public Action<Weapon, Projectiles.Projectile> FireCallback = delegate { };

        public byte[] FullState { get { return GetFullState(); } set { SetFullState(value); } }

        public class TargetingUIArgs : EventArgs
        {
            public Weapon Weapon { get; set; }
            public float MaxDistance { get; set; }
            public Action<Vector2> CompletionCallback { get; set; } = delegate { };
            public Action CancelCallback { get; set; } = delegate { };
        }
        private float TimeRechargedMs;

        private bool _isNullWeapon;

        private List<Projectiles.Projectile> _projectiles = new List<Projectiles.Projectile>();
        public IReadOnlyList<Projectiles.Projectile> Projectiles { get { return _projectiles; } }

        public Weapon(Tank tank)
        {
            Owner = tank;
            Game = tank.Game;
        }

        private Weapon()
        {
            _isNullWeapon = true;
        }

        public byte[] GetFullState()
        {
            if (_isNullWeapon)
                return new[] { SerializationHelpers.FalseByte };
            else
                return SerializationHelpers.AllocateArray(true,
                      _isNullWeapon,
                      (byte)TargetingType,
                      MaxDistance,
                      ProjectileReflectionName,
                      new HalfVector2(ProjectileVelocity),
                      new HalfVector2(ProjectileOffset),
                      (Half)ProjectileRotation,
                      (Half)ProjectileRotationVelocity,
                      MaxActiveProjectileCount,
                      FireRotationIsRelativeToTankRotation,
                      FireRotationIsRelativeToTankLookDirection,
                      (Half)AddedRotation,
                      TransformPositionAndVelocityByRotation,
                      WeaponRechargeTimeMs,
                      WeaponName,
                      TimeRechargedMs,
                      SerializationHelpers.AllocateArray(true, _projectiles.Select(p => (object)p.ObjectId).ToArray())
                      );
        }

        private byte[] _projectileArray;
        public void SetFullState(byte[] state)
        {
            int offset = 0;
            var isNull = state.GetBool(offset); offset++;
            if (isNull)
            {
                _isNullWeapon = true;
                return;
            }
            else _isNullWeapon = false;
            TargetingType = (WeaponTargetingType)state[offset]; offset++;
            MaxDistance = state.GetFloat(offset); offset += 4;
            ProjectileReflectionName = state.GetString(offset); offset += state.GetUShort(offset); offset += 2;
            ProjectileVelocity = state.GetHalfVector(offset).ToVector2(); offset += 4;
            ProjectileOffset = state.GetHalfVector(offset).ToVector2(); offset += 4;
            ProjectileRotation = state.GetHalf(offset); offset += 2;
            ProjectileRotationVelocity = state.GetHalf(offset); offset += 2;
            MaxActiveProjectileCount = state.GetUShort(offset); offset += 2;
            FireRotationIsRelativeToTankRotation = state.GetBool(offset); offset++;
            FireRotationIsRelativeToTankLookDirection = state.GetBool(offset); offset++;
            AddedRotation = state.GetHalf(offset); offset += 2;
            TransformPositionAndVelocityByRotation = state.GetBool(offset); offset++;
            WeaponRechargeTimeMs = state.GetFloat(offset); offset += 4;
            WeaponName = state.GetString(offset); offset += state.GetUShort(offset); offset += 2;
            TimeRechargedMs = state.GetFloat(offset); offset += 4;

            _projectileArray = state.GetByteArray(offset); offset += state.GetUShort(offset); offset += 2;
        }

        public void DeferredSetFullState()
        {
            _projectiles.Clear();
            for (var i = 0; i < _projectileArray.Length; i += 2)
                _projectiles.Add((Projectiles.Projectile)Game.GameObjectsById[_projectileArray.GetUShort(i)]);
        }

        private bool _isWaitingForTarget = false;
        public virtual void Fire(Vector2? spawnPosition = null, Vector2? velocity = null)
        {
            if (_isNullWeapon || _isWaitingForTarget || !Recharged ||
                MaxActiveProjectileCount <= Projectiles.Count)
                return;

            if (TargetingType == WeaponTargetingType.Targeted)
            {
                _isWaitingForTarget = true;
                _targetingUIInstance.CompletionCallback = DeferredFireWithTargetingUI;
                _targetingUIInstance.CancelCallback = () => _isWaitingForTarget = false;
                _targetingUIInstance.MaxDistance = MaxDistance;
                _targetingUIInstance.Weapon = this;
                OnShowTargetingUIRequested(this, _targetingUIInstance);
                return;
            }

            FireInternal(spawnPosition, velocity);
        }

        private void DeferredFireWithTargetingUI(Vector2 positionToFireAt)
        {
            FireInternal(positionToFireAt);
        }

        private void FireInternal(Vector2? spawnPosition = null, Vector2? velocity = null)
        {
            var exactRotation = AddedRotation
                + (FireRotationIsRelativeToTankLookDirection ? Owner.FireDirection : 0)
                + (FireRotationIsRelativeToTankRotation ? Owner.Rotation : 0);

            float cos = 0, sin = 0;
            cos = (float)Math.Cos(exactRotation);
            sin = (float)Math.Sin(exactRotation);

            var centered = ProjectileOffset - Owner.Size / 2;

            var exactPosition = spawnPosition ?? Owner.Position +
                new Vector2(centered.X * cos + centered.Y * -sin,
                centered.X * -sin + centered.Y * cos);

            if (!TransformPositionAndVelocityByRotation)
                exactPosition = spawnPosition ?? Owner.Position + ProjectileOffset;

            var exactVelocity = velocity ??
                new Vector2(ProjectileVelocity.X * cos + ProjectileVelocity.Y * -sin,
                ProjectileVelocity.X * -sin + ProjectileVelocity.Y * cos);

            if (!TransformPositionAndVelocityByRotation)
                exactVelocity = velocity ?? ProjectileVelocity;

            var prj = Game.AddProjectile(ProjectileReflectionName, Owner, Game.Authoritative);
            prj.Position = exactPosition;
            prj.LinearVelocity = exactVelocity;
            prj.Rotation = ProjectileRotation;
            prj.AngularVelocity = ProjectileRotationVelocity;

            _projectiles.Add(prj);

            FireCallback(this, prj);

            TimeRechargedMs = 0;
        }

        private List<Projectiles.Projectile> _removeQueue = new List<Projectiles.Projectile>();
        public virtual void Update(GameTime time)
        {
            if (_isNullWeapon) return;
            TimeRechargedMs += (float)(time.ElapsedGameTime.TotalMilliseconds);

            foreach (var prj in Projectiles)
                if (!prj.Alive) _removeQueue.Add(prj);

            foreach (var prj in _removeQueue)
                _projectiles.Remove(prj);

            _removeQueue.Clear();
        }
    }
}
