using Microsoft.Xna.Framework;
using MPTanks.Engine.Gamemodes;
using MPTanks.Engine.Helpers;
using MPTanks.Engine.Settings;
using MPTanks.Modding;
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
            Vector2 position = default(Vector2), float rotation = 0, float density = 100, float restitution = 0f)
            : base(game, authorized, density, restitution, position, rotation)
        {
        }

        private static Dictionary<string, string> _staticSettingsDictionary = new Dictionary<string, string>();
        public IReadOnlyDictionary<string, string> InstanceSettings { get; private set; } = _staticSettingsDictionary;

        internal void ProcessInstanceSettings(IDictionary<string, string> settings)
        {
            SetInstanceSettings(settings);
        }

        /// <summary>
        /// Allows you to handle design time instance settings (configured via the [GameObjectAttribute])
        /// </summary>
        /// <param name="settings"></param>
        protected virtual void SetInstanceSettings(IDictionary<string, string> settings)
        {

        }

        protected override void CreateInternal()
        {
            if (((Modding.MapObjectAttribute)(GetType().GetCustomAttributes(
                typeof(Modding.MapObjectAttribute), true)[0])).IsStatic)
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

        protected override void GetTypeStateHeader(ByteArrayWriter writer)
        {
            //Fast case
            if (InstanceSettings.Count == 0)
            {
                writer.Write((ushort)0);
                return;
            }
            //Otherwise
            //Encode the instance settings dictionary
            writer.Write((ushort)InstanceSettings.Count);
            foreach (var set in InstanceSettings)
            {
                writer.Write(set.Key);
                writer.Write(set.Value);
            }
        }
        protected override void SetTypeStateHeader(ByteArrayReader reader)
        {
            var settings = new Dictionary<string, string>();

            var ct = reader.ReadUShort();
            if (ct == 0) return;
            for (var i = 0; i < ct; i++)
                settings.Add(reader.ReadString(), reader.ReadString());

            InstanceSettings = settings;
        }

        #region Static initialization
        private static Dictionary<string, Type> _objTypes =
            new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);
        public static new IReadOnlyDictionary<string, Type> AvailableTypes => _objTypes;

        public static MapObject ReflectiveInitialize(string objName, GameCore game, bool authorized, Vector2 position = default(Vector2), float rotation = 0, byte[] state = null)
        {
            long totalMem = 0;
            if (GlobalSettings.Debug)
                totalMem = GC.GetTotalMemory(true);

            if (!_objTypes.ContainsKey(objName.ToLower())) throw new Exception("Map object type does not exist.");

            var inst = (MapObject)Activator.CreateInstance(_objTypes[objName.ToLower()], game, authorized, position, rotation);
            if (state != null) inst.ReceiveStateData(state);

            if (GlobalSettings.Debug)
            {
                var memUsageBytes = (GC.GetTotalMemory(true) - totalMem) / 1024f;
                game.Logger.Trace($"Allocating (Map)Object {objName}, size is: {memUsageBytes.ToString("N2")} KiB");
            }
            return inst;
        }

        public static T ReflectiveInitialize<T>(string objName, GameCore game, bool authorized, Vector2 position = default(Vector2), float rotation = 0, byte[] state = null)
        where T : MapObject
        {
            return (T)ReflectiveInitialize(objName, game, authorized, position, rotation, state);
        }

        private static void RegisterType<T>(Module module) where T : MapObject
        {
            //get the name
            var name = module.Name + "+" + typeof(T).Name;
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
