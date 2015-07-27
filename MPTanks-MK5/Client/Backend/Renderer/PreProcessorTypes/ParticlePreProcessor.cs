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
        }
    }
}
