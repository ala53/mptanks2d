using Microsoft.Xna.Framework;
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
            Vector2 position = default(Vector2), float rotation = 0, byte[] state = null) where T : Projectile
        {
            return (T)ReflectiveInitialize(prjName, owner, game, authorized, position, rotation, state);
        }
        public static Projectile ReflectiveInitialize(string prjName, byte[] state = null, params object[] args)
        {
            if (!_prjTypes.ContainsKey(prjName.ToLower())) throw new Exception("Projectile type does not exist.");

            var inst = (Projectile)Activator.CreateInstance(_prjTypes[prjName.ToLower()], args);
            if (state != null) inst.ReceiveStateData(state);

            return inst;
        }
        public static T ReflectiveInitialize<T>(string prjName, byte[] state = null, params object[] args) where T : Projectile
        {
            return (T)ReflectiveInitialize(prjName, state, args);
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
