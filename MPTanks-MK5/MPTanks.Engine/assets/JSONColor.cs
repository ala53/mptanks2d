using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Serialization
{
    public class JSONColor
    {
        [JsonProperty("r")]
        [DefaultValue((byte)0)]
        public byte R { get; set; }
        [JsonProperty("g")]
        [DefaultValue((byte)0)]
        public byte G { get; set; }
        [JsonProperty("b")]
        [DefaultValue((byte)0)]
        public byte B { get; set; }
        [JsonProperty("a")]
        [DefaultValue((byte)255)]
        public byte A { get; set; }

        public static implicit operator Color(JSONColor color)
        {
            if (color == null)
                return Color.Black;

            return new Color(color.R, color.G, color.B, color.A);
        }

        public static implicit operator JSONColor(Color color)
        {
            return new JSONColor
            {
                R = color.R,
                G = color.G,
                B = color.B,
                A = color.A
            };
        }
    }
}
