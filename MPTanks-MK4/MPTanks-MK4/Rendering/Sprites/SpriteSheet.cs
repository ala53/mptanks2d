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
        public string Name { get; private set; }
        public Sprite DefaultSprite { get; private set; }
        public Sprite[] Sprites { get; private set; }
        public Animation[] Animations { get; private set; }
        public Texture Texture { get; set; }
        public int Width { get { return Texture.Width; } }
        public int Height { get { return Texture.Height; } }

        public Animation GetAnimationByName(string name)
        {
            foreach (var animation in Animations)
                if (animation.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return animation;

            return null;
        }

        public Sprite GetSpriteByName(string name)
        {
            foreach (var sprite in Sprites)
                if (sprite.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return sprite;

            //Return the default sprite if we cannot find it
            return DefaultSprite;
        }
    }
}
