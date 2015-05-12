using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Assets
{
    public struct SpriteAnimationInfo
    {
        public readonly string AnimationName;
        public readonly string SheetName;
        public SpriteAnimationInfo(string animName, string sheetName)
        {
            AnimationName = animName;
            SheetName = sheetName;
        }
    }

    public struct SpriteInfo
    {
        public readonly string SpriteName;
        public readonly string SheetName;
        public SpriteInfo(string spriteName, string sheetName)
        {
            SpriteName = spriteName;
            SheetName = sheetName;
        }

        public static implicit operator SpriteInfo(SpriteAnimationInfo anim)
        {
            return new SpriteInfo(
                Rendering.Animations.Animation.AnimationAsString(anim.AnimationName, anim.SheetName, 0, true), "");
        }
    }
}
