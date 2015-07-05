using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Rendering.Renderer.Assets.Sprites
{
    class Sprite
    {
        private SpriteSheet _sheet;
        public SpriteSheet SpriteSheet
        {
            get { return _sheet; }
            set
            {
                if (_sheet == null) _sheet = value;
                else throw new MemberAccessException("Cannot change attached sprite sheet");
            }
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public string Name { get; private set; }

        public Sprite(int x, int y, int w, int h, string name)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;

            Name = name;
        }
    }
}
