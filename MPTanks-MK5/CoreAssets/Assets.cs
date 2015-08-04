using MPTanks.Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.CoreAssets
{
    public static class Assets
    {
        public const string ModuleName = "MPTanks Core Assets";
        public static string[] GetSongNames()
        {
            return new[] {
            AssetResolver.ResolveAsset(ModuleName, "alien_technology.mp3"),
            AssetResolver.ResolveAsset(ModuleName, "summer_shade.mp3")
            };
        }
    }
}
