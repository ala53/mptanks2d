using MPTanks_MK4.Helpers;
using Newtonsoft.Json;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4.Rendering.Sprites
{
    class Sprite
    {
        [JsonIgnore]
        public SpriteSheet Sheet { get; set; }
        public Vector2 TopLeft { get; set; }
        public Vector2 Size { get; set; }
        public Rectangle Rectangle
        {
            get { return new Vector4(TopLeft.X, TopLeft.Y, Size.X, Size.Y); }
            set { TopLeft = Rectangle.Position; Size = Rectangle.Size; }
        }
        /// <summary>
        /// Gets the Normalized rectangle rather than the pixel measured rectangle for the object.
        /// </summary>
        public Rectangle Normalized
        {
            get
            {
                return new Rectangle(
                    Rectangle.X / Sheet.Width,
                    Rectangle.Y / Sheet.Height,
                    Rectangle.Width / Sheet.Width,
                    Rectangle.Height / Sheet.Height
                    );
            }
        }
    }
}
