using Microsoft.Xna.Framework;
using MPTanks.Engine.Assets;
using MPTanks.Engine.Logging;
using MPTanks.Client.Backend.Renderer.Assets.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace MPTanks.Client.Backend.Renderer.Assets
{
    class AssetFinder
    {
        private GameCoreRenderer _renderer;
        private AssetCache _cache;
        public AssetFinder(GameCoreRenderer renderer, AssetCache cache)
        {
            _renderer = renderer;
            _cache = cache;
        }

        /// <summary>
        /// Retrieves an asset by name
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public Sprite RetrieveAsset(SpriteInfo info)
        {
            if (info.IsAnimation)
            {
                var frame = GetAnimationFrameInfo(info);
                return _cache.GetSprite(frame);
            }

            //If not animated, just do the simple thing and...
            return _cache.GetSprite(info);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IncrementAnimation(ref SpriteInfo info, GameTime gameTime)
        {
            info.PositionInAnimationMs += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        private SpriteInfo GetAnimationFrameInfo(SpriteInfo info)
        {
            if (_cache.IsLoading(info.SheetName))
                return new SpriteInfo(AssetCache.LoadingTextureSpriteName, null);

            var anim = _cache.GetAnimation(info.FrameName, info.SheetName);
            if (anim == null) return new SpriteInfo(AssetCache.MissingTextureSpriteName, null);

            int frame = (int)((info.PositionInAnimationMs % anim.Length.TotalMilliseconds) / anim.FrameLengthMs);

            if (frame >= anim.FrameNames.Count || info.PositionInAnimationMs > anim.Length.TotalMilliseconds)
                return new SpriteInfo(anim.FrameNames[0], anim.SpriteSheet.Name);
            else
                return new SpriteInfo(anim.FrameNames[frame], anim.SpriteSheet.Name);
        }
    }
}
