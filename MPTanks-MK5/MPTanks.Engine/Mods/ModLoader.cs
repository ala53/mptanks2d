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
        private static Dictionary<string, Assembly> _loadedModAssemblies = new Dictionary<string, Assembly>();

        public static bool LoadModComplex(string[] sourceCode, string[] assemblyFiles, bool verifySafety, out string errors,
            out Modding.Module module, bool activate = false)
        {
            //Load the assemblies into memory
            var asms = new List<Assembly>();
            foreach (var asmFile in assemblyFiles)
            {
                if (_loadedModAssemblies.ContainsKey(asmFile.ToLower()))
                {
                    asms.Add(_loadedModAssemblies[asmFile.ToLower()]);
                    continue;
                }

                var asm = Assembly.LoadFile(asmFile);
                _loadedModAssemblies.Add(asmFile.ToLower(), asm);
                asms.Add(asm);
            }

            //Then load the actual mod
            module = null;
            errors = "";
            try
            {
                var mod = MPTanks.Modding.ModLoader.Load(sourceCode, verifySafety, out errors, asms.ToArray(), assemblyFiles);
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
                //Handle errors
                errors = "Something went really wrong!\n\n" + e.ToString();
                return false;
            }

            return true;
        }

        public static bool LoadModFromFile(string file, bool verifySafety, out string errors,
            out Modding.Module module, bool activate = false)
        {
            module = null;
            if (_loadedModAssemblies.ContainsKey(file.ToLower()))
            {
                errors = "Mod already loaded.";
                return false;
            }
            errors = "";
            try
            {
                var asm = Assembly.LoadFile(file);
                var mod = MPTanks.Modding.ModLoader.Load(asm, verifySafety, out errors);
                if (mod == null)
                {
                    errors = "Mod injection failed.\n=================================\n\n\n" + errors;
                    return false;
                }
                _modules.Add(mod);
                module = mod;
                if (activate) ActivateMod(mod);

                _loadedModAssemblies.Add(file.ToLower(), asm);
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
