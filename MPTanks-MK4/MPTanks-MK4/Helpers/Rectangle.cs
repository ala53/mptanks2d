using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4.Helpers
{
    struct Rectangle
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Width;
        public readonly float Height;

        public Vector2 Position
        {
            get { return new Vector2(X, Y); }
        }

        public Vector2 Size
        {
            get { return new Vector2(Width, Height); }
        }

        public Rectangle(Vector4 rect)
        {
            X = rect.X;
            Y = rect.Y;
            Width = rect.Z;
            Height = rect.W;
        }

        public Rectangle(Vector2 pos, Vector2 size)
        {
            X = pos.X;
            Y = pos.Y;
            Width = size.X;
            Height = size.Y;
        }

        public Rectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public static implicit operator Vector4(Rectangle rect)
        {
            return new Vector4(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static implicit operator Rectangle(Vector4 rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Z, rect.W);
        }
    }
}
