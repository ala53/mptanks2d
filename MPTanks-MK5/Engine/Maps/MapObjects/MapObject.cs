using Microsoft.Xna.Framework;
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

        #region Static initialization
        private static Dictionary<string, Type> _objTypes =
            new Dictionary<string, Type>();

        public static MapObject ReflectiveInitialize(string objName, GameCore game, bool authorized, Vector2 position = default(Vector2), float rotation = 0, byte[] state = null)
        {
            if (!_objTypes.ContainsKey(objName.ToLower())) throw new Exception("Tank type does not exist.");

            var inst = (MapObject)Activator.CreateInstance(_objTypes[objName.ToLower()], game, authorized, position, rotation);
            if (state != null) inst.ReceiveStateData(state);

            return inst;
        }

        public static T ReflectiveInitialize<T>(string objName, GameCore game, bool authorized, Vector2 position = default(Vector2), float rotation = 0, byte[] state = null)
        where T : MapObject
        {
            return (T)ReflectiveInitialize(objName, game, authorized, position, rotation, state);
        }

        protected static void RegisterType<T>() where T : MapObject
        {
            //get the name
            var name = (string)typeof(T).GetProperty("ReflectionTypeName",
            System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.GetProperty |
            System.Reflection.BindingFlags.Public)
            .GetMethod.Invoke(null, null);
            if (_objTypes.ContainsKey(name)) throw new Exception("Already registered!");

            _objTypes.Add(name, typeof(T));
        }

        public static ICollection<string> GetAllMapObjectTypes()
        {
            return _objTypes.Keys;
        }
        #endregion
    }
}
