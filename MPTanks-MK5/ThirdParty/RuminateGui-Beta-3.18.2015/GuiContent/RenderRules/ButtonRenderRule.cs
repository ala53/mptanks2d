using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ruminate.GUI.Framework;

namespace Ruminate.GUI.Content {

    public class ButtonRenderRule : RenderRule {

        private Text _textRenderer;
        private SlidingDoorRenderer _default, _hover, _pressed;

        private Rectangle _area;
        public override Rectangle Area {
            get { return _area; }
            set { _area = value; }
        }

        public enum RenderMode { Default, Hover, Pressed }
        public RenderMode Mode { get; set; }

        public string Label { get; set; }
        public SpriteFont Font { get { return _textRenderer.SpriteFont; } }

        public int Height { get { return _default.Across; } }
        public int Edge { get { return _default.Edge; } }

        public ButtonRenderRule(string skin = null) {
            Skin = skin;
        }

        public override void SetSize(int w, int h) {
            _area.Width = w;
            _area.Height = h;
        }

        protected override void LoadRenderers() {

            _default = LoadRenderer<SlidingDoorRenderer>(Skin, "button");
            _hover = LoadRenderer<SlidingDoorRenderer>(Skin, "button_hover");
            _pressed = LoadRenderer<SlidingDoorRenderer>(Skin, "button_pushed");

            _textRenderer = RenderManager.Texts[Text];
        }

        public override void Draw() {

            switch (Mode) {
                case RenderMode.Default: {
                    _default.Render(RenderManager.SpriteBatch, Area);
                    break;
                } case RenderMode.Hover: {
                    _hover.Render(RenderManager.SpriteBatch, Area);
                    break;
                } case RenderMode.Pressed: {
                    _pressed.Render(RenderManager.SpriteBatch, Area);
                    break;
                }
            }

            _textRenderer.Render(
                RenderManager.SpriteBatch,
                Label,
                Area,
                TextHorizontal.CenterAligned,
                TextVertical.CenterAligned);
        }

        public override void DrawNoClipping() { }
    }
}