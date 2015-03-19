using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ruminate.Utils {

    /// <summary>
    /// 
    /// </summary>
    internal struct DrawSpot {
        public Vector2 Vector;
        public Rectangle Area;
        public Color Color;
    }

    /// <summary>
    /// 
    /// </summary>
    public class DebugUtils {

        /// <summary>
        /// 
        /// </summary>
        public static Texture2D White;
        public static SpriteFont Font;

        /// <summary>
        /// 
        /// </summary>
        private static List<DrawSpot> _drawAreas = new List<DrawSpot>();
        private static readonly List<DrawSpot> DrawAreasPersistant = new List<DrawSpot>();

        /// <summary>
        /// 
        /// </summary>
        public static void Init(GraphicsDevice graphicsDevice, SpriteFont font) {
            White = new Texture2D(graphicsDevice, 1, 1);            
            var color = Color.White;
            White.SetData(new[] { color });

            Font = font;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="color"></param>
        public static void AddDrawRectangle(Rectangle area, Color color) {
            _drawAreas.Add(new DrawSpot { Vector = Vector2.Zero, Area = area, Color = color });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="color"></param>
        public static void AddDrawRectanglePersistant(Rectangle area, Color color) {
            DrawAreasPersistant.Add(new DrawSpot { Vector = Vector2.Zero, Area = area, Color = color });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="color"></param>
        public static void AddVectorPoint(Vector2 vector, Color color) {
            _drawAreas.Add(new DrawSpot { Vector = vector, Area = Rectangle.Empty, Color = color });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void Draw(SpriteBatch spriteBatch) {
            foreach (var drawSpot in _drawAreas) {
                if (drawSpot.Area != Rectangle.Empty)
                    spriteBatch.Draw(White, drawSpot.Area, drawSpot.Color);
                else if (drawSpot.Vector != Vector2.Zero) {
                    spriteBatch.Draw(White, drawSpot.Vector, drawSpot.Color);
                }
            }

            foreach (var drawSpot in DrawAreasPersistant) {
                if (drawSpot.Area != Rectangle.Empty)
                    spriteBatch.Draw(White, drawSpot.Area, drawSpot.Color);
                else if (drawSpot.Vector != Vector2.Zero) {
                    spriteBatch.Draw(White, drawSpot.Vector, drawSpot.Color);
                }
            }

            _drawAreas = new List<DrawSpot>();
        }
    }    
}
