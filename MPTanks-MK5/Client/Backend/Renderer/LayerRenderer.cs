using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Client.Backend.Renderer.Assets;
using MPTanks.Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Renderer
{
   abstract class LayerRenderer
    {
        public GameCoreRenderer Renderer { get; private set; }
        public AssetFinder Finder { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }
        public ContentManager Content { get; private set; }
        public RectangleF ViewRect { get; set; }
        public LayerRenderer(GameCoreRenderer renderer, GraphicsDevice gd, ContentManager content, AssetFinder finder)
        {
            Renderer = renderer;
            Finder = finder;
            GraphicsDevice = gd;
            Content = content;
        }
        public abstract void Draw(GameTime gameTime, RenderTarget2D target);
    }
}
