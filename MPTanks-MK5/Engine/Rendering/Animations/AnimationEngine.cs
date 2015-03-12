using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Rendering.Animations
{
    public class AnimationEngine
    {
        private List<Animation> animations = new List<Animation>();
        public IReadOnlyList<Animation> Animations { get { return animations; } }

        private bool _dirty = false;
        public bool IsDirty { get { var dirty = _dirty; _dirty = false; return dirty; } }

        public void AddAnimation(Animation anim)
        {
            _dirty = true;
            animations.Add(anim);
        }
        public void RemoveAnimation(Animation anim)
        {
            _dirty = true;
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
