using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ruminate.GUI.Framework;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Ruminate.GUI.Content {

// ReSharper disable ClassNeverInstantiated.Global
    public class BorderRenderer : Renderer {
// ReSharper restore ClassNeverInstantiated.Global

        private Rectangle _corner, _edge;
        private readonly Rectangle _background;

        public int BorderWidth { get; set; }

        public BorderRenderer(Texture2D imageMap, Rectangle source, int borderWidth, int background) : base(imageMap) {

            BorderWidth = borderWidth;

            _corner = new Rectangle(source.Left, source.Top, BorderWidth, BorderWidth);
            _edge = new Rectangle(source.Left + borderWidth, source.Top, BorderWidth, BorderWidth);
            _background = new Rectangle(source.Left, source.Top + (4 * BorderWidth), background, background);
        }

        public override Rectangle BuildChildArea(Point size) {
            return new Rectangle(BorderWidth, BorderWidth, size.X - 2 * BorderWidth, size.Y - 2 * BorderWidth);
        }

        public override void Render(SpriteBatch batch, Rectangle destination) {

            // ##### Draw Background ##### //

            var drawArea = new Rectangle(
                destination.Left + BorderWidth, 
                destination.Top + BorderWidth, 
                destination.Width - (2 *BorderWidth), 
                destination.Height - (2 * BorderWidth));
            batch.Draw(ImageMap, drawArea, _background, Color.White);

            // ##### Draw Corners ##### //

            drawArea.Width = BorderWidth;
            drawArea.Height = BorderWidth;

            //Top Left
            drawArea.X = destination.Left; 
            drawArea.Y = destination.Top;            
            batch.Draw(ImageMap, drawArea, _corner, Color.White); 

            //Top Right
            drawArea.X = destination.Right - BorderWidth; 
            drawArea.Y = destination.Top;
            _corner.Y += BorderWidth;
            batch.Draw(ImageMap, drawArea, _corner, Color.White);

            //Bottom Right
            drawArea.X = destination.Right - BorderWidth;
            drawArea.Y = destination.Bottom - BorderWidth;
            _corner.Y += BorderWidth;
            batch.Draw(ImageMap, drawArea, _corner, Color.White);

            //Bottom Left
            drawArea.X = destination.Left;
            drawArea.Y = destination.Bottom - BorderWidth;
            _corner.Y += BorderWidth;
            batch.Draw(ImageMap, drawArea, _corner, Color.White);            

            _corner.Y -= (3 * BorderWidth);

            // ##### Draw Edges ##### //

            //Top Edge
            drawArea.X = destination.Left + BorderWidth;
            drawArea.Y = destination.Top;
            drawArea.Width = destination.Width - (2 * BorderWidth);
            batch.Draw(ImageMap, drawArea, _edge, Color.White);

            //Bottom Edge
            drawArea.Y = destination.Bottom - BorderWidth;
            _edge.Y += (2 * BorderWidth);
            batch.Draw(ImageMap, drawArea, _edge, Color.White);

            //Left Edge
            drawArea.X = destination.Left;
            drawArea.Y = destination.Top + BorderWidth;
            drawArea.Width = BorderWidth;
            drawArea.Height = destination.Height - (2 * BorderWidth);
            _edge.Y += BorderWidth;
            batch.Draw(ImageMap, drawArea, _edge, Color.White);

            //Right Edge
            drawArea.X = destination.Right - BorderWidth;
            _edge.Y -= (2 * BorderWidth);
            batch.Draw(ImageMap, drawArea, _edge, Color.White);

            _edge.Y -= (1 * BorderWidth);
        } 
    }
}
