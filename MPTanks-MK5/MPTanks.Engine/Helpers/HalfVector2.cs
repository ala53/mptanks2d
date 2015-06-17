using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Helpers
{
    public struct HalfVector2
    {
        internal Half _x, _y;
        

        public float X { get { return _x; } }
        public float Y { get { return _y; } }

        public HalfVector2(float x, float y)
        {
            _x = (Half)x;
            _y = (Half)y;
        }
        public HalfVector2(Half x, Half y)
        {
            _x = x;
            _y = y;
        }
        public HalfVector2(Half x, float y)
        {
            _x = (Half)x;
            _y = (Half)y;
        }
        public HalfVector2(float x, Half y)
        {
            _x = (Half)x;
            _y = y;
        }


        public static explicit operator HalfVector2(Vector2 value)
        {
            return new HalfVector2() { _x = (Half)value.X, _y = (Half)value.Y };
        }
        public static implicit operator Vector2(HalfVector2 value)
        {
            return new Vector2() { X = value.X, Y = value.Y };
        }
    }
}
