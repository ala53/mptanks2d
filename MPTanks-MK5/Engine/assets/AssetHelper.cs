using MPTanks.Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Assets
{
    static class AssetHelper
    {
        private static Random _rand = new Random();
        public static SpriteAnimationInfo GetRandomExplosionAnimation()
        {
            return ChooseRandom(Explosions.ExplosionAnimations);
        }

        public static SpriteInfo GetRandomSmokePuff()
        {
            return ChooseRandom(SmokePuffs.SmokePuffSprites);
        }

        public static string AnimationToString(SpriteAnimationInfo info, float positionMs = 0, bool loop = false)
        {
            return MPTanks.Engine.Rendering.Animations.Animation.
                AnimationAsString(info.AnimationName, info.SheetName, positionMs, loop);
        }

        private static SpriteAnimationInfo ChooseRandom(SpriteAnimationInfo[] options)
        {
            return options[_rand.Next(0, options.Length)];
        }
        private static SpriteInfo ChooseRandom(SpriteInfo[] options)
        {
            var index = _rand.Next(0, options.Length - 1);
            return options[index];
        }
    }
}
