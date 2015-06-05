using MPTanks.Modding;
using System;
using System.Collections.Generic;

namespace MPTanks.Engine.Rendering
{
    public static class SpriteSheetLookupHelper
    {
        private static Func<Module, GamePlayer, string, string> _tankResolver = (m, p, a) =>
        {
            //Simple passthrough search
            if (m.Assets.ContainsKey(a))
                return m.Assets[a];
            return null;
        };
        private static Func<Module, string, string> _assetResolver = (m, a) =>
        {
            if (m.Assets.ContainsKey(a))
                return m.Assets[a];
            return null;
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
