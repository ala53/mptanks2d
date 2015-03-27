using Microsoft.Xna.Framework;
using MPTanks.Engine.Maps.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Maps
{
    public class Map
    {
        private GameCore _game;
        private List<MapObject> _objects = new List<MapObject>();
        public IReadOnlyList<MapObject> Objects { get { return _objects; } }

        private List<TeamSpawns> _spawns = new List<TeamSpawns>();
        public static Map LoadMap(string mapData, GameCore game)
        {
            var map =  new Map() { _game = game };
            dynamic decoded = Newtonsoft.Json.JsonConvert.DeserializeObject(mapData);



            return map;
        }

        /// <summary>
        /// Gets a spawn position, by team
        /// </summary>
        /// <param name="teamIndex"></param>
        /// <returns></returns>
        public Vector2 GetSpawnPosition(int teamIndex)
        {
            return new Vector2(_game.SharedRandom.Next(0, 100), _game.SharedRandom.Next(0, 100));
        }

        private class TeamSpawns
        {
            public int Index;
            public List<Vector2> Positions = new List<Vector2>();
        }
    }
}
