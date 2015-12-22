using MPTanks.Engine.Gamemodes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Engine.Assets;
using Newtonsoft.Json;
using MPTanks.Engine.Helpers;
using MPTanks.Engine.Settings;
using MPTanks.Modding;

namespace MPTanks.Engine.Tanks
{

    public abstract class Tank : GameObject
    {
        [JsonIgnore]
        public GamePlayer Player { get; private set; }
        [JsonIgnore]
        public Team Team { get { return Player.Team; } }
        public InputState InputState { get; set; }

        protected abstract float RotationSpeed { get; }
        protected abstract float MovementSpeed { get; }

        public virtual Weapon PrimaryWeapon { get; protected set; } = Weapon.Null;
        public virtual Weapon SecondaryWeapon { get; protected set; } = Weapon.Null;
        public virtual Weapon TertiaryWeapon { get; protected set; } = Weapon.Null;
        public virtual Weapon ActiveWeapon { get; protected set; } = Weapon.Null;

        public virtual float FireDirection => InputState.LookDirection;

        private void SetActiveWeapon()
        {
            ActiveWeapon = Weapon.Null;

            if (InputState.WeaponNumber == 0)
                ActiveWeapon = PrimaryWeapon ?? SecondaryWeapon ?? TertiaryWeapon ?? Weapon.Null;
            if (InputState.WeaponNumber == 1)
                ActiveWeapon = SecondaryWeapon ?? TertiaryWeapon ?? PrimaryWeapon ?? Weapon.Null;
            if (InputState.WeaponNumber == 2)
                ActiveWeapon = TertiaryWeapon ?? PrimaryWeapon ?? SecondaryWeapon ?? Weapon.Null;
        }

        public Tank(GamePlayer player, GameCore game, bool authorized)
            : base(game, authorized, game.Settings.TankDensity, 0, default(Vector2), 0)
        {
            Player = player;
        }

        private bool _killed = false;
        protected override bool CollideInternal(GameObject other, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (other.GetType().IsSubclassOf(typeof(Projectiles.Projectile)) && other.Alive)
            {
                var o = (Projectiles.Projectile)other;

                if (!IsDamageAllowed(o.Owner) || !o.CanDamage(this, Game.FriendlyFireEnabled))
                    return true;

                //In case of friendly firing the spawning tank
                //because it spawned too close, ignore the collision to give it a chance.
                if (o.Owner == this && o.TimeAlive < TimeSpan.FromMilliseconds(500))
                    return false;

                Health -= o.DamageAmount;

                o.CollidedWithTank(this);

                if (Health <= 0 && !_killed)
                {
                    if (Authoritative) Game.RemoveGameObject(this, o);
                    _killed = true;
                }
                return true;
            }

            return base.CollideInternal(other, contact);
        }

        private bool IsDamageAllowed(Tank tank)
        {
            if (Game.FriendlyFireEnabled)
                return true;
            if (tank.Team != Team)
                return true;
            return false;
        }

        protected override void UpdateInternal(GameTime time)
        {
            Body.SleepingAllowed = false;

            UnsafeDisableEvents();
            var velocity = InputState.MovementSpeed *
                MovementSpeed;

            var rotationAmount = InputState.RotationSpeed *
                RotationSpeed * (time.ElapsedGameTime.TotalMilliseconds / 16.66666);

            //calculate the actual movement by axis
            var x = velocity * Math.Sin(Rotation);
            var y = velocity * -Math.Cos(Rotation);

            LinearVelocity = new Vector2((float)x, (float)y);
            AngularVelocity = 0;
            Rotation += (float)rotationAmount;
            UnsafeEnableEvents();
            //Set the active weapon
            SetActiveWeapon();
            //And update weapons
            PrimaryWeapon?.Update(time);
            SecondaryWeapon?.Update(time);
            TertiaryWeapon?.Update(time);

            if (ActiveWeapon.Recharged && InputState.FirePressed) ActiveWeapon.Fire();
        }

        protected override void GetTypeStateHeader(ByteArrayWriter writer)
        {
            writer.Write(PrimaryWeapon != null);
            if (PrimaryWeapon != null)
            {
                PrimaryWeapon.GetFullState(writer);
            }
            writer.Write(SecondaryWeapon != null);
            if (SecondaryWeapon != null)
            {
                SecondaryWeapon.GetFullState(writer);
            }
            writer.Write(SecondaryWeapon != null);
            if (SecondaryWeapon != null)
            {
                SecondaryWeapon.GetFullState(writer);
            }
        }
        protected override void SetTypeStateHeader(ByteArrayReader reader)
        {
            if (reader.ReadBool() && PrimaryWeapon != null)
                PrimaryWeapon.SetFullState(reader);
            if (reader.ReadBool() && SecondaryWeapon != null)
                SecondaryWeapon.SetFullState(reader);
            if (reader.ReadBool() && TertiaryWeapon != null)
                TertiaryWeapon.SetFullState(reader);
        }

        public override string ToString()
        {
            return base.ToString() + ", Player ID: " + Player.Id.ToString();
        }

        #region Static initialization
        private static Dictionary<string, Type> _tankTypes =
            new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);

        public static new IReadOnlyDictionary<string, Type> AvailableTypes => _tankTypes;

        public static Tank ReflectiveInitialize(string tankName, GamePlayer player, GameCore game, bool authorized, byte[] state = null)
        {
            long totalMem = 0;
            if (GlobalSettings.Debug)
                totalMem = GC.GetTotalMemory(true);

            if (!_tankTypes.ContainsKey(tankName.ToLower())) throw new Exception("Tank type does not exist.");

            var inst = (Tank)Activator.CreateInstance(_tankTypes[tankName.ToLower()], player, game, authorized);
            if (state != null) inst.ReceiveStateData(state);

            if (GlobalSettings.Debug)
            {
                var memUsageBytes = (GC.GetTotalMemory(true) - totalMem) / 1024f;
                game.Logger.Trace($"Allocating Generic (Game)Object {tankName}, size is: {memUsageBytes.ToString("N2")} KiB");
            }
            return inst;
        }

        public static T ReflectiveInitialize<T>(string tankName, GamePlayer player, GameCore game, bool authorized, byte[] state = null)
            where T : Tank
        {
            return (T)ReflectiveInitialize(tankName, player, game, authorized, state);
        }

        private static void RegisterType<T>(Module module) where T : Tank
        {
            //get the name
            var name = module.Name + "+" + typeof(T).Name;
            if (_tankTypes.ContainsKey(name)) throw new Exception("Already registered!");

            _tankTypes.Add(name.ToLower(), typeof(T));
        }

        public static ICollection<string> GetAllTankTypes()
        {
            return _tankTypes.Keys;
        }
        #endregion

        static Tank()
        {
        }
    }
}
