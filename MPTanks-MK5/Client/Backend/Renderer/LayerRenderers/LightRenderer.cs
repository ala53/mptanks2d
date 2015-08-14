using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MPTanks.Client.Backend.Renderer.Assets;

namespace MPTanks.Client.Backend.Renderer.LayerRenderers
{
    class LightRenderer : LayerRenderer
    {
        private Effect _lightRenderer;
        private Effect _lightMaskPreCompositor;
        public LightRenderer(GameCoreRenderer renderer, GraphicsDevice gd,
            ContentManager content, AssetFinder finder)
            : base(renderer, gd, content, finder)
        { }
        public override void Draw(GameTime gameTime, RenderTarget2D target)
        {
        }
    }
}
