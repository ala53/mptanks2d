using Microsoft.Xna.Framework;
using MPTanks.Engine.Maps.MapObjects;
using MPTanks.Engine.Maps.Serialization;
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

        private MapJSON _deserialized;

        private Dictionary<int, TeamSpawn> _spawnsByTeam =
            new Dictionary<int, TeamSpawn>();

        public string Name { get; private set; }
        public string Description { get; private set; }

        public IReadOnlyDictionary<int, TeamSpawn> SpawnsByTeam { get { return _spawnsByTeam; } }

        private string _data;
        public string RawData { get { return _data; } }

        public Color ShadowColor { get; set; }
        public Vector2 ShadowOffset { get; set; }

        public static Map LoadMap(string mapData, GameCore game)
        {
            var map = new Map()
            {
                _game = game,
                _deserialized = MapJSON.Load(mapData),
                _data = mapData
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

        public override string ToString()
        {
            return _data;
        }

        /// <summary>
        /// Creates the map objects in game
        /// </summary>
        public void CreateObjects()
        {
            if (ShadowOffset == null)
                ShadowOffset = new Vector2(0.25f);
            else ShadowOffset = _deserialized.ShadowOffset;
            if (ShadowColor == null)
                ShadowColor = new Color(50, 50, 50, 100);
            else ShadowColor = _deserialized.ShadowColor;

            foreach (var mapObj in _deserialized.Objects)
            {
                MapObject obj = MapObject.ReflectiveInitialize(mapObj.TypeName, _game, true, mapObj.Position, mapObj.Rotation);
                obj.ColorMask = mapObj.Mask;
                obj.Size = mapObj.DesiredSize;

                _game.AddGameObject(obj, null, true);
            }
        }

        private Random random = new Random();
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
                return SpawnsByTeam[teamIndex].Positions[random.Next(0, SpawnsByTeam[teamIndex].Positions.Count - 1)].Position;
            }

            var teamToSpawnOn = SpawnsByTeam[random.Next(0, SpawnsByTeam.Count - 1)];
            return teamToSpawnOn.Positions[random.Next(0, teamToSpawnOn.Positions.Count - 1)].Position;
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
