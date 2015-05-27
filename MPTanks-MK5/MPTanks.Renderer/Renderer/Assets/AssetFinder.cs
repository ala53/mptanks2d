using Microsoft.Xna.Framework;
using MPTanks.Rendering.Renderer.Assets.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Rendering.Renderer.Assets
{
    class AssetFinder
    {
        private GameWorldRenderer _renderer;
        private AssetCache _cache;
        public AssetFinder(GameWorldRenderer renderer, AssetCache cache)
        {
            _renderer = renderer;
            _cache = cache;
        }

        public bool IsAnimation(string assetName)
        {
            return assetName.StartsWith("[animation]");
        }

        public Sprite ParseAndRetrieveAsset(string asset, string sheet)
        {
            if (IsAnimation(asset))
            {
                var parsed = ParseAnimation(asset);
                var frameName = GetAnimationFrameName(parsed);
                var sheetName = GetAnimationSheetName(parsed);
                return _cache.GetSprite(frameName, sheetName);
            }

            //If not animated, just do the simple thing and...
            return _cache.GetSprite(asset, sheet);
        }

        /// <summary>
        /// Adds the specified amount of time to an animation, returning the new animation string.
        /// If it's at the end, it either loops or stops.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public string IncrementAnimation(string asset, GameTime gameTime)
        {
            return null;
        }

        private string[] ParseAnimation(string asset)
        {
            return null;
        }
        private string GetAnimationFrameName(string[] animation)
        {
            return null;
        }
        private string GetAnimationSheetName(string[] animation)
        {
            return null;
        }
    }
}
