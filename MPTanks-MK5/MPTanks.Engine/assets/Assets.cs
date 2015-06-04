using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Assets
{
    public struct SpriteAnimationInfo
    {
        private readonly string _animationName;
        private readonly string _sheetName;
        public SpriteAnimationInfo(string animName, string sheetName)
        {
            _animationName = animName;
            _sheetName = sheetName;
        }

        public string AnimationName
        {
            get
            {
                return _animationName;
            }
        }

        public string SheetName
        {
            get
            {
                return _sheetName;
            }
        }
    }

    public struct SpriteInfo
    {
        private readonly string _spriteName;
        private readonly string _sheetName;

        public string SpriteName
        {
            get
            {
                return _spriteName;
            }
        }

        public string SheetName
        {
            get
            {
                return _sheetName;
            }
        }

        public SpriteInfo(string spriteName, string sheetName)
        {
            this._spriteName = spriteName;
            _sheetName = sheetName;
        }

        public static implicit operator SpriteInfo(SpriteAnimationInfo anim)
        {
            return new SpriteInfo(
                Rendering.Animations.Animation.AnimationAsString(anim.AnimationName, anim.SheetName, 0, true), "");
        }
    }
}
