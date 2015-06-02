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

        private MapDeserializationClasses.MapJSON _deserialized;

        private Dictionary<int, TeamSpawn> _spawnsByTeam =
            new Dictionary<int, TeamSpawn>();

        public string Name { get; private set; }
        public string Description { get; private set; }

        public IReadOnlyDictionary<int, TeamSpawn> SpawnsByTeam { get { return _spawnsByTeam; } }

        public static Map LoadMap(string mapData, GameCore game)
        {
            var map = new Map()
            {
                _game = game,
                _deserialized = MapDeserializationClasses.MapJSON.Load(mapData)
            };

            //Process basic
            foreach (var team in map._deserialized.Spawns)
            {
                var ts = new TeamSpawn();
                ts.TeamIndex = team.TeamIndex;
                foreach (var pos in team.SpawnPositions)
                    ts.Positions.Add(new TeamSpawn.SpawnPosition(pos));

                map._spawnsByTeam.Add(team.TeamIndex, ts);
            }

            return map;
        }

        /// <summary>
        /// Creates the map objects in game
        /// </summary>
        public void CreateObjects()
        {
            foreach (var mapObj in _deserialized.Objects)
            {
                MapObject obj = MapObject.ReflectiveInitialize(mapObj.TypeName, _game, true, mapObj.Position, mapObj.Rotation);
                obj.ColorMask = mapObj.Mask;

                _game.AddGameObject(obj, null, true);
            }
        }

        /// <summary>
        /// Gets a spawn position, by team
        /// </summary>
        /// <param name="teamIndex"></param>
        /// <returns></returns>
        public Vector2 GetSpawnPosition(int teamIndex)
        {
            if (SpawnsByTeam.ContainsKey(teamIndex))
            {
                foreach (var spawn in SpawnsByTeam[teamIndex].Positions)
                    if (!spawn.InUse) //Loop through and find an unused spawn point
                    {
                        spawn.ToggleInUse(true);
                        return spawn.Position;
                    }
                return SpawnsByTeam[teamIndex].Positions[0].Position;
            }

            return SpawnsByTeam[0].Positions[0].Position;
        }

        /// <summary>
        /// Release all of the spawn points so that they can be reused
        /// </summary>
        public void ResetSpawns()
        {
            foreach (var team in SpawnsByTeam.Values)
                foreach (var spawn in team.Positions)
                    spawn.ToggleInUse(false);
        }

        public class TeamSpawn
        {
            public int TeamIndex;
            public List<SpawnPosition> Positions = new List<SpawnPosition>();

            public class SpawnPosition
            {
                public Vector2 Position { get; private set; }
                public bool InUse { get; private set; }

                public SpawnPosition(Vector2 pos)
                {
                    Position = pos;
                }

                public void ToggleInUse(bool? value = null)
                {
                    if (value.HasValue)
                        InUse = value.Value;
                    else
                        InUse = !InUse;
                }
            }
        }
    }
}
