using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Maps.MapDeserializationClasses
{
    [JsonObject(MemberSerialization.OptOut)]
    public class MapJSON
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public MapVectorJSON Size { get; set; }
        public bool FogOfWar { get; private set; }
        public int MaxPlayers { get; set; }
        public bool WhitelistGamemodes { get; set; }
        public string[] AllowedGamemodes { get; set; }
        public BackgroundTileJSON[] Background { get; set; }
        public MapTeamsJSON[] Spawns { get; set; }
        public MapObjectJSON[] Objects { get; set; }
        public string[] ModDependencies { get; set; }

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
        public MapVectorJSON Position { get; set; }
        public MapVectorJSON Repeat { get; set; }
    }

    public class MapTeamsJSON
    {
        public int TeamIndex { get; set; }
        public MapVectorJSON[] SpawnPositions { get; set; }
    }

    public class MapVectorJSON
    {
        [JsonProperty("x")]
        public float X { get; set; }
        [JsonProperty("y")]
        public float Y { get; set; }

        public static implicit operator Vector2(MapVectorJSON vec)
        {
            return new Vector2(vec.X, vec.Y);
        }

        public static implicit operator MapVectorJSON(Vector2 vec)
        {
            return new MapVectorJSON { X = vec.X, Y = vec.Y };
        }
    }

    public class MapObjectJSON
    {
        public string TypeName { get; set; }
        public float Rotation { get; set; }
        public MapVectorJSON Position { get; set; }
        public MapVectorJSON Size { get; set; }
        public Color Mask { get; set; }
    }
}
