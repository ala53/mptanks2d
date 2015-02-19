using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4.Rendering.Sprites
{
    class Animation
    {
        public SpriteSheet Sheet;
        public int[] SpriteSequence;
        public int LoopCount;

        public AnimationState Play()
        {
            return new AnimationState(this);
        }

        public class AnimationState
        {
            public AnimationState(Animation anim)
            {
                Animation = anim;
                IsPlaying = true;
            }
            public Animation Animation { get; private set; }
            public int FrameNumber { get; private set; }
            public bool IsPlaying { get; private set; }
            public Sprite CurrentFrame { get; private set; }

            public Sprite GetNextFrame()
            {
                //If the animation is finished...
                if (((float)FrameNumber / Animation.SpriteSequence.Length) >= Animation.LoopCount)
                {
                    IsPlaying = false;
                    return Animation.Sheet.DefaultSprite;
                }

                //Calculate the frame number accounting for loops in the animation
                var frame = FrameNumber++ % Animation.SpriteSequence.Length;

                CurrentFrame = Animation.Sheet.Sprites[Animation.SpriteSequence[frame]];
                return CurrentFrame;
            }

            public void Stop()
            {
                IsPlaying = false;
            }
        }
    }
}
