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
        public string Name;
        public Keyframe[] Frames;
        public int LoopCount;

        public double LengthMs
        {
            get
            {
                double ms = 0;
                foreach (var kf in Frames) ms += kf.LengthMs;
                return ms;
            }
        }

        public class Keyframe
        {
            public Sprite Sprite;
            public float LengthMs;
            public bool FadeIn;
            public float FadeInTimeMs;
        }

        public State Play()
        {
            return new State(this);
        }

        public class State
        {
            public State(Animation anim)
            {
                Animation = anim;
                IsPlaying = true;
            }
            public Animation Animation { get; private set; }
            public bool IsPlaying { get; private set; }
            public Image CurrentFrame { get; private set; }

            private double totalMillisecondsIntoAnimation;

            /// <summary>
            /// Gets the next frame for the current animation
            /// </summary>
            /// <param name="ms">The number of milliseconds either in total or since the last frame.</param>
            /// <param name="isDeltaTime">If set to true, the previous parameter is interpreted 
            /// as a delta between frames. If false, it is assumed to be the whole time since 
            /// application start.</param>
            /// <returns></returns>
            public Sprite GetCurrentFrame(double ms, bool isDeltaTime = true)
            {
                if (isDeltaTime)
                    totalMillisecondsIntoAnimation += ms;
                else
                    totalMillisecondsIntoAnimation = ms;

                double msPrevFrames = 0;

                foreach (var kf in Animation.Frames)
                {
                    //if (current time in animation) < (frame position in animation)
                    //then this is the right frame to display
                    if (totalMillisecondsIntoAnimation < msPrevFrames + kf.LengthMs)
                        return kf.Sprite;

                    msPrevFrames += kf.LengthMs;
                }

                //If we finished the animation
                IsPlaying = false;
                return Animation.Sheet.DefaultSprite;
            }

            public void Stop()
            {
                IsPlaying = false;
            }
        }
    }
}
