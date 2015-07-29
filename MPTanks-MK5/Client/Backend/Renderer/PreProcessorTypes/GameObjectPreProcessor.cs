using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MPTanks.Engine;
using MPTanks.Engine.Core;

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
                    var asset = Finder.RetrieveAsset(ref info);
                    if (info.IsAnimation)
                    {
                        Finder.IncrementAnimation(ref info, gameTime);
                        component.SpriteInfo = info;
                    }

                    var physCompensation = 
                        new Vector2(Renderer.PhysicsCompensation, Renderer.PhysicsCompensation) / obj.Scale;

                    Compositor.AddDrawable(new DrawableObject
                    {
                        Mask = new Color(component.Mask.ToVector4() * obj.ColorMask.ToVector4()),
                        Position = obj.Position,
                        Rotation = component.Rotation,
                        ObjectRotation = obj.Rotation,
                        RotationOrigin = component.RotationOrigin + component.Offset,
                        Scale = obj.Scale,
                        Rectangle = new RectangleF(component.Offset.X - physCompensation.X,
                        component.Offset.Y - physCompensation.Y,
                        component.Size.X + physCompensation.X * 2, component.Size.Y + physCompensation.Y * 2),
                        Size = obj.DefaultSize,
                        Texture = asset
                    }, component.DrawLayer);
                }
            }
        }
    }
}
