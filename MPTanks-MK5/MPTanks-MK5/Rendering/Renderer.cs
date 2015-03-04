using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPTanks_MK5.Rendering
{
    abstract class Renderer
    {
        public GameClient Game { get; private set; }
        public Renderer(GameClient game)
        {
            Game = game;
        }

        abstract public void Render(SpriteBatch sb);

        abstract public void Destroy();
    }
}
