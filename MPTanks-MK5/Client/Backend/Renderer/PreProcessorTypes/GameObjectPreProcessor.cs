using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MPTanks.Engine;

namespace MPTanks.Client.Backend.Renderer.PreProcessorTypes
{
    class GameObjectPreProcessor : PreProcessor
    {
        public GameObjectPreProcessor(GameCoreRenderer renderer, Assets.AssetFinder finder, RenderCompositor compositor)
            : base(renderer, finder, compositor)
        { }
        public override void Process(GameTime gameTime)
        {
            foreach (var obj in Game.GameObjects)
            {
                foreach (var component in obj.Components.Values)
                {
                    var info = component.SpriteInfo;
                    var asset = Finder.RetrieveAsset(info);
                    if (info.IsAnimation)
                    {
                        Finder.IncrementAnimation(ref info, gameTime);
                        component.SpriteInfo = info;
                    }

                    Compositor.AddDrawable(new DrawableObject
                    {
                        Mask = new Color(component.Mask.ToVector4() * obj.ColorMask.ToVector4()),
                        Position = obj.Position + component.Offset,
                        Rotation = obj.Rotation + component.Rotation,
                        RotationOrigin = component.RotationOrigin,
                        Scale = obj.Scale * component.Scale,
                        Size = obj.Size,
                        Texture = asset
                    }, component.DrawLayer);
                }
            }
        }
    }
}
