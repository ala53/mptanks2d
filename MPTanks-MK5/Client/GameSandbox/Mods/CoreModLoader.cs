using MPTanks.Engine.Settings;
using MPTanks.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.GameSandbox.Mods
{
    public static class CoreModLoader
    {
        private static bool _hasLoadedMods = false;
        public static void LoadTrustedMods(GameSettings settings)
        {
            string errors = "";
            if (_hasLoadedMods) return;
            _hasLoadedMods = true;

            foreach (var modFile in settings.CoreMods.Value)
            {
                if (GlobalSettings.Debug)
                    LoadModInternal(modFile, ref errors);
                else
                    try { LoadModInternal(modFile, ref errors); }
                    catch (Exception ex)
                    {
                        errors += ex.ToString();
                        Logger.Error(errors);
                    }
            }
        }

        private static void LoadModInternal(string modFile, ref string errors)
        {
            string err = "";
            var mod = Modding.ModLoader.Load(modFile, false, out err);

            errors += "\n\n\n" + err;
        }
    }
}
