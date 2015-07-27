using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Renderer.Assets.Sprites
{
    class Animation
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

        private string[] _frameNames;
        public IReadOnlyList<string> FrameNames { get { return _frameNames; } }
        private List<Sprite> _sprites;
        public IReadOnlyList<Sprite> Frames { get { return _sprites; } }
        public string Name { get; private set; }
        public float FramesPerSecond { get; private set; }
        public float FrameLengthMs => 1000 / FramesPerSecond;
        public TimeSpan Length
        {
            get { return TimeSpan.FromSeconds(Frames.Count / FramesPerSecond); }
        }

        public Animation(string name, string[] frameNames, float framerate)
        {
            _frameNames = frameNames;
            Name = name;
            FramesPerSecond = framerate;
        }

        public void FindFrames(IReadOnlyDictionary<string, Sprite> sprites, Sprite missingTextureSprite = null)
        {
            if (_sprites != null) throw new Exception("FindFrames() can only be called once");

            _sprites = new List<Sprite>();
            foreach (var spName in _frameNames)
            {
                if (sprites.ContainsKey(spName))
                    _sprites.Add(sprites[spName]);
                else if (missingTextureSprite != null)
                    _sprites.Add(missingTextureSprite);
            }
        }
    }
}
