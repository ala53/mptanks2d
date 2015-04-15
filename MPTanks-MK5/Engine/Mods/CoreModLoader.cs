using MPTanks.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Mods
{
    public static class CoreModLoader
    {
        private static bool _hasLoadedMods = false;
        public static void LoadTrustedMods()
        {
            if (_hasLoadedMods) return;
            _hasLoadedMods = true;

            foreach (var modFile in Settings.DefaultTrustedMods)
            {
                try
                {
                    var ext = System.IO.Path.GetExtension(modFile);

                    if (ext == ".cs")
                    {
                        string errors;
                        Module mod;
                        ModLoader.LoadModComplex(
                            new string[] { System.IO.File.ReadAllText(modFile) }, null,
                            false, out errors, out mod, true
                            );
                    }

                    if (ext == ".dll")
                    {
                        string errors;
                        Module mod;
                        var result = ModLoader.LoadModFromFile(
                            System.IO.Path.GetFullPath(modFile), false, out errors, out mod, true
                            );
                    }
                }
                catch
                {

                }
            }
        }
    }
}
