using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Serialization
{
    /// <summary>
    /// A vector2 that is serializable as a JSON object. A workaround for inconsistent serialization of Vector2's.
    /// </summary>
    public class JSONVector
    {
        [JsonProperty("x")]
        public float X { get; set; }
        [JsonProperty("y")]
        public float Y { get; set; }

        public static implicit operator Vector2(JSONVector vec)
        {
            if (vec == null) return default(Vector2);
            return new Vector2(vec.X, vec.Y);
        }

        public static implicit operator JSONVector(Vector2 vec)
        {
            return new JSONVector { X = vec.X, Y = vec.Y };
        }

    }
}