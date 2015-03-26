using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.GameClient.Rendering
{
    abstract class Renderer
    {
        public Screens.Screen Screen { get; private set; }
        public Renderer(Screens.Screen _screen)
        {
            Screen = _screen;
        }
        public abstract void Render(SpriteBatch sb, GameTime gt);

        public abstract void Destroy();
    }
}
