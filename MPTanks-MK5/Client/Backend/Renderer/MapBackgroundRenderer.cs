using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Engine.Maps;
using MPTanks.Client.Backend.Renderer.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Renderer
{
    /// <summary>
    /// A renderer for the map background tiles.
    /// </summary>
    class MapBackgroundRenderer
    {
        private SpriteBatch _spriteBatch;
        public MapBackgroundRenderer(GameWorldRenderer renderer, Map map, SpriteBatch sb, GraphicsDevice gd, BasicEffect effect, AssetResolver resolver)
        {

        }

        public void Draw(GameTime gameTime)
        {

        }
    }
}
