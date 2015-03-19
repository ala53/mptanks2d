using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ruminate.GUI.Framework {

    public class Text {

        public SpriteFont SpriteFont { get; set; }

        public Color Color { get; set; }

        public Text(SpriteFont font, Color color) {

            SpriteFont = font;
            Color = color;
        }

        public void Render(SpriteBatch spriteBatch, string value, Rectangle renderArea,
            TextHorizontal h = TextHorizontal.LeftAligned, TextVertical v = TextVertical.TopAligned) {

            var location = new Vector2(renderArea.X, renderArea.Y);

            var size = SpriteFont.MeasureString(value);

            switch (h) {
                case TextHorizontal.CenterAligned:
                    location.X += (renderArea.Width - size.X) / 1.9f;
                    break;
                case TextHorizontal.RightAligned:
                    location.X += renderArea.Width - size.X;
                    break;
            }

            switch (v) {
                case TextVertical.CenterAligned:
                    location.Y += (renderArea.Height - size.Y) / 1.9f;
                    break;
                case TextVertical.BottomAligned:
                    location.Y += renderArea.Height - size.Y;
                    break;
            }

            spriteBatch.DrawString(SpriteFont, value, location, Color);
        }
    }
}
