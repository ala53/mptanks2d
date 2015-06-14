using MPTanks.Modding;
using System;
using System.Collections.Generic;

namespace MPTanks.Engine
{
    public partial class GameObject
    {
        #region Reflection helper
        //We cache the info for performance. Multiple calls only create one instance
        private string _cachedReflectionName;
        public string ReflectionName
        {
            get
            {
                if (_cachedReflectionName == null)
                    _cachedReflectionName = ((GameObjectAttribute[])(GetType()
                          .GetCustomAttributes(typeof(GameObjectAttribute), true)))[0]
                          .ReflectionTypeName;

                return _cachedReflectionName;
            }
        }
        private string _cachedDisplayName;
        public string DisplayName
        {
            get
            {
                if (_cachedDisplayName == null)
                    _cachedDisplayName = ((GameObjectAttribute[])(GetType()
                          .GetCustomAttributes(typeof(GameObjectAttribute), true)))[0]
                          .DisplayName;

                return _cachedDisplayName;
            }
        }
        private string _cachedDescription;
        public string Description
        {
            get
            {
                if (_cachedDescription == null)
                    _cachedDescription = ((GameObjectAttribute[])(GetType()
                          .GetCustomAttributes(typeof(GameObjectAttribute), true)))[0]
                          .Description;

                return _cachedDescription;
            }
        }

        private Module _cachedModule;
        /// <summary>
        /// The module that contains this object
        /// </summary>
        public Module ContainingModule
        {
            get
            {
                return ModDatabase.ReverseTypeTable[GetType()];
            }
        }
        #endregion

        private static Dictionary<string, Type> _types = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);
        private static void RegisterType<T>() where T : GameObject
        {
            //get the name
            var name = ((GameObjectAttribute)(typeof(T).
                GetCustomAttributes(typeof(GameObjectAttribute), true))[0]).ReflectionTypeName;
            if (_types.ContainsKey(name)) throw new Exception("Already registered!");

            _types.Add(name.ToLower(), typeof(T));
        }

        public static T ReflectiveInitialize<T>
            (string reflectionName, GameCore game, bool authorized = false) where T : GameObject =>
            (T)ReflectiveInitialize(reflectionName, game, authorized);

        public static GameObject ReflectiveInitialize(string reflectionName, GameCore game, bool authorized = false)
        {
            return (GameObject)Activator.CreateInstance(_types[reflectionName], game, authorized);
        }
    }
}
