using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Engine.Rendering;
using MPTanks.Engine.Rendering.Lighting;
using MPTanks.Rendering.Renderer.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Rendering.Renderer
{
    class LightMaskRenderer 
    {
        private RenderTarget2D _lightMask;
        public LightMaskRenderer(int teamNumber, LightEngine engine,  SpriteBatch sb, GraphicsDevice gd, BasicEffect effect, AssetResolver resolver)
        {

        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
