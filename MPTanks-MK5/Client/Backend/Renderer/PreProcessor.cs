using Microsoft.Xna.Framework;
using MPTanks.Client.Backend.Renderer.Assets;
using MPTanks.Client.Backend.Renderer.LayerRenderers;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Renderer
{
    abstract class PreProcessor
    {
        public GameCoreRenderer Renderer { get; private set; }
        public GameWorldRenderer Compositor { get; private set; }
        public AssetFinder Finder { get; private set; }
        public GameCore Game => Renderer.Game;
        public PreProcessor(GameCoreRenderer renderer, AssetFinder finder, GameWorldRenderer compositor)
        {
            Renderer = renderer;
            Finder = finder;
            Compositor = compositor;
        }

        public abstract void Process(GameTime gameTime);
    }
}
