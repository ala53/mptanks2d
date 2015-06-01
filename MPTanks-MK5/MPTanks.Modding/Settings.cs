using System;
using System.IO;

namespace MPTanks.Modding
{
    static class Settings
    {
        public static readonly string ConfigDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "MP Tanks 2D");
        
        public const string EngineNS = "MPTanks.Engine";
        public const string TankTypeName = EngineNS + ".Tanks.Tank";
        public const string GamemodeTypeName = EngineNS + ".Gamemodes.Gamemode";
        public const string MapObjectTypeName = EngineNS + ".Maps.MapObjects.MapObject";
        public const string ProjectileTypeName = EngineNS + ".Projectiles.Projectile";
    }
}
