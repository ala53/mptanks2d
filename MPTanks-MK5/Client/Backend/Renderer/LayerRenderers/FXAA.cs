using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Client.Backend.Renderer.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MPTanks.Client.Backend.Renderer.LayerRenderers
{
    class FXAA : LayerRenderer 
    {
        public FXAA(GameCoreRenderer renderer, GraphicsDevice gd,
            ContentManager content, AssetFinder finder)
            : base(renderer, gd, content, finder)
        {

        }

        public override void Draw(GameTime gameTime, RenderTarget2D target)
        {
        }
    }
}
