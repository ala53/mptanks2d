using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using MPTanks.Client.GameSandbox;
using MPTanks.Engine;
using MPTanks.Engine.Gamemodes;
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
        public string MapName { get; set; }
        public string MapAuthor { get; set; }
        public Color BackgroundColor = Color.Black;
        public Color ShadowColor = new Color(50, 50, 50, 100);
        public Vector2 ShadowOffset = new Vector2(0.3f, -0.3f);

        public GameCore CreateFromMap(MapJSON map) => CreateGameFromMapAndUpdateModsList(map);

        private GameCore CreateGameFromMapAndUpdateModsList(MapJSON map)
        {
            MapName = map.Name;
            MapAuthor = map.Author;
            BackgroundColor = map.BackgroundColor;
            ShadowColor = map.ShadowColor;
            ShadowOffset = map.ShadowOffset;

            var game = new GameCore(null, new NullGamemode(), Newtonsoft.Json.JsonConvert.SerializeObject(map));
            game.Authoritative = true;
            game.BeginGame(true);
            //Blow up the mod infos
            var deps = map.ModDependencies.Select(a =>
             {
                 var name = a.Split(':')[0];
                 var major = int.Parse(a.Split(':')[1].Split('.')[0]);
                 var minor = int.Parse(a.Split(':')[1].Split('.')[1].Split('-')[0]);

                 return new Modding.ModInfo()
                 {
                     ModName = name,
                     ModMajor = major,
                     ModMinor = minor
                 };
             });

            //And make sure they're loaded
            foreach (var itm in deps)
            {
                var db = Modding.ModDatabase.Get(itm.ModName, itm.ModMajor);
                if (db == null || db.Minor < itm.ModMinor)
                {
                    throw new Exception($"Mod {db.Name} v{db.Major}.{db.Minor} not found or is out of date.");
                }

                string err;
                var mod = db.LoadIfNotLoaded(GameSettings.Instance.ModUnpackPath,
                    GameSettings.Instance.ModMapPath,
                    GameSettings.Instance.ModAssetPath, out err);
                if (mod == null)
                    throw new Exception($"Failed to load mod {db.Name} v{db.Major}.{db.Minor}", new Exception(err));
            }

            Mods = deps.ToList();

            //Recreate the SpawnPoint objects from the map spawns list

            var spawns = map.Spawns.SelectMany(a =>
            {
                return a.SpawnPositions.Select(b =>
                {
                    dynamic gObj = game.AddMapObject("CoreAssets+SpawnPoint", true);
                    gObj.Team = (short)a.TeamIndex;
                    gObj.Position = b;
                    return (Engine.Maps.MapObjects.MapObject)gObj;
                });
            });

            return game;
        }

        private MapJSON CreateMapFromGame(GameCore game)
        {
            var mapJSON = new MapJSON();
            //Find all of the objects and spawns
            var objs = game.GameObjects.Where(a => a.GetType().FullName != "MPTanks.CoreAssets.MapObjects.SpawnPoint")
                .Where(a => a.GetType().IsSubclassOf(typeof(Engine.Maps.MapObjects.MapObject)))
                .Select(a => (Engine.Maps.MapObjects.MapObject)a);
            var spawns = game.GameObjects.Where(a => a.GetType().FullName == "MPTanks.CoreAssets.MapObjects.SpawnPoint")
                .Select(a => (dynamic)a); //Cast to dynamic because we *cannot* preload the CoreAssets assembly

            //Remap the spawns from SpawnPoint mapobjects to "spawns" in map terms

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

            mapJSON.Author = MapAuthor;
            mapJSON.Name = MapName;
            mapJSON.ShadowColor = ShadowColor;
            mapJSON.ShadowOffset = ShadowOffset;
            mapJSON.BackgroundColor = BackgroundColor;

            return mapJSON;
        }

        public static string Default => Newtonsoft.Json.JsonConvert.SerializeObject(new MapJSON());

        public string GenerateMap(GameCore game)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(CreateMapFromGame(game), Newtonsoft.Json.Formatting.Indented);
        }
    }
}
