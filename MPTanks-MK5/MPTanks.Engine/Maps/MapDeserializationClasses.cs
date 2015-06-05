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
    class MapJSON
    {
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

        public static MapJSON Load(string data)
        {
            return JsonConvert.DeserializeObject<MapJSON>(data);
        }
    }

    class MapModJSON
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string MinVersion { get; set; }
    }

    class BackgroundTileJSON
    {
        public string SpriteName { get; set; }
        public string AssetFileName { get; set; }
        public Color Mask { get; set; }
        public JSONVector Position { get; set; }
        public JSONVector Repeat { get; set; }
    }

    class MapTeamsJSON
    {
        public int TeamIndex { get; set; }
        public JSONVector[] SpawnPositions { get; set; }
    }

    class MapObjectJSON
    {
        public string TypeName { get; set; }
        public float Rotation { get; set; }
        public JSONVector Position { get; set; }
        public JSONVector Size { get; set; }
        public Color Mask { get; set; }
    }
}
