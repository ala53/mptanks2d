using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Mods
{
    public static class ModLoader
    {
        private static List<Modding.Module> _modules = new List<Modding.Module>();
        public static IReadOnlyList<Modding.Module> Modules { get { return _modules.ToArray(); } }

        public static bool LoadModFromSource(string sourceCode, bool verifySafety, out string errors, 
            out Modding.Module module, bool activate = false)
        {
            module = null;
            errors = "";
            try
            {
                var mod = MPTanks.Modding.ModLoader.Load(sourceCode, verifySafety, out errors);
                if (mod == null)
                {
                    errors = "Mod injection failed.\n=================================\n\n\n" + errors;
                    return false;
                }
                _modules.Add(mod);
                if (activate) ActivateMod(mod);

                module = mod;
            }
            catch (Exception e)
            {
                errors = "Something went really wrong!\n\n" + e.ToString();
                return false;
            }

            return true;
        }

        private static HashSet<string> _loadedModFiles = new HashSet<string>();
        public static bool LoadModFromFile(string file, bool verifySafety, out string errors, 
            out Modding.Module module, bool activate = false)
        {
            module = null;
            if (_loadedModFiles.Contains(file.ToLower()))
            {
                errors = "Mod already loaded.";
                return false;
            }
            errors = "";
            try
            {
                var mod = MPTanks.Modding.ModLoader.Load(Assembly.LoadFile(file), verifySafety, out errors);
                if (mod == null)
                {
                    errors = "Mod injection failed.\n=================================\n\n\n" + errors;
                    return false;
                }
                _modules.Add(mod);
                module = mod;
                if (activate) ActivateMod(mod);

                _loadedModFiles.Add(file.ToLower());
            }
            catch (Exception e)
            {
                errors = "Something went really wrong!\n\n" + e.ToString();
                return false;
            }

            return true;
        }

        public static void ActivateMod(Modding.Module mod)
        {
            if (!mod.Activated)
                mod.Inject();
        }
    }
}
