using Microsoft.Xna.Framework;
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
        [DefaultValue(0)]
        public float R { get; set; }
        [DefaultValue(0)]
        public float G { get; set; }
        [DefaultValue(0)]
        public float B { get; set; }
        [DefaultValue(255)]
        public float A { get; set; }

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
