using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Modding
{
    public class Module
    {
        public bool Activated { get; private set; }
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public string Author { get; internal set; }
        public string Version { get; internal set; }
        public Assembly[] Assemblies { get; internal set; }
        /// <summary>
        /// A list of the types of tanks
        /// </summary>
        public TankType[] Tanks { get; internal set; }
        /// <summary>
        /// A list of the types of projectiles
        /// </summary>
        public ProjectileType[] Projectiles { get; internal set; }
        /// <summary>
        /// A list of the types of gamemodes
        /// </summary>
        public GamemodeType[] Gamemodes { get; internal set; }
        /// <summary>
        /// A list of the types of mapobjects
        /// </summary>
        public MapObjectType[] MapObjects { get; internal set; }


        /// <summary>
        /// Injects the mod into the engine by calling the appropriate RegisterType()'s.
        /// </summary>
        public void Inject()
        {
            if (Activated) return; //No multiple initialization
            Activated = true;
            foreach (var tank in Tanks)
            {
                var type = GetTypeHelper.GetType(Settings.TankTypeName);
                var method = type.GetMethod("RegisterType", BindingFlags.NonPublic | BindingFlags.Static);
                var generic = method.MakeGenericMethod(tank.Type);
                generic.Invoke(null, null);
            }

            foreach (var prj in Projectiles)
            {
                var type = GetTypeHelper.GetType(Settings.ProjectileTypeName);
                var method = type.GetMethod("RegisterType", BindingFlags.NonPublic | BindingFlags.Static);
                var generic = method.MakeGenericMethod(prj.Type);
                generic.Invoke(null, null);
            }

            foreach (var gamemode in Gamemodes)
            {
                var type = GetTypeHelper.GetType(Settings.GamemodeTypeName);
                var method = type.GetMethod("RegisterType", BindingFlags.NonPublic | BindingFlags.Static);
                var generic = method.MakeGenericMethod(gamemode.Type);
                generic.Invoke(null, null);
            }

            foreach (var mapObj in MapObjects)
            {
                var type = GetTypeHelper.GetType(Settings.MapObjectTypeName);
                var method = type.GetMethod("RegisterType", BindingFlags.NonPublic | BindingFlags.Static);
                var generic = method.MakeGenericMethod(mapObj.Type);
                generic.Invoke(null, null);
            }
        }
    }

    public class GamemodeType
    {
        public Type Type { get; internal set; }
        public string ReflectionTypeName { get; internal set; }
        public string DisplayName { get; internal set; }
        public string DisplayDescription { get; internal set; }
        /// <summary>
        /// The minimum number of players required to start a game.
        /// </summary>
        public int MinPlayersCount { get; set; }
        /// <summary>
        /// Whether to whitelist or blacklist allowed tank types.
        /// </summary>
        public bool WhitelistPlayerTankTypes { get; set; }
        /// <summary>
        /// Whether to allow supertanks
        /// </summary>
        public bool AllowSuperTanks { get; set; }
        /// <summary>
        /// If whitelisted: the allowed player tank types (reflection names)
        /// </summary>
        public IEnumerable<string> AllowedPlayerTankTypes { get; set; }
        /// <summary>
        /// If blacklisted: the disallowed player tank types (reflection names)
        /// </summary>
        public IEnumerable<string> DisallowedPlayerTankTypes { get; set; }

        public GamemodeType(Type t)
        {
            var attrib = t.GetCustomAttribute<GamemodeAttribute>(true);
            if (attrib == null)
                throw new Exception(t.FullName + " is missing [GamemodeAttribute]");

            Type = t;
            ReflectionTypeName = (string)t.GetProperty("ReflectionTypeName", BindingFlags.Public
                | BindingFlags.GetProperty | BindingFlags.Static).GetMethod.Invoke(null, null);
            DisplayName = attrib.Name;
            DisplayDescription = attrib.Description;
            MinPlayersCount = attrib.MinPlayersCount;
            WhitelistPlayerTankTypes = attrib.WhitelistPlayerTankTypes;
            AllowSuperTanks = attrib.AllowSuperTanks;
            AllowedPlayerTankTypes = attrib.AllowedPlayerTankTypes;
            DisallowedPlayerTankTypes = attrib.DisallowedPlayerTankTypes;
        }
        internal static bool IsGamemodeType(Type t)
        {
            var gamemode = GetTypeHelper.GetType(Settings.GamemodeTypeName);

            return t.IsSubclassOf(gamemode);
        }
    }
    public class TankType
    {
        public TankAttribute.TankCategory Category { get; internal set; }
        public Type Type { get; internal set; }
        public string ReflectionTypeName { get; internal set; }
        public string DisplayName { get; internal set; }
        public string DisplayDescription { get; internal set; }

        public TankType(Type t)
        {
            var attrib = t.GetCustomAttribute<TankAttribute>(true);
            if (attrib == null)
                throw new Exception(t.FullName + " is missing [TankAttribute]");

            Category = attrib.Category;
            Type = t;
            ReflectionTypeName = (string)t.GetProperty("ReflectionTypeName", BindingFlags.Public
                | BindingFlags.GetProperty | BindingFlags.Static).GetMethod.Invoke(null, null);
            DisplayName = attrib.DisplayName;
            DisplayDescription = attrib.Description;
        }
        internal static bool IsTankType(Type t)
        {
            var tank = GetTypeHelper.GetType(Settings.TankTypeName);

            return t.IsSubclassOf(tank);
        }
    }

    public class ProjectileType
    {
        public Type Type { get; internal set; }
        public TankType OwnerType { get; internal set; }
        public string DisplayName { get; internal set; }
        public string ReflectionTypeName { get; internal set; }

        public ProjectileType(Type t, TankType[] tankTypes)
        {
            var attrib = t.GetCustomAttribute<ProjectileAttribute>(true);
            if (attrib == null)
                throw new Exception(t.FullName + " is missing [ProjectileAttribute]");

            Type = t;
            ReflectionTypeName = (string)t.GetProperty("ReflectionTypeName", BindingFlags.Public
                | BindingFlags.GetProperty | BindingFlags.Static).GetMethod.Invoke(null, null);
            DisplayName = attrib.Name;

            foreach (var tk in tankTypes)
                if (tk.ReflectionTypeName == attrib.OwnerReflectionName)
                    OwnerType = tk;

            if (OwnerType == null)
                throw new Exception(attrib.Name + "'s owner \"" + attrib.OwnerReflectionName
                    + "\" does not exist in this module");
        }

        internal static bool IsProjectileType(Type t)
        {
            var prj = GetTypeHelper.GetType(Settings.ProjectileTypeName);

            return t.IsSubclassOf(prj);
        }
    }
    public class MapObjectType
    {
        public Type Type { get; internal set; }
        public string DisplayName { get; internal set; }
        public string ReflectionTypeName { get; internal set; }
        public bool StaticObject { get; internal set; }
        public float MinWidth { get; internal set; }
        public float MinHeight { get; internal set; }

        public MapObjectType(Type t)
        {
            var attrib = t.GetCustomAttribute<MapObjectAttribute>(true);
            if (attrib == null)
                throw new Exception(t.FullName + " is missing [MapObjectAttribute]");

            Type = t;
            ReflectionTypeName = (string)t.GetProperty("ReflectionTypeName", BindingFlags.Public
                | BindingFlags.GetProperty | BindingFlags.Static).GetMethod.Invoke(null, null);
            DisplayName = attrib.Name;

        }

        internal static bool IsMapObjectType(Type t)
        {
            var prj = GetTypeHelper.GetType(Settings.MapObjectTypeName);

            return t.IsSubclassOf(prj);
        }
    }

    internal static class GetTypeHelper
    {
        private static Dictionary<string, Type> _types =
            new Dictionary<string, Type>();
        public static Type GetType(string name)
        {
            if (_types.ContainsKey(name))
                return _types[name];

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                foreach (var type in asm.GetTypes())
                    if (type.FullName == name)
                    {
                        _types.Add(name, type);
                        return type;
                    }

            return null;
        }
    }
}
