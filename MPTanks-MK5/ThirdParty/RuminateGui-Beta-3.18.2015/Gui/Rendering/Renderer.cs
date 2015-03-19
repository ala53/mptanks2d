using Microsoft.Xna.Framework.Graphics;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Ruminate.GUI.Framework {

    abstract public class Renderer {

        protected Texture2D ImageMap { get; set; }

        protected Renderer(Texture2D imageMap) { ImageMap = imageMap; }

        abstract public Rectangle BuildChildArea(Point size);
        abstract public void Render(SpriteBatch batch, Rectangle destination);
    }
}
