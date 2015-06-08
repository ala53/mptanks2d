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
        static List<ModDatabaseItem> _items = new List<ModDatabaseItem>();

        public static IReadOnlyList<ModDatabaseItem> Mods { get { return _items; } }

        static List<Module> _loadedModules = new List<Module>();

        public static IReadOnlyList<Module> LoadedModules { get { return _loadedModules; } }

        public static Dictionary<Type, Module> _reverseLookupTable = new Dictionary<Type, Module>();

        public static Dictionary<Type, Module> ReverseTypeTable { get { return _reverseLookupTable; } }

        static ModDatabase()
        {
            var fName = Path.Combine(Settings.ConfigDir, "Mod Database.json");
            if (File.Exists(fName))
                _items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModDatabaseItem>>(
                    File.ReadAllText(fName));
        }

        public static bool Contains(string name)
        {
            return Get(name) != null;
        }

        public static ModDatabaseItem Get(string name)
        {
            ModDatabaseItem result = null;
            foreach (var mod in Mods)
            {
                if (mod.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    //We try to find the newest version
                    if (result == null)
                    {
                        result = mod;
                        continue;
                    }
                    else
                    {
                        if (mod.Major > result.Major)
                            result = mod;
                        else if (mod.Major == result.Major)
                            if (mod.Minor > result.Minor)
                                result = mod;
                    }
                }
            }

            return result;
        }

        public static void Add(string name, int major, int minor, string tag, string file)
        {
            if (Contains(name))
            {
                Get(name).File = file;
            }
            else
            {
                var itm = new ModDatabaseItem();
                itm.Name = name;
                itm.Major = major;
                itm.Minor = minor;
                itm.Tag = tag;
                itm.File = file;
            }
            Save();
        }

        public static void AddLoaded(Module module)
        {
            if (!_loadedModules.Contains(module))
                _loadedModules.Add(module);

            foreach (var tank in module.Tanks)
                _reverseLookupTable.Add(tank.Type, module);

            foreach (var prj in module.Projectiles)
                _reverseLookupTable.Add(prj.Type, module);

            foreach (var mapObj in module.MapObjects)
                _reverseLookupTable.Add(mapObj.Type, module);

            foreach (var mode in module.Gamemodes)
                _reverseLookupTable.Add(mode.Type, module);
        }

        public static void Remove(string name, int major, int minor)
        {
            if (Contains(name))
            {
                if (Get(name).Major == major && Get(name).Minor == minor)
                    _items.Remove(Get(name));
            }
            Save();
        }

        private static void Save()
        {
            File.WriteAllText(Path.Combine(Settings.ConfigDir, "Mod Database.json"),
                Newtonsoft.Json.JsonConvert.SerializeObject(_items, Newtonsoft.Json.Formatting.Indented));
        }
    }

    public class ModDatabaseItem
    {
        public string Name { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public string Tag { get; set; }
        public string File { get; set; }
    }
}
