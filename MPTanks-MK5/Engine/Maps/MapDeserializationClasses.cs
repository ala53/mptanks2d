using Microsoft.Xna.Framework;
using MPTanks.Engine.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Maps.Serialization
{
    public class MapJSON
    {
        /// <summary>
        /// For mod compiler type detection
        /// </summary>
        public string Type { get; set; } = "map";
        public string Name { get; set; }
        public string Author { get; set; }
        public JSONVector Size { get; set; }
        public bool FogOfWar { get; private set; }
        public int MaxPlayers { get; set; }
        public bool WhitelistGamemodes { get; set; }
        public string[] AllowedGamemodes { get; set; }
        public BackgroundTileJSON[] Background { get; set; }
        public MapTeamsJSON[] Spawns { get; set; }
        public MapObjectJSON[] Objects { get; set; }
        public string[] ModDependencies { get; set; }
        public JSONVector ShadowOffset { get; set; }
        public JSONColor ShadowColor { get; set; }
        public JSONColor BackgroundColor { get; set; }

        public static MapJSON Load(string data)
        {
            return JsonConvert.DeserializeObject<MapJSON>(data);
        }
    }

    public class MapModJSON
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string MinVersion { get; set; }
    }

    public class BackgroundTileJSON
    {
        public string SpriteName { get; set; }
        public string AssetFileName { get; set; }
        public Color Mask { get; set; }
        public JSONVector Position { get; set; }
    }

    public class MapTeamsJSON
    {
        public int TeamIndex { get; set; }
        public JSONVector[] SpawnPositions { get; set; }
    }

    public class MapObjectJSON
    {
        [JsonProperty("reflectionName")]
        public string ReflectionName { get; set; }
        public float Rotation { get; set; }
        public JSONVector Position { get; set; }
        [JsonProperty("size")]
        public JSONVector DesiredSize { get; set; }
        [JsonProperty("settings")]
        public Dictionary<string, string> ConfiguredSettings { get; set; }
        public Color Mask { get; set; }
        public int DrawLayer { get; set; }
    }
}
