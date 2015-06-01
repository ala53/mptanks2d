using MPTanks.Engine.Settings;
using MPTanks.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.GameClient
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
#if !DEBUG
                try
                {
#endif
                string err = "";
                var mod = ModLoader.Load(modFile, false, out err);

                errors += "\n\n\n" + err;
#if !DEBUG
            }
                catch (Exception ex)
                {
                    errors += ex.ToString();
                    Logger.Error(errors);
                }
#endif
            }
        }
    }
}
