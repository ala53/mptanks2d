using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MPTanks.Engine;
using MPTanks.Client.Backend.Renderer.Assets;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MPTanks.Client.Backend.Renderer.LayerRenderers
{
    class MapBackgroundRenderer : LayerRenderer
    {
        public MapBackgroundRenderer(GameCoreRenderer renderer, GraphicsDevice gd, 
            ContentManager content, AssetFinder finder)
            :base(renderer, gd, content, finder)
        {
        }
        public override void Draw(GameTime gameTime, RenderTarget2D target)
        {
        }
    }
}
