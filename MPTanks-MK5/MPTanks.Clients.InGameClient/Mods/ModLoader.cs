using MPTanks.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.GameClient.Mods
{
    public static class ModLoader
    {
        private static Dictionary<string, Module> _loaded = new Dictionary<string, Module>(StringComparer.InvariantCultureIgnoreCase);
        public static Module LoadMod(string modFile, GameSettings settings)
        {
            Module mod;

            if (LoadMod(modFile, settings, out mod))
                return mod;

            return null;
        }
        public static bool LoadMod(string modFile, GameSettings settings, out Module loaded)
        {
            if (_loaded.ContainsKey(modFile))
            {
                loaded = _loaded[modFile];
                return true;
            }
            string errors;
            var mod = Modding.ModLoader.LoadMod(
                 modFile, settings.ModUnpackPath,
                 settings.ModAssetPath, out errors);

            loaded = mod;

            if (mod == null)
            {
                Logger.Error($"Loading mod failed: {modFile}");
                Logger.Error(errors);
                return false;
            }
            else
            {
                _loaded.Add(modFile, mod);
                return true;
            }
        }
    }
}
