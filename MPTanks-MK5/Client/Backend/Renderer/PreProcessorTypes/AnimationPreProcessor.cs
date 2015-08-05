using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MPTanks.Engine;
using MPTanks.Client.Backend.Renderer.LayerRenderers;

namespace MPTanks.Client.Backend.Renderer.PreProcessorTypes
{
    class AnimationPreProcessor : PreProcessor
    {
        public AnimationPreProcessor(GameCoreRenderer renderer, Assets.AssetFinder finder, GameWorldRenderer compositor)
            : base(renderer, finder, compositor)
        { }
        public override void Process(GameTime gameTime)
        {
            foreach (var animation in Renderer.Game.AnimationEngine.Animations)
            {
                var animInfo = Finder.Cache.GetAnimation(
                    animation.SpriteInfo.FrameName, animation.SpriteInfo.SheetName);
                //Check if the animation has ended (or rather, if it will end one frame from now
                var info = animation.SpriteInfo;
                Finder.IncrementAnimation(ref info, gameTime);
                if (animInfo != null && animInfo.Length.TotalMilliseconds * animation.SpriteInfo.LoopCount
                        < info.PositionInAnimation.TotalMilliseconds)
                    Renderer.Game.AnimationEngine.MarkAnimationCompleted(animation);

                info = animation.SpriteInfo;
                Compositor.AddDrawable(new DrawableObject
                {
                    Mask = animation.Mask,
                    Rotation = animation.Rotation,
                    Size = animation.Size,
                    RotationOrigin = animation.RotationOrigin,
                    Scale = new Vector2(1),
                    Position = animation.Position,
                    Rectangle = new Engine.Core.RectangleF(0, 0, animation.Size.X, animation.Size.Y),
                    Texture = Finder.RetrieveAsset(ref info)
                }, animation.DrawLayer);
                Finder.IncrementAnimation(ref info, gameTime);
                animation.SpriteInfo = info;

            }
        }
    }
}
