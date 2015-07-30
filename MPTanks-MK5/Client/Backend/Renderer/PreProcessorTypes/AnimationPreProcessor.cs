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
        }
    }
}
