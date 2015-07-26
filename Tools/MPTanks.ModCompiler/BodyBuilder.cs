using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.ModCompiler
{
    public class BodyBuilder
    {
        public static string ProcessObject(Engine.Serialization.GameObjectComponentsJSON componentObject)
        {
            if (componentObject.__image__body != null)
            {
                var img = new Bitmap(componentObject.__image__body);
                var data = new uint[img.Width * img.Height];
                for (var y = 0; y < img.Height; y++)
                    for (var x = 0; x < img.Width; x++)
                    {
                        data[(img.Height * y) + x] = unchecked((uint)img.GetPixel(x, y).ToArgb());
                    }

                var built = BuildBody(data, img.Width, new Vector2(componentObject.DefaultSize.X, componentObject.DefaultSize.Y));
                componentObject.Body = built;

                img.Dispose();
            }
            return JsonConvert.SerializeObject(componentObject, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
        public static MPTanks.Engine.Serialization.GameObjectBodySpecifierJSON BuildBody(uint[] imageData, int width, Vector2 objSize)
        {
            var vertices = PolygonTools.CreatePolygon(imageData, width, true);
            var imgHeight = imageData.Length / width;
            var scale = new Vector2(objSize.X / width, objSize.Y / imgHeight);
            vertices.Scale(scale);
            var decomposed = Triangulate.ConvexPartition(vertices, TriangulationAlgorithm.Bayazit);

            var result = new Engine.Serialization.GameObjectBodySpecifierJSON();
            var fixtures = new List<Engine.Serialization.GameObjectBodySpecifierJSON.FixtureSpecifierJSON>();

            foreach (var fixture in decomposed)
            {
                var fx = new Engine.Serialization.GameObjectBodySpecifierJSON.FixtureSpecifierJSON();
                var vertList = new List<Engine.Serialization.JSONVector>();
                var holeList = new List<Engine.Serialization.GameObjectBodySpecifierJSON.FixtureSpecifierJSON.HolesSpecifierJSON>();
                foreach (var vert in fixture)
                    vertList.Add(new Engine.Serialization.JSONVector { X = vert.X, Y = vert.Y });

                foreach (var h in fixture.Holes) {
                    var hole = new Engine.Serialization.GameObjectBodySpecifierJSON.FixtureSpecifierJSON.HolesSpecifierJSON();
                    var vList = new List<Engine.Serialization.JSONVector>();
                    foreach (var v in h)
                        vList.Add(new Engine.Serialization.JSONVector { X = v.X, Y = v.Y });
                    hole.Vertices = vList.ToArray();

                    holeList.Add(hole);
                }

                fx.Vertices = vertList.ToArray();
                fx.Holes = holeList.ToArray();
                fixtures.Add(fx);
            }

            result.Fixtures = fixtures.ToArray();
            result.Size = new Engine.Serialization.JSONVector { X = objSize.X, Y = objSize.Y };
            return result;
        }
    }
}
