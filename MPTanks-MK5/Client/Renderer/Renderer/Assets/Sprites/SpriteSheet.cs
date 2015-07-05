using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Renderer.Renderer.Assets.Sprites
{
    class SpriteSheet : IReadOnlyDictionary<string, Sprite>, IReadOnlyDictionary<string, Animation>, IReadOnlyList<Sprite>, IReadOnlyList<Animation>
    {
        public string Name { get; private set; }

        private List<Animation> _animations = new List<Animation>();
        private List<Sprite> _sprites = new List<Sprite>();
        private Dictionary<string, Animation> _animsByName = new Dictionary<string, Animation>(
            StringComparer.InvariantCultureIgnoreCase);
        private Dictionary<string, Sprite> _spritesByName = new Dictionary<string, Sprite>(
            StringComparer.InvariantCultureIgnoreCase);

        public IReadOnlyList<Animation> Animations { get { return _animations; } }
        public IReadOnlyList<Sprite> Sprites { get { return _sprites; } }
        public IReadOnlyDictionary<string, Animation> AnimationsByName { get { return _animsByName; } }
        public IReadOnlyDictionary<string, Sprite> SpritesByName { get { return _spritesByName; } }

        public Texture2D Texture { get; private set; }

        #region List of Animations
        Animation IReadOnlyList<Animation>.this[int index]
        {
            get { return _animations[index]; }
        }

        int IReadOnlyCollection<Animation>.Count
        {
            get { return _animations.Count; }
        }

        IEnumerator<Animation> IEnumerable<Animation>.GetEnumerator()
        {
            return _animations.GetEnumerator();
        }
        #endregion

        #region List of Sprites
        Sprite IReadOnlyList<Sprite>.this[int index]
        {
            get { return _sprites[index]; }
        }

        int IReadOnlyCollection<Sprite>.Count
        {
            get { return _sprites.Count; }
        }

        IEnumerator<Sprite> IEnumerable<Sprite>.GetEnumerator()
        {
            return _sprites.GetEnumerator();
        }
        #endregion

        #region Dictionary of Animations
        bool IReadOnlyDictionary<string, Animation>.ContainsKey(string key)
        {
            return _animsByName.ContainsKey(key);
        }

        IEnumerable<string> IReadOnlyDictionary<string, Animation>.Keys
        {
            get { return _animsByName.Keys; }
        }

        bool IReadOnlyDictionary<string, Animation>.TryGetValue(string key, out Animation value)
        {
            return _animsByName.TryGetValue(key, out value);
        }

        IEnumerable<Animation> IReadOnlyDictionary<string, Animation>.Values
        {
            get { return _animsByName.Values; }
        }

        Animation IReadOnlyDictionary<string, Animation>.this[string key]
        {
            get { return _animsByName[key]; }
        }

        int IReadOnlyCollection<KeyValuePair<string, Animation>>.Count
        {
            get { return _animsByName.Count; }
        }

        IEnumerator<KeyValuePair<string, Animation>> IEnumerable<KeyValuePair<string, Animation>>.GetEnumerator()
        {
            return _animsByName.GetEnumerator();
        }
        #endregion

        #region Dictionary of sprites implementation

        public bool ContainsKey(string key)
        {
            return _spritesByName.ContainsKey(key);
        }

        public IEnumerable<string> Keys
        {
            get { return _spritesByName.Keys; }
        }

        public bool TryGetValue(string key, out Sprite value)
        {
            return _spritesByName.TryGetValue(key, out value);
        }

        public IEnumerable<Sprite> Values
        {
            get { return _spritesByName.Values; }
        }

        public Sprite this[string key]
        {
            get { return _spritesByName[key]; }
        }

        int IReadOnlyCollection<KeyValuePair<string, Sprite>>.Count
        {
            get { return _spritesByName.Count; }
        }

        IEnumerator<KeyValuePair<string, Sprite>> IEnumerable<KeyValuePair<string, Sprite>>.GetEnumerator()
        {
            return _spritesByName.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _spritesByName.GetEnumerator();
        }
        #endregion

        public SpriteSheet(Dictionary<string, Animation> animations, Dictionary<string, Sprite> sprites, Texture2D texture,
            string name, Sprite missingTextureSprite = null)
        {
            _animsByName = new Dictionary<string, Animation>(animations);
            _spritesByName = new Dictionary<string, Sprite>(sprites);

            _animations = animations.Values.ToList();
            _sprites = sprites.Values.ToList();

            foreach (var sprite in _sprites) sprite.SpriteSheet = this;
            foreach (var anim in _animations)
            {
                anim.SpriteSheet = this;
                anim.FindFrames(sprites, missingTextureSprite);
            }

            Texture = texture;
            Name = name;
        }
    }
}
