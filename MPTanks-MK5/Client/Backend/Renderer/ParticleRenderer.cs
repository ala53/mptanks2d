using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Engine.Rendering.Particles;
using MPTanks.Client.Backend.Renderer.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Renderer
{
    class ParticleRenderer
    {
        public ParticleRenderer(GameWorldRenderer renderer, ParticleEngine game, SpriteBatch sb, GraphicsDevice gd, BasicEffect effect, AssetResolver resolver)
        {

        }

        public void DrawBelow(GameTime gameTime)
        {
            Sort();
        }

        public void DrawAbove(GameTime gameTime)
        {

        }

        private void Sort()
        {

        }
    }
}
