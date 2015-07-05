using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.GameSandbox.Rendering.Sprites
{
    class Sprite
    {
        public virtual string Name { get; private set; }
        public virtual SpriteSheet SpriteSheet { get; private set; }
        public virtual Rectangle Bounds { get; private set; }

        public Sprite(SpriteSheet sheet, string name, Rectangle rectangle)
        {
            SpriteSheet = sheet;
            Name = name;
            Bounds = rectangle;
        }
    }
}
