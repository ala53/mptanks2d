using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Assets
{
    public static class BasicTank
    {
        const string asset = "assets/tanks/basictank2d.png";
        const string projectileAsset = "assets/animations/basictank_projectile.png";
        public static readonly SpriteInfo TurretBase = new SpriteInfo("turretbase", asset);
        public static readonly SpriteInfo GrillMask = new SpriteInfo("grillmask", asset);
        public static readonly SpriteInfo MainGunSparks = new SpriteInfo("gunsparks", asset);
        public static readonly SpriteAnimationInfo MainProjectile = new SpriteAnimationInfo("projectileAnim", projectileAsset);
    }

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
