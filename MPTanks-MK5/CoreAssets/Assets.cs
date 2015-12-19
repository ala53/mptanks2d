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
        //DO NOT CHANGE...LIKE,EVER.
        //Directly referenced from GamemodeAttribute.DefaultTankReflectionName
        public const string ModuleName = "CoreAssets";
        public static string[] GetSongNames()
        {
            return new[] {
            AssetResolver.ResolveAsset(ModuleName, "alien_technology.mp3"),
            AssetResolver.ResolveAsset(ModuleName, "summer_shade.mp3")
            };
        }
    }
}
