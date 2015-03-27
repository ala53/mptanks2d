using Microsoft.Xna.Framework;
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

        abstract public void CollidedWithTank(Tanks.Tank tank);

        #region ReflectionHelpers
        private static Dictionary<string, Type> _prjTypes =
            new Dictionary<string, Type>();

        public static Projectile ReflectiveInitialize(string prjName, Tanks.Tank owner, GameCore game, bool authorized,
            Vector2 position = default(Vector2), float rotation = 0, byte[] state = null)
        {
            if (!_prjTypes.ContainsKey(prjName.ToLower())) throw new Exception("Tank type does not exist.");

            var inst = (Projectile)Activator.CreateInstance(_prjTypes[prjName.ToLower()], owner, game, authorized,
                position);
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
            if (!_prjTypes.ContainsKey(prjName.ToLower())) throw new Exception("Tank type does not exist.");

            var inst = (Projectile)Activator.CreateInstance(_prjTypes[prjName.ToLower()], args);
            if (state != null) inst.ReceiveStateData(state);

            return inst;
        }
        public static T ReflectiveInitialize<T>(string prjName, byte[] state = null, params object[] args) where T : Projectile
        {
            return (T)ReflectiveInitialize(prjName, state, args);
        }

        protected static void RegisterType<T>() where T : Projectile
        {
            //get the ReflectionTypeName static property 
            var name = (string)typeof(T).GetProperty("ReflectionTypeName",
            System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.GetProperty |
            System.Reflection.BindingFlags.Public)
            .GetMethod.Invoke(null, null);
            if (_prjTypes.ContainsKey(name)) throw new Exception("Already registered!");

            _prjTypes.Add(name, typeof(T));
        }

        public static ICollection<string> GetAllProjectileTypes()
        {
            return _prjTypes.Keys;
        }
        #endregion
    }
}
