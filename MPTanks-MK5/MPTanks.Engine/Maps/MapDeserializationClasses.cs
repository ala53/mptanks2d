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
            return Newtonsoft.Json.JsonConvert.DeserializeObject<MapJSON>(data);
        }
    }

    internal class MapModJSON
    {
        public string Name { get; set; }
        public string Author { get; set; }
        /// <summary>
        /// Version can be *, which means either installed or current version aka whatever
        /// Version can be 1.0+, which means version 1.0, 1.1, 1.2, 2.0, etc. AKA 1.0 or greater
        /// Version can be 1.0, which means exactly version 1
        /// </summary>
        public string Version { get; set; }
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
