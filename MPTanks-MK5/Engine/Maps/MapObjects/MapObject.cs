using Microsoft.Xna.Framework;
using MPTanks.Engine.Gamemodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Maps.MapObjects
{
    public abstract class MapObject : GameObject
    {
        public MapObject(GameCore game, bool authorized,
            Vector2 position = default(Vector2), float rotation = 0)
            : base(game, authorized, 100, 0, position, rotation)
        {
        }

        protected override void CreateInternal()
        {
            Body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
            base.CreateInternal();
        }

        private bool _killed = false;
        protected override bool CollideInternal(GameObject other, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (other.GetType().IsSubclassOf(typeof(Projectiles.Projectile)) && other.Alive)
            {
                var o = (Projectiles.Projectile)other;

                if (!CanBeDamaged(o.Owner.Team))
                    return true;
                Health -= o.DamageAmount;

                o.CollidedWithMapObject(this);

                if (Health <= 0 && !_killed)
                {
                    Game.RemoveGameObject(this, o);
                    _killed = true;
                }
                return true;
            }

            return base.CollideInternal(other, contact);
        }

        protected virtual bool CanBeDamaged(Team team) => true;

        #region Static initialization
        private static Dictionary<string, Type> _objTypes =
            new Dictionary<string, Type>();

        public static MapObject ReflectiveInitialize(string objName, GameCore game, bool authorized, Vector2 position = default(Vector2), float rotation = 0, byte[] state = null)
        {
#if DBG_WATCH_GAMEOBJECT_SIZES //Wrapped because of significant performance overhead
            var totalMem = GC.GetTotalMemory(true);
#endif
            if (!_objTypes.ContainsKey(objName.ToLower())) throw new Exception("Map object type does not exist.");

            var inst = (MapObject)Activator.CreateInstance(_objTypes[objName.ToLower()], game, authorized, position, rotation);
            if (state != null) inst.ReceiveStateData(state);
#if DBG_WATCH_GAMEOBJECT_SIZES
            var memUsageBytes = (GC.GetTotalMemory(true) - totalMem) / 1024f;
            game.Logger.Trace($"Allocating (Map)Object {objName}, size is: {memUsageBytes.ToString("N2")} KiB");
#endif
            return inst;
        }

        public static T ReflectiveInitialize<T>(string objName, GameCore game, bool authorized, Vector2 position = default(Vector2), float rotation = 0, byte[] state = null)
        where T : MapObject
        {
            return (T)ReflectiveInitialize(objName, game, authorized, position, rotation, state);
        }

        private static void RegisterType<T>() where T : MapObject
        {
            //get the name
            var name = ((MPTanks.Modding.GameObjectAttribute)(typeof(T).
                GetCustomAttributes(typeof(MPTanks.Modding.GameObjectAttribute), true))[0]).ReflectionTypeName;
            if (_objTypes.ContainsKey(name)) throw new Exception("Already registered!");

            _objTypes.Add(name.ToLower(), typeof(T));
        }

        public static ICollection<string> GetAllMapObjectTypes()
        {
            return _objTypes.Keys;
        }
        #endregion

        #region
        static MapObject()
        {
        }
        #endregion
    }
}
