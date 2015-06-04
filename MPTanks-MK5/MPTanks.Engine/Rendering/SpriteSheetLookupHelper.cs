using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Rendering
{
    public static class SpriteSheetLookupHelper
    {
        private static Func<Assembly, GamePlayer, string, string> _tankResolver;
        private static Func<Assembly, string, string> _assetResolver;
        /// <summary>
        /// Registers the resolver
        /// </summary>
        /// <param name="tankResolver">
        /// Takes the calling assembly, 
        /// the player that the tank belongs to, 
        /// and the requested asset name. 
        /// It then outputs the actual asset name, accounting for multiple mods.</param>
        /// <param name="assetResolver">
        /// Takes the calling assembly and the requested asset name. 
        /// It then outputs the actual asset name, accounting for multiple mods</param>
        public static void RegisterResolver(
            Func<Assembly, GamePlayer, string, string> tankResolver, Func<Assembly, string, string> assetResolver)
        {
            if (_tankResolver == null)
                _tankResolver = tankResolver;
            else throw new Exception("Cannot change resolvers post-init.");
            if (_assetResolver == null)
                _assetResolver = assetResolver;
            else throw new Exception("Cannot change resolvers post-init.");
        }
        public static string ResolveAsset(object calling, string asset)
        {
            if (_tankResolver == null && _assetResolver == null)
                return asset;

            if (calling.GetType().IsSubclassOf(typeof(Tanks.Tank)))
                return _tankResolver(calling.GetType().Assembly, ((Tanks.Tank)calling).Player, asset);
            else
                return _assetResolver(calling.GetType().Assembly, asset);
        }
    }
}
