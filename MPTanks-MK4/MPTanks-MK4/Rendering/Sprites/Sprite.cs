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
    class Sprite
    {
        public string Name { get; private set; }
        public int DamageLevels { get { return Sprites.Length; } }
        public Image[] Sprites { get; private set; }

        /// <summary>
        /// Gets a sprite based on the damage level (if it is over, we return the highest level).
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public Image GetTexture(int level)
        {
            if (level >= Sprites.Length)
                return Sprites[Sprites.Length - 1];
            return Sprites[level];
        }
    }
}
