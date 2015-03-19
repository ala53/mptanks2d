using Microsoft.Xna.Framework;
using Ruminate.GUI.Framework;

namespace Ruminate.GUI.Content {

    public class PanelRenderRule : RenderRule {

        private Rectangle _area;
        public override Rectangle Area {
            get { return _area; }
            set { _area = value; }
        }

        public int BorderWidth { get { return _default.BorderWidth; } }
        public override Rectangle SafeArea {
            get {
                return new Rectangle(
                    Area.X + BorderWidth,
                    Area.Y + BorderWidth,
                    Area.Width - (BorderWidth * 2),
                    Area.Height - (BorderWidth * 2));
            }
        }

        private BorderRenderer _default;

        public override void SetSize(int w, int h) {
            _area.Width = w;
            _area.Height = h;
        }        

        protected override void LoadRenderers() {

            _default = LoadRenderer<BorderRenderer>(Skin, "panel");
        }

        public Rectangle BuildChildArea(Point size) {

            return _default.BuildChildArea(size);
        }

        public override void Draw() {
            _default.Render(RenderManager.SpriteBatch, Area);
        }

        public override void DrawNoClipping() { }
    }
}