using MPTanks.Engine.Assets;
using MPTanks.Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine
{
    public static partial class Helpers
    {
        private static Random _rand = new Random();

        public static string AnimationToString(SpriteAnimationInfo info, float positionMs = 0, bool loop = false)
        {
            return Rendering.Animations.Animation.
                AnimationAsString(info.AnimationName, info.SheetName, positionMs, loop);
        }
        public static T ChooseRandom<T>(this T[] options)
        {
            var index = _rand.Next(0, options.Length - 1);
            return options[index];
        }

    }
}
