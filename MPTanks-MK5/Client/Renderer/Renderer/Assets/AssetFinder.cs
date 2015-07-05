using Microsoft.Xna.Framework;
using MPTanks.Engine.Assets;
using MPTanks.Engine.Logging;
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

        /// <summary>
        /// Parses and retrieves a sprite sheet
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public Sprite ParseAndRetrieveAsset(string asset, string sheet)
        {
            if (IsAnimation(asset))
            {
                var frame = GetAnimationFrameInfo(asset);
                return _cache.GetSprite(frame);
            }

            //If not animated, just do the simple thing and...
            return _cache.GetSprite(asset, sheet);
        }

        /// <summary>
        /// Finds the asset and then increments the animation time (if it is an animation)
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public Sprite ParseAndRetrieveAsset(ref string asset, string sheet, GameTime gameTime)
        {
            if (IsAnimation(asset)) //Fast path
            {
                var parsed = new ParsedAnimation(asset, _renderer.Logger);
                var frameInfo = GetAnimationFrameInfo(parsed);
                _cache.GetSprite(frameInfo);
                asset = IncrementAnimation(parsed, gameTime);
            }
            //Normal path
            return ParseAndRetrieveAsset(asset, sheet);
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
            return IncrementAnimation(ParsedAnimation.ParseAnimation(asset), gameTime);
        }

        public string IncrementAnimation(string[] animation, GameTime gameTime)
        {
            return IncrementAnimation(new ParsedAnimation(animation, _renderer.Logger), gameTime);
        }

        private string IncrementAnimation(ParsedAnimation parsed, GameTime gameTime)
        {
            if (parsed.ErrorResult != null) return null;

            parsed.Time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            return parsed.ToString();
        }

        private SpriteInfo GetAnimationFrameInfo(string[] animation)
        {
            //"[animation]" + positionInAnimationMs + "," + spriteSheetName + "," + animationName + "," + shouldLoop;
            var parsed = new ParsedAnimation(animation, _renderer.Logger);
            return GetAnimationFrameInfo(parsed);
        }

        private SpriteInfo GetAnimationFrameInfo(ParsedAnimation parsed)
        {

            if (parsed.ErrorResult != null)
                return parsed.ErrorResult.Value;

            if (_cache.IsLoading(parsed.SheetName))
                return new SpriteInfo(AssetCache.LoadingTextureSpriteName, null);
            var anim = _cache.GetAnimation(parsed.FrameName, parsed.SheetName);
            if (anim == null) return new SpriteInfo(AssetCache.MissingTextureSpriteName, null);

            double loops = parsed.Time / anim.Length.TotalMilliseconds;
            if (loops > 1 && !parsed.ShouldLoop)
                return new SpriteInfo(anim.FrameNames.Last(), anim.SpriteSheet.Name);

            double position = parsed.Time % anim.Length.TotalMilliseconds;
            int frame = (int)(position / (1000 / anim.FramesPerSecond));

            if (frame >= anim.FrameNames.Count)
                return new SpriteInfo(anim.FrameNames[0], anim.SpriteSheet.Name);
            else
                return new SpriteInfo(anim.FrameNames[frame], anim.SpriteSheet.Name);
        }

        public SpriteInfo GetAnimationFrameInfo(string animation)
        {
            return GetAnimationFrameInfo(ParsedAnimation.ParseAnimation(animation));
        }

        private struct ParsedAnimation
        {
            public SpriteInfo? ErrorResult;
            public float Time;
            public string SheetName;
            public string FrameName;
            public bool ShouldLoop;

            public ParsedAnimation(string animation, ILogger logger)
                : this(ParseAnimation(animation), logger)
            { }
            public ParsedAnimation(string[] animation, ILogger logger)
            {
                ErrorResult = null;
                Time = 0;
                ShouldLoop = false;

                if (animation.Length < 1 || !float.TryParse(animation[0], out Time))
                {
                    logger.Error("Parsing animation failed.");
                    logger.Error("[animation]" + string.Join(",", animation));
                    ErrorResult = new SpriteInfo(AssetCache.MissingTextureSpriteName, null);
                }
                SheetName = animation.Length > 1 ? GetAnimationSheetName(animation) : null;
                FrameName = animation.Length > 2 ? animation[2] : null;

                if (SheetName == null)
                {
                    logger.Error("Sheet name for animation is null");
                    logger.Error("[animation]" + string.Join(",", animation));
                    ErrorResult = new SpriteInfo(AssetCache.MissingTextureSpriteName, null);
                }

                if (FrameName == null)
                {
                    logger.Error("Frame/Sprite name for animation is null");
                    logger.Error("[animation]" + string.Join(",", animation));
                    ErrorResult = new SpriteInfo(AssetCache.MissingTextureSpriteName, null);
                }

                if (animation.Length <= 3 || !bool.TryParse(animation[3], out ShouldLoop))
                {
                    logger.Error("Should loop for animation is not defined.");
                    logger.Error("[animation]" + string.Join(",", animation));
                    ErrorResult = new SpriteInfo(AssetCache.MissingTextureSpriteName, null);
                }
            }

            public override string ToString()
            {
                return new StringBuilder().Append("[animation]").Append(Time)
                    .Append(",").Append(SheetName).Append(",")
                    .Append(FrameName).Append(",").Append(ShouldLoop).ToString();
            }

            public static string[] ParseAnimation(string asset)
            {
                return asset.Substring("[animation]".Length).Split(',');
            }
            public static string GetAnimationSheetName(string[] animation)
            {
                return animation[1];
            }
            public static string GetAnimationSheetName(string animation)
            {
                return GetAnimationSheetName(ParseAnimation(animation));
            }
        }
    }
}
