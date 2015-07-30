using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MPTanks.Engine;

namespace MPTanks.Client.Backend.Renderer.PreProcessorTypes
{
    class ParticlePreProcessor : PreProcessor
    {
        public ParticlePreProcessor(GameCoreRenderer renderer, Assets.AssetFinder finder, RenderCompositor compositor)
            : base(renderer, finder, compositor)
        { }
        public override void Process(GameTime gameTime)
        {
            var belowLayer = Compositor.GetOrAddLayer(-100000000);
            var aboveLayer = Compositor.GetOrAddLayer(100000000);
            foreach (var particle in Game.ParticleEngine.Particles)
            {
                if (!particle.Alive) continue;
                var info = particle.SpriteInfo;
                var part = new DrawableObject
                {
                    Mask = particle.ColorMask,
                    ObjectRotation = particle.Rotation,
                    Position = particle.Position,
                    Scale = Vector2.One,
                    Rectangle = new Engine.Core.RectangleF(0, 0, particle.Size.X, particle.Size.Y),
                    Size = particle.Size,
                    Texture = Finder.RetrieveAsset(ref info)
                };
                if (particle.RenderBelowObjects)
                    belowLayer.AddObject(part);
                else aboveLayer.AddObject(part);
            }
        }
    }
}
