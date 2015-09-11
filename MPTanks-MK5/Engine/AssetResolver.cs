using MPTanks.Modding;
using System;
using System.Collections.Generic;

namespace MPTanks.Engine.Rendering
{
    public static class AssetResolver
    {
        private static Func<Module, GamePlayer, string, string> _tankResolver = (m, p, a) =>
        {
            if (m == null || a == null || p == null)
                return a;
            //Simple passthrough search (with extension prediction)
            if (m.AssetMappings.ContainsKey(a))
                return m.AssetMappings[a];
            if (m.AssetMappings.ContainsKey(a + ".json"))
                return m.AssetMappings[a + ".json"];
            if (m.AssetMappings.ContainsKey(a + ".mp3"))
                return m.AssetMappings[a + ".mp3"];
            if (m.AssetMappings.ContainsKey(a + ".wav"))
                return m.AssetMappings[a + ".wav"];
            if (m.AssetMappings.ContainsKey(a + ".ogg"))
                return m.AssetMappings[a + ".ogg"];
            if (m.AssetMappings.ContainsKey(a + ".wma"))
                return m.AssetMappings[a + ".wma"];
            if (m.AssetMappings.ContainsKey(a + ".png"))
                return m.AssetMappings[a + ".png"];
            if (m.AssetMappings.ContainsKey(a + ".jpg"))
                return m.AssetMappings[a + ".jpg"];
            if (m.AssetMappings.ContainsKey(a + ".jpeg"))
                return m.AssetMappings[a + ".jpeg"];
            if (m.AssetMappings.ContainsKey(a + ".gif"))
                return m.AssetMappings[a + ".gif"];
            if (m.AssetMappings.ContainsKey(a + ".dds"))
                return m.AssetMappings[a + ".dds"];
            return a;
        };
        private static Func<Module, string, string> _assetResolver = (m, a) =>
        {
            if (m == null)
                return a;
            //Simple passthrough
            if (m.AssetMappings.ContainsKey(a))
                return m.AssetMappings[a];
            return a;
        };
        /// <summary>
        /// Registers the resolver
        /// </summary>
        /// <param name="tankResolver">
        /// Takes the calling module, 
        /// the player that the tank belongs to, 
        /// and the requested asset name. 
        /// It then outputs the actual asset name, accounting for multiple mods.</param>
        /// <param name="assetResolver">
        /// Takes the calling module and the requested asset name. 
        /// It then outputs the actual asset name, accounting for multiple mods</param>
        public static void RegisterResolver(
            Func<Module, GamePlayer, string, string> tankResolver, Func<Module, string, string> assetResolver)
        {
            _tankResolver = tankResolver;
            _assetResolver = assetResolver;
        }
        public static string ResolveAsset(string moduleName, string asset, GamePlayer player = null)
        {
            if (_tankResolver == null && _assetResolver == null)
                return asset;

            if (player != null)
                return _tankResolver(FindModuleByName(moduleName), player, asset);
            else
                return _assetResolver(FindModuleByName(moduleName), asset);
        }

        private static Dictionary<string, Module> _cachedSearches =
            new Dictionary<string, Module>(StringComparer.InvariantCultureIgnoreCase);

        private static Module FindModuleByName(string name)
        {
            if (_cachedSearches.ContainsKey(name))
                return _cachedSearches[name];
            foreach (var mod in ModDatabase.LoadedModules)
            {
                if (mod.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    _cachedSearches.Add(name, mod);
                    return mod;
                }
            }

            return null;
        }
    }
}
