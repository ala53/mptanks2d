﻿using Microsoft.Xna.Framework;
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
        public AssetCache Cache { get; private set; }
        public AssetFinder(GameCoreRenderer renderer, AssetCache cache)
        {
            _renderer = renderer;
            Cache = cache;
        }

        /// <summary>
        /// Retrieves an asset by name
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public Sprite RetrieveAsset(ref SpriteInfo info)
        {
            if (info.IsAnimation)
            {
                var frame = GetAnimationFrameInfo(info);
                return Cache.GetSprite(frame);
            }

            //If not animated, just do the simple thing and...
            return Cache.GetSprite(info);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IncrementAnimation(ref SpriteInfo info, GameTime gameTime)
        {
            info.PositionInAnimation += gameTime.ElapsedGameTime;
        }

        private SpriteInfo GetAnimationFrameInfo(SpriteInfo info)
        {
            if (Cache.IsLoading(info.SheetName))
                return new SpriteInfo(AssetCache.LoadingTextureSpriteName, null);

            var anim = Cache.GetAnimation(info.FrameName, info.SheetName);
            if (anim == null) return new SpriteInfo(AssetCache.MissingTextureSpriteName, null);

            int frame = (int)((info.PositionInAnimation.TotalMilliseconds % anim.Length.TotalMilliseconds) / anim.FrameLengthMs);

            if (frame >= anim.FrameNames.Count ||
                info.PositionInAnimation.TotalMilliseconds > (anim.Length.TotalMilliseconds * info.LoopCount))
                return new SpriteInfo(anim.FrameNames[0], anim.SpriteSheet.FileName);
            else
                return new SpriteInfo(anim.FrameNames[frame], anim.SpriteSheet.FileName);
        }
    }
}
