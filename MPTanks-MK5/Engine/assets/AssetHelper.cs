using Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Assets
{
    static class AssetHelper
    {
        //We have a constant seed here so the game is deterministic across computers
        //(not quite, but better than a randomly seeded one)
        private static Random _rand = new Random(733201);
        public static SpriteAnimationInfo GetRandomExplosionAnimation()
        {
            return ChooseRandom(Explosions.ExplosionAnimations);
        }

        public static string AnimationToString(SpriteAnimationInfo info, float positionMs = 0, bool loop = false)
        {
            return Animation.AnimationAsString(info.AnimationName, info.SheetName, positionMs, loop);
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
