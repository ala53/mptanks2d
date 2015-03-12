using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK5.Rendering
{
    struct RectangleF
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public float Left { get { return X; } }
        public float Right { get { return X + Width; } }
        public float Top { get { return Y; } }
        public float Bottom { get { return Y+Height; } }

        public RectangleF(float x, float y, float width, float height)
        {
            X = x; Y = y; Width = width; Height = height;
        }
    }
}
