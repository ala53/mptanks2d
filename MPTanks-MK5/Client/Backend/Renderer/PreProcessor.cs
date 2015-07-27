using Microsoft.Xna.Framework;
using MPTanks.Client.Backend.Renderer.Assets;
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
        public RenderCompositor Compositor { get; private set; }
        public AssetFinder Finder { get; private set; }
        public GameCore Game { get; private set; }
        public PreProcessor(GameCoreRenderer renderer, AssetFinder finder, RenderCompositor compositor)
        {
            Renderer = renderer;
            Finder = finder;
            Compositor = compositor;
            Game = renderer.Game;
        }

        public abstract void Process(GameTime gameTime);
    }
}
