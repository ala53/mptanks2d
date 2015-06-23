using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Rendering.Animations
{
    public class AnimationEngine
    {
        private HashSet<Animation> animations = new HashSet<Animation>();
        public ISet<Animation> Animations { get { return animations; } }

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

        public void AddAnimation(Animation anim)
        {
            _dirty = true;
            if (!animations.Contains(anim))
                animations.Add(anim);
        }
        public void RemoveAnimation(Animation anim)
        {
            _dirty = true;
            if (animations.Contains(anim))
                animations.Remove(anim);
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
            foreach (var animation in animations)
            {
                animation.PositionInAnimationMs += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }
    }
}
