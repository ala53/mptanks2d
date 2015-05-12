using MPTanks.Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Assets
{
    #region Assets
    public static class Explosions
    {

        public static readonly SpriteAnimationInfo[] ExplosionAnimations = {
            new SpriteAnimationInfo("explosionAnim", "assets/animations/explosion.png"),
            new SpriteAnimationInfo("explosionAnim2", "assets/animations/explosion2.png"),
            new SpriteAnimationInfo("explosionAnim3", "assets/animations/explosion3.png")                                         
        };

        public static readonly SpriteAnimationInfo Explosion1 = ExplosionAnimations[0];
        public static readonly SpriteAnimationInfo Explosion2 = ExplosionAnimations[1];
        public static readonly SpriteAnimationInfo Explosion3 = ExplosionAnimations[2];
    }

    public static class SmokePuffs
    {
        const string asset = "assets/other/smokepuff.png";
        public static readonly SpriteInfo[] SmokePuffSprites = {
            new SpriteInfo("puff_0", asset),
            new SpriteInfo("puff_1", asset),
            new SpriteInfo("puff_2", asset),
            new SpriteInfo("puff_3", asset),
            new SpriteInfo("puff_4", asset),
            new SpriteInfo("puff_5", asset),
            new SpriteInfo("puff_6", asset),
            new SpriteInfo("puff_7", asset),
            new SpriteInfo("puff_8", asset),
            new SpriteInfo("puff_9", asset),
            new SpriteInfo("puff_10", asset),
            new SpriteInfo("puff_11", asset),
            new SpriteInfo("puff_12", asset),
            new SpriteInfo("puff_13", asset),
            new SpriteInfo("puff_14", asset),
            new SpriteInfo("puff_15", asset)
        };
    }
    #endregion
    public static class AssetHelper
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

        public static SpriteAnimationInfo ChooseRandom(SpriteAnimationInfo[] options)
        {
            return options[_rand.Next(0, options.Length)];
        }
        public static SpriteInfo ChooseRandom(SpriteInfo[] options)
        {
            var index = _rand.Next(0, options.Length - 1);
            return options[index];
        }
    }
}
