using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ruminate.GUI.Framework;

namespace Ruminate.GUI.Content {

    public class LabelRenderRule : FontRenderRule {

        private Rectangle _area, _icon;
        public override Rectangle Area {
            get {
                return _area;
            }
            set {
                _area = value;
                _icon.X = _area.X;
                _icon.Y = _area.Y;
            }
        }

        private Texture2D _image;
        public Texture2D Image {
            get { 
                return _image; 
            } set {
                _image = value;
                _icon.Width = _image.Width;
                _icon.Height = _image.Height;
            }
        }

        public string Label { get; set; }

        public override void SetSize(int w, int h) {
            _area.Width = w; 
            _area.Height = h;
        }

        protected override void LoadRenderers() { }

        public override void Draw() {            

            if (Label != null) {                
                TextRenderer.Render(RenderManager.SpriteBatch, Label, _area,
                    TextHorizontal.RightAligned,
                    TextVertical.CenterAligned);
            }

            if (Image != null) {
                RenderManager.SpriteBatch.Draw(Image, _icon, Color.White);
            }
        }

        public override void DrawNoClipping() { }
    }
}
