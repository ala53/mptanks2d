using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4.Rendering.Sprites
{
    /// <summary>
    /// A collection of sprites that can represent something
    /// </summary>
    class SpriteCollection
    {
        public Animation[] Animations { get; private set; }
        public Sprite[] Sprites { get; private set; }
    }
}
