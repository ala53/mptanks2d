using MPTanks.Engine.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Modding.Mods.Core.Assets
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

    public static class SatelliteDish
    {
        const string assetName = "assets/mapobjects/moving/satellite_dish.png";
        public static readonly SpriteInfo Base = new SpriteInfo("baseblock", assetName);
        public static readonly SpriteInfo DishAndRevolver = new SpriteInfo("dishandrevolver", assetName);
    }

}
