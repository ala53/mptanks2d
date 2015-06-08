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
        public ModuleDeclarationAttribute.ModuleVersion Version { get; internal set; }
        public Assembly[] Assemblies { get; internal set; }
        public Assembly[] Dependencies { get; internal set; }
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

        internal Dictionary<string, string> assets = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        public IReadOnlyDictionary<string,string> Assets { get { return assets; } }

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

        public GamemodeType(Type t)
        {
            var attrib = t.GetCustomAttribute<GamemodeAttribute>(true);
            if (attrib == null)
                throw new Exception(t.FullName + " is missing [GamemodeAttribute]");

            Type = t;
            ReflectionTypeName = attrib.ReflectionTypeName;
            DisplayName = attrib.DisplayName;
            DisplayDescription = attrib.Description;
            MinPlayersCount = attrib.MinPlayersCount;
        }
        internal static bool IsGamemodeType(Type t)
        {
            var gamemode = GetTypeHelper.GetType(Settings.GamemodeTypeName);

            return t.IsSubclassOf(gamemode);
        }
    }
    public class TankType
    {
        public Type Type { get; internal set; }
        public string ReflectionTypeName { get; internal set; }
        public string DisplayName { get; internal set; }
        public string DisplayDescription { get; internal set; }

        public TankType(Type t)
        {
            var attrib = t.GetCustomAttribute<TankAttribute>(true);
            if (attrib == null)
                throw new Exception(t.FullName + " is missing [TankAttribute]");

            Type = t;
            ReflectionTypeName = attrib.ReflectionTypeName;
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
            ReflectionTypeName = attrib.ReflectionTypeName;
            DisplayName = attrib.DisplayName;

            foreach (var tk in tankTypes)
                if (tk.ReflectionTypeName == attrib.OwnerReflectionName)
                    OwnerType = tk;

            if (OwnerType == null)
                throw new Exception(attrib.DisplayName + "'s owner \"" + attrib.OwnerReflectionName
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
            ReflectionTypeName = attrib.ReflectionTypeName;
            DisplayName = attrib.DisplayName;

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
