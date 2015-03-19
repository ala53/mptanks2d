using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Point;

namespace Ruminate.Utils {

    public static class ConverterExtentions {
        
        public static Vector2 ToVector2(this Point point) {
            return new Vector2(point.X, point.Y);
        }

        public static Point ToPoint(this Vector2 vector) {
            return new Point((int)vector.X, (int)vector.Y);
        }
    }
}
