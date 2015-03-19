using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Ruminate.GUI.Content {

// ReSharper disable ClassNeverInstantiated.Global
    public class HorizontalSlidingDoorRenderer : SlidingDoorRenderer {
// ReSharper restore ClassNeverInstantiated.Global

        private readonly Rectangle _center, _edge;
        private readonly int _buffer;

        public override int Across { get { return _edge.Height; } }
        public override int Lenght { get { return _center.Width; } }
        public override int Edge { get { return _edge.Width; } }
        public override int Buffer { get { return _buffer; } }

        public HorizontalSlidingDoorRenderer(Texture2D imageMap, Rectangle source, int center, int edge, int buffer)
            : base(imageMap) {

            _center = new Rectangle(source.Left, source.Top, center, source.Height);
            _edge = new Rectangle(source.Left + center, source.Top, edge, source.Height);
            _buffer = buffer;
        }

        public override Rectangle BuildChildArea(Point size) { 
            return Rectangle.Empty;
        }

        public override void Render(SpriteBatch batch, Rectangle destination) {

            var drawArea = new Rectangle(destination.Left, destination.Top, _edge.Width, _edge.Height);
            batch.Draw(ImageMap, drawArea, _edge, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);

            drawArea.X = destination.Right - _edge.Width;
            batch.Draw(ImageMap, drawArea, _edge, Color.White);

            drawArea.X = destination.Left + _edge.Width;
            drawArea.Width = destination.Width - (2 * _edge.Width);
            batch.Draw(ImageMap, drawArea, _center, Color.White);
        }
    }
}
