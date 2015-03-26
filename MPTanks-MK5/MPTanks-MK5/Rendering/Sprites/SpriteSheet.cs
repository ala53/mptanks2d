using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.GameClient.Rendering.Sprites
{
    class SpriteSheet
    {
        public string Name { get; private set; }
        public IReadOnlyDictionary<string, Sprite> Sprites { get; private set; }
        public Texture2D Texture { get; private set; }
        public IReadOnlyDictionary<string, Animation.Animation> Animations { get; private set; }

        public SpriteSheet(string name, Texture2D texture, Dictionary<string, Rectangle> sprites,
            Dictionary<string, Animation.Animation> animations = null)
        {
            Name = name;
            Texture = texture;
            var _sprites = new Dictionary<string, Sprite>();

            foreach (var sprite in sprites)
                _sprites.Add(sprite.Key, new Sprite(this, sprite.Key, sprite.Value));

            if (animations != null)
            {
                Animations = animations;
                foreach (var anim in animations)
                    anim.Value.Sheet = this;
            }

            Sprites = _sprites;
        }
    }
}
