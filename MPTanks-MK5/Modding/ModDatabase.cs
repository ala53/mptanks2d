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

        public static IReadOnlyList<ModDatabaseItem> Mods { get { return _items; } }

        static List<Module> _loadedModules = new List<Module>();

        public static IReadOnlyList<Module> LoadedModules { get { return _loadedModules; } }

        public static Dictionary<Type, Module> _reverseLookupTable = new Dictionary<Type, Module>();

        public static Dictionary<Type, Module> ReverseTypeTable { get { return _reverseLookupTable; } }

        static ModDatabase()
        {
            var fName = Path.Combine(ModSettings.ConfigDir, "moddatabase.json");
            if (File.Exists(fName))
                _items = JsonConvert.DeserializeObject<List<ModDatabaseItem>>(
                    File.ReadAllText(fName));
            else Save();
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


        public static void Add(string name, int major, int minor, string tag, string file, bool hasWhitelist)
        {
            if (Contains(name))
            {
                Get(name).File = file;
                Get(name).Major = major;
                Get(name).Minor = minor;
                Get(name).Tag = tag;
                Get(name).UsesWhitelist = hasWhitelist;
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
            if (!Contains(module.Name))
                Add(module.Name, module.Version.Major, module.Version.Minor, module.Version.Tag, module.PackedFile, module.UsesWhitelist);

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

        public static void Remove(string name)
        {
            if (Contains(name))
                _items.Remove(Get(name));
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
