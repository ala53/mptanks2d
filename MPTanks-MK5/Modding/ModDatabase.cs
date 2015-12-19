using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Modding
{
    public static class ModDatabase
    {
        static List<ModDatabaseItem> _items { get; set; } = new List<ModDatabaseItem>();

        public static IReadOnlyList<ModDatabaseItem> AllModsList { get { return _items; } }

        static List<Module> _loadedModules = new List<Module>();

        public static IReadOnlyList<Module> LoadedModulesList { get { return _loadedModules; } }

        static Dictionary<string, Module> _loadedModulesByName = new Dictionary<string, Module>();
        public static IReadOnlyDictionary<string, Module> ModuleNameToLoadedModuleTable { get { return _loadedModulesByName; } }

        public static Dictionary<Type, Module> _reverseLookupTable = new Dictionary<Type, Module>();

        public static IReadOnlyDictionary<Type, Module> TypeToModuleTable { get { return _reverseLookupTable; } }

        public static Dictionary<string, Type> _reflectionToTypeObjectTable = new Dictionary<string, Type>();

        public static IReadOnlyDictionary<string, Type> ReflectionNameToTypeTable { get { return _reflectionToTypeObjectTable; } }

        static ModDatabase()
        {
            var fName = Path.Combine(ModSettings.ConfigDir, "moddatabase.json");
            if (File.Exists(fName))
                _items = JsonConvert.DeserializeObject<List<ModDatabaseItem>>(
                    File.ReadAllText(fName));
            else Save();
        }

        public static bool Contains(string name, int major)
        {
            return Get(name, major) != null;
        }

        public static ModDatabaseItem Get(string name, int major)
        {
            ModDatabaseItem result = null;
            foreach (var mod in AllModsList)
            {
                if (mod.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && mod.Major == major)
                {
                    //We try to find the newest version under a specified major version
                    if (result == null)
                    {
                        result = mod;
                        continue;
                    }
                    else if (mod.Minor > result.Minor)
                        result = mod;
                }
            }

            return result;
        }

        public static Module GetLoaded(string name, int major)
        {
            if (!Contains(name, major)) return null;
            var mod = Get(name, major);
            foreach (var module in LoadedModulesList)
                if (mod.Name.Equals(
                    module.Name, StringComparison.InvariantCultureIgnoreCase) && module.Version.Major == major)
                    return module;

            return null;
        }

        public static bool IsLoaded(string name, int major) => GetLoaded(name, major) != null;

        public static void Add(string name, int major, int minor, string tag, string file, bool hasWhitelist)
        {
            if (Contains(name, major))
            {
                Get(name, major).File = file;
                Get(name, major).Major = major;
                Get(name, major).Minor = minor;
                Get(name, major).Tag = tag;
                Get(name, major).UsesWhitelist = hasWhitelist;
            }
            else
            {
                var itm = new ModDatabaseItem();
                itm.Name = name;
                itm.Major = major;
                itm.Minor = minor;
                itm.Tag = tag;
                itm.File = file;
                itm.UsesWhitelist = hasWhitelist;
                _items.Add(itm);
            }
            Save();
        }

        public static void AddLoaded(Module module)
        {
            if (!Contains(module.Name, module.Version.Major))
                Add(module.Name, module.Version.Major, module.Version.Minor, module.Version.Tag, module.PackedFile, module.UsesWhitelist);

            if (_loadedModules.Contains(module))
                return;

            _loadedModulesByName.Add(module.Name, module);
            _loadedModules.Add(module);

            foreach (var tank in module.Tanks)
            {
                _reflectionToTypeObjectTable.Add(tank.ReflectionTypeName, tank.Type);
                _reverseLookupTable.Add(tank.Type, module);
            }

            foreach (var prj in module.Projectiles)
            {
                _reflectionToTypeObjectTable.Add(prj.ReflectionTypeName, prj.Type);
                _reverseLookupTable.Add(prj.Type, module);
            }

            foreach (var mapObj in module.MapObjects)
            {
                _reflectionToTypeObjectTable.Add(mapObj.ReflectionTypeName, mapObj.Type);
                _reverseLookupTable.Add(mapObj.Type, module);
            }

            foreach (var mode in module.Gamemodes)
            {
                _reflectionToTypeObjectTable.Add(mode.ReflectionTypeName, mode.Type);
                _reverseLookupTable.Add(mode.Type, module);
            }
        }

        public static void Remove(string name, int major)
        {
            if (Contains(name, major))
                _items.Remove(Get(name, major));
            Save();
        }

        private static void Save()
        {
            File.WriteAllText(Path.Combine(ModSettings.ConfigDir, "Mod Database.json"),
                JsonConvert.SerializeObject(_items, Formatting.Indented));
        }
    }

    public class ModDatabaseItem
    {
        public string Name { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public string Tag { get; set; }
        public string File { get; set; }
        public bool UsesWhitelist { get; set; }
        private Lazy<ModMetadata> _metadata;
        [JsonIgnore]
        public ModMetadata Metadata { get { return _metadata.Value; } }

        public async Task LoadMetadata() =>
            await Task.Run(() => { var tmp = Metadata; });

        public ModDatabaseItem()
        {
            _metadata = new Lazy<ModMetadata>(() => ModMetadata.CreateMetadata(File, UsesWhitelist));
        }
    }
}
