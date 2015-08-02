using Microsoft.Xna.Framework;
using MPTanks.Engine.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Rendering.Animations
{
    public class AnimationEngine
    {
        private HashSet<Animation> _animations = new HashSet<Animation>();
        public ISet<Animation> Animations { get { return _animations; } }

        private bool _dirty = false;
        /// <summary>
        /// Gets whether the state has changed since the last time this flag was checked.
        /// </summary>
        public bool IsDirty
        {
            get
            {
                var dirty = _dirty;
                _dirty = false;
                return dirty;
            }
        }

        private List<Animation> _animAddCache = new List<Animation>();
        public void AddAnimation(Animation anim)
        {
            _dirty = true;
            if (!_animations.Contains(anim) && !_animAddCache.Contains(anim))
                _animAddCache.Add(anim);
        }
        private List<Animation> _animRemovalCache = new List<Animation>();
        public void RemoveAnimation(Animation anim)
        {
            _dirty = true;
            if (_animations.Contains(anim) || _animRemovalCache.Contains(anim))
                _animRemovalCache.Add(anim);
        }
        /// <summary>
        /// For the renderer to use: Marks an animation as completed/finished and removes it.
        /// </summary>
        /// <param name="anim"></param>
        public void MarkAnimationCompleted(Animation anim)
        {
            if (anim.CompletionCallback != null)
                anim.CompletionCallback(anim);

            RemoveAnimation(anim);
        }
        public void Update(GameTime gameTime)
        {
            foreach (var anim in _animAddCache)
                _animations.Add(anim);
            foreach (var anim in _animRemovalCache)
                _animations.Remove(anim);
            _animAddCache.Clear();
            _animRemovalCache.Clear();
        }
    }
}
