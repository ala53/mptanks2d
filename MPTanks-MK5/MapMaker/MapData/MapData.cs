using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using MPTanks.Engine;
using MPTanks.Engine.Maps.Serialization;
using MPTanks.Engine.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.MapMaker.MapData
{
    class MapData
    {
        public List<Modding.ModInfo> Mods { get; set; } = new List<Modding.ModInfo>();


        private MapJSON CreateMapFromGame(GameCore game)
        {
            var mapJSON = new MapJSON();
            //Find all of the objects and spawns
            var objs = game.GameObjects.Where(a => a.GetType().FullName != "MPTanks.CoreAssets.MapObjects.SpawnPoint")
                .Where(a => a.GetType().IsSubclassOf(typeof(Engine.Maps.MapObjects.MapObject)))
                .Select(a => (Engine.Maps.MapObjects.MapObject)a);
            var spawns = game.GameObjects.Where(a => a.GetType().FullName == "MPTanks.CoreAssets.MapObjects.SpawnPoint")
                .Select(a => (dynamic)a); //Cast to dynamic because we *cannot* preload the CoreAssets assembly

            //Remap the spawns from SpawnPoint objects to "spawns"

            //Find out how many teams
            var teams = spawns.Select(a => a.Team).Distinct()
                .Select(a => new MapTeamsJSON() { TeamIndex = a })
                .ToArray();

            //And append the spawn points
            foreach (var team in teams)
            {
                team.SpawnPositions = spawns
                    .Where(a => a.Team == team.TeamIndex)
                    .Select(a => a.Position)
                    .Select(a => new JSONVector { X = a.X, Y = a.Y })
                    .ToArray();
            }

            mapJSON.Spawns = teams;

            //Map all of the objects
            mapJSON.Objects = objs.Select(a => new MapObjectJSON
            {
                ConfiguredSettings = a.InstanceSettings.ToDictionary(b => b.Key, b => b.Value),
                DesiredSize = a.Size,
                DrawLayer = a.DrawLayer,
                Mask = a.ColorMask,
                Position = a.Position,
                ReflectionName = a.ReflectionName,
                Rotation = a.Rotation
            }).ToArray();

            mapJSON.ModDependencies = Mods.Select(a => a.ModName + ":" + a.ModMajor + "." + a.ModMinor).ToArray();

            //And find the map size
            Vector2 min = new Vector2(float.PositiveInfinity), max = new Vector2(float.NegativeInfinity);

            //Loop through the bounding boxes of each object and find the minimum and maximum
            foreach (var obj in game.GameObjects)
            {
                //Get the aabb
                var aabbs = obj.Body.FixtureList.Select(a =>
                {
                    Transform tf;
                    obj.Body.GetTransform(out tf);
                    AABB aabb;
                    a.Shape.ComputeAABB(out aabb, ref tf, 0);
                    return aabb;
                });

                foreach (var ab in aabbs)
                {
                    foreach (var vertF in ab.Vertices)
                    {
                        var vert = vertF;
                        vert /= game.Settings.PhysicsScale;
                        if (vert.X > max.X)
                            max.X = vert.X;
                        if (vert.Y > max.Y)
                            max.Y = vert.Y;
                        if (vert.X < min.X)
                            min.X = vert.X;
                        if (vert.Y < min.Y)
                            min.Y = vert.Y;
                    }
                }
            }

            mapJSON.Size = new JSONVector
            {
                X = max.X - min.X,
                Y = max.Y - min.Y
            };
            mapJSON.TopLeft = min;
            mapJSON.BottomRight = max;

            return mapJSON;
        }

        public static string Default => Newtonsoft.Json.JsonConvert.SerializeObject(new MapJSON());

        public string GenerateMap(GameCore game)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(CreateMapFromGame(game));
        }
    }
}
