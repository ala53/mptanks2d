using Microsoft.Xna.Framework;
using MPTanks.Engine.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Projectiles
{
    public abstract class Projectile : GameObject
    {
        public abstract int DamageAmount { get; }

        [JsonIgnore]
        public Tanks.Tank Owner { get; private set; }

        public virtual bool DamagesMapObjects => true;
        public Projectile(Tanks.Tank owner, GameCore game, bool authorized, float density = 1,
            float bounciness = 0.1f, Vector2 position = default(Vector2), float rotation = 0)
            : base(game, authorized, density, bounciness, position, rotation)
        {
            Owner = owner;
        }

        protected override void CreateInternal()
        {
            Body.IsBullet = true;
            base.CreateInternal();
        }

        public virtual void CollidedWithTank(Tanks.Tank tank)
        {
            Kill(tank);
        }

        public virtual bool CanDamage(Tanks.Tank tank, bool friendlyFireEnabled)
        {
            return true;
        }

        public virtual void CollidedWithMapObject(Maps.MapObjects.MapObject mapObject)
        {
            if (DamagesMapObjects)
                Kill(mapObject);
        }

        #region Static initialization
        private static Dictionary<string, Type> _prjTypes =
            new Dictionary<string, Type>();

        public static Projectile ReflectiveInitialize(string prjName, Tanks.Tank owner, GameCore game, bool authorized,
            Vector2 position = default(Vector2), float rotation = 0, byte[] state = null)
        {
            if (!_prjTypes.ContainsKey(prjName.ToLower())) throw new Exception("Projectile type does not exist.");

            var inst = (Projectile)Activator.CreateInstance(_prjTypes[prjName.ToLower()], owner, game, authorized,
                position, rotation);
            if (state != null) inst.ReceiveStateData(state);

            return inst;
        }

        public static T ReflectiveInitialize<T>(string prjName, Tanks.Tank owner, GameCore game, bool authorized,
            byte[] state = null) where T : Projectile
        {
            return (T)ReflectiveInitialize(prjName, game, owner, authorized, state);
        }
        public static Projectile ReflectiveInitialize(string prjName, GameCore game, Tanks.Tank tank, bool authorized, byte[] state = null)
        {
            long totalMem = 0;
            if (GlobalSettings.Debug)
                totalMem = GC.GetTotalMemory(true);

            if (!_prjTypes.ContainsKey(prjName.ToLower())) throw new Exception("Projectile type does not exist.");

            var inst = (Projectile)Activator.CreateInstance(_prjTypes[prjName.ToLower()], tank, game, authorized);
            if (state != null) inst.ReceiveStateData(state);

            if (GlobalSettings.Debug)
            {
                var memUsageBytes = (GC.GetTotalMemory(true) - totalMem) / 1024f;
                game.Logger.Trace($"Allocating (Projectile)Object {prjName}, size is: {memUsageBytes.ToString("N2")} KiB");
            }
            return inst;
        }

        private static void RegisterType<T>() where T : Projectile
        {
            //get the reflection name from the attribute 
            var name = ((MPTanks.Modding.GameObjectAttribute)(typeof(T).
                GetCustomAttributes(typeof(MPTanks.Modding.GameObjectAttribute), true))[0]).ReflectionTypeName;
            if (_prjTypes.ContainsKey(name)) throw new Exception("Already registered!");

            _prjTypes.Add(name.ToLower(), typeof(T));
        }

        public static ICollection<string> GetAllProjectileTypes()
        {
            return _prjTypes.Keys;
        }
        #endregion

        static Projectile()
        {
        }
    }
}
