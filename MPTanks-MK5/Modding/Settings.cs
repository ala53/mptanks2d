using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Modding
{
    static class Settings
    {
        public const string EngineNS = "MPTanks.Engine";
        public const string TankTypeName = EngineNS + ".Tanks.Tank";
        public const string GamemodeTypeName = EngineNS + ".Gamemodes.Gamemode";
        public const string MapObjectTypeName = EngineNS + ".Maps.MapObjects.MapObject";
        public const string ProjectileTypeName = EngineNS + ".Projectiles.Projectile";
    }
}
