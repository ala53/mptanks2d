using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Maps.MapDeserializationClasses
{
    [JsonObjectAttribute(MemberSerialization.OptOut)]
    internal class MapJSON
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public Vector2 Size { get; set; }
        public int MaxPlayers { get; set; }
        public bool WhitelistGamemodes { get; set; }
        public string[] AllowedGamemodes { get; set; }
        public BackgroundTileJSON[] Background { get; set; }
        public MapTeamsJSON[] Spawns { get; set; }
        public MapObjectJSON[] Objects { get; set; }

        public static MapJSON Load(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<MapJSON>(data);
        }
    }

    internal class BackgroundTileJSON
    {
        public string SpriteName { get; set; }
        public string AssetFileName { get; set; }
        public Color Mask { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Repeat { get; set; }
    }

    internal class MapTeamsJSON
    {
        public int TeamIndex { get; set; }
        public Vector2[] SpawnPositions { get; set; }
    }

    internal class MapObjectJSON
    {
        public string TypeName { get; set; }
        public float Rotation { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Color Mask { get; set; }
    }
}
