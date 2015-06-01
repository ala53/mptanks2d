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

        public static IEnumerable<ModDatabaseItem> Mods { get { return _items; } }

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
