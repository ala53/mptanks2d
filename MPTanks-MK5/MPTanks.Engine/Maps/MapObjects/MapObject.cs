﻿using Microsoft.Xna.Framework;
using MPTanks.Engine.Mods;
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
            if (!_objTypes.ContainsKey(objName.ToLower())) throw new Exception("Map object type does not exist.");

            var inst = (MapObject)Activator.CreateInstance(_objTypes[objName.ToLower()], game, authorized, position, rotation);
            if (state != null) inst.ReceiveStateData(state);

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
            Mods.CoreModLoader.LoadTrustedMods();
        }
        #endregion
    }
}