using MPTanks_MK4.Resources.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4.Rendering.Sprites
{
    class SpriteSheet
    {
        public Sprite DefaultSprite { get; private set; }
        public Sprite[] Sprites { get; private set; }
        public SpriteCollection[] Collections { get; private set; }
        public Animation Animations { get; private set; }
        public Texture Texture { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
    }
}
