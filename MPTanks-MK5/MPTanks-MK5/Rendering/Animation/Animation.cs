using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK5.Rendering.Animation
{
    class Animation
    {
        public string Name { get; private set; }
        private Sprites.SpriteSheet _sheet;
        public Sprites.SpriteSheet Sheet
        {
            get { return _sheet; }
            set
            {
                if (_sheet != null)
                    throw new Exception("Cannot change spritesheet after init");
                _sheet = value;
            }
        }
        public string[] FrameNames { get; private set; }
        public float FramesPerSecond { get; private set; }

        public float LengthMs { get { return (FrameNames.Length / FramesPerSecond) * 1000; } }

        public Animation(string name, string[] frames, float fps)
        {
            Name = name;
            FrameNames = frames;
            FramesPerSecond = fps;
        }

        public void Begin(Engine.Rendering.RenderableComponent component, int frameNumber = 0)
        {
            var msIntoAnim = frameNumber * (1000 / FramesPerSecond);
            component.AssetName = Engine.Rendering.Animation.AnimationAsString(Name, Sheet.Name, msIntoAnim);
        }

        public static Sprites.Sprite GetFrame(string animationDescription, Dictionary<string, Animation> animations)
        {
            var animInfo = animationDescription.Substring(11);
            var timeIntoAnimation = float.Parse(animInfo.Split(',')[0]);
            var sheetName = animInfo.Split(',')[1];
            var animName = animInfo.Split(',')[2];

            if (!animations.ContainsKey(animName))
            {
                Logger.Error("Animation not found by name (Name: " + animName +
                    ", full information: " + animationDescription + ")");
                return null;
            }
            var anim = animations[animName];

            var frameNumber = (int)(timeIntoAnimation / (1000 / anim.FramesPerSecond));

            return anim.GetFrame(frameNumber);
        }

        public static string AdvanceAnimation(string animation, float amountMs, Dictionary<string, Animation> animations)
        {
            var animInfo = animation.Substring(11);
            var animSplit = animInfo.Split(',');
            var timeIntoAnimation = float.Parse(animSplit[0]);
            var sheetName = animSplit[1];
            var animName = animSplit[2];
            var loop = bool.Parse(animSplit[3]);


            if (!animations.ContainsKey(animName))
            {
                Logger.Error("Animation not found by name (Name: " + animName +
                    ", full information: " + animInfo + ")");
                return "[animation]" + (timeIntoAnimation + amountMs) + "," + sheetName + "," + animName + "," + loop;
            }
            var anim = animations[animName];

            if (Animation.Ended(animName, animations, timeIntoAnimation + amountMs) && loop)
                return "[animation]" + 0 + "," + sheetName + "," + animName + "," + loop;
            else
                return "[animation]" + (timeIntoAnimation + amountMs) + "," + sheetName + "," + animName + "," + loop;
        }

        public static string GetSheetName(string animation)
        {
            var animInfo = animation.Substring(11);
            return animInfo.Split(',')[1];
        }

        public static Sprites.Sprite GetFrame(string animName, Dictionary<string, Animation> animations, float positionMs)
        {
            if (!animations.ContainsKey(animName))
            {
                Logger.Error("Animation not found by name (Name: " + animName + ")");
                return null;
            }
            var anim = animations[animName];

            return anim.GetFrame((int)(positionMs / (1000 / anim.FramesPerSecond)));
        }

        public static bool Ended(string animName, Dictionary<string, Animation> animations, float positionMs, float loopCount = 1)
        {
            if (!animations.ContainsKey(animName))
            {
                Logger.Error("Animation not found by name (Name: " + animName + ")");
                return true;
            }
            var anim = animations[animName];
            return ((positionMs / loopCount) >= anim.LengthMs);
        }

        private Sprites.Sprite GetFrame(int frameNumber)
        {
            if (frameNumber < 0)
            {
                Logger.Error("Out of bounds frame requested for animation " + Name +
                    ". Requested " + frameNumber + ", have " + FrameNames.Length);
                return Sheet.Sprites[FrameNames[0]];
            }
            return Sheet.Sprites[FrameNames[frameNumber % FrameNames.Length]];
        }
    }
}
