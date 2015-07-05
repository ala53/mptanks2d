using System;
using System.IO;

namespace MPTanks.Modding
{
    static class Settings
    {
        public static readonly string ConfigDir = "";

        static Settings()
        {
            Directory.CreateDirectory(MetadataModUnpackDir);
            if (File.Exists("configpath.txt"))
#if DEBUG
                ConfigDir = Environment.ExpandEnvironmentVariables(File.ReadAllLines("configpath.txt")[0]);
#else
                ConfigDir = Environment.ExpandEnvironmentVariables(File.ReadAllLines("configpath.txt")[1]);
#endif
            if (ConfigDir != "")
                Directory.CreateDirectory(ConfigDir);
        }
        public static readonly string MetadataModUnpackDir = Path.Combine(ConfigDir, "tempmodmetadataunpack");

        public const string EngineNS = "MPTanks.Engine";
        public const string TankTypeName = EngineNS + ".Tanks.Tank";
        public const string GamemodeTypeName = EngineNS + ".Gamemodes.Gamemode";
        public const string MapObjectTypeName = EngineNS + ".Maps.MapObjects.MapObject";
        public const string ProjectileTypeName = EngineNS + ".Projectiles.Projectile";
        public const string GameObjectTypeName = EngineNS + ".GameObject";
    }
}
