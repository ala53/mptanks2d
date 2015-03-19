using Microsoft.Xna.Framework;
using Ruminate.GUI.Framework;

namespace Ruminate.GUI.Content {

    public class RadioButtonRenderRule : ElevatorRenderRule {       

        public RadioButtonRenderRule() : base("radio_button") { }
    }

    public class CheckBoxRenderRule : ElevatorRenderRule {        

        public CheckBoxRenderRule() : base("checkbox") { }
    }

    public abstract class ElevatorRenderRule : FontRenderRule {        

        private Rectangle _area, _icon;
        public override Rectangle Area {
            get {
                return _area;
            } set {
                _area = value;
                _icon.X = _area.X;
                _icon.Y = _area.Y;                
            }
        }

        public bool Checked { get; set; }

        public Point IconSize { get { return _default.Size; } }

        public enum RenderMode { Default, Hover, Pressed }
        public RenderMode Mode { get; set; }

        public string Label { get; set; }

        private IconRenderer _default, _hover, _checked, _hoverChecked, _pressed, _pressedChecked;
        private readonly string _type;        

        protected ElevatorRenderRule(string type, string skin = null) {

            Skin = skin;
            _type = type;
            _icon = Rectangle.Empty;
        }

        public override void SetSize(int w, int h) {
            _area.Width = w; _area.Height = h;
        }

        protected override void LoadRenderers() {
                       
            _default = LoadRenderer<IconRenderer>(Skin, _type);
            _checked = LoadRenderer<IconRenderer>(Skin, _type + "_checked");

            _hover = LoadRenderer<IconRenderer>(Skin, _type + "_hover");            
            _hoverChecked = LoadRenderer<IconRenderer>(Skin, _type + "_hover_checked");

            _pressed = LoadRenderer<IconRenderer>(Skin, _type + "_pressed");
            _pressedChecked = LoadRenderer<IconRenderer>(Skin, _type + "_pressed_checked");

            _icon.Width = _default.Width;
            _icon.Height = _default.Height;
        }

        public override void Draw() {

            switch (Mode) {
                case RenderMode.Default: {
                    if (Checked) {
                        _checked.Render(RenderManager.SpriteBatch, _icon);
                    } else {
                        _default.Render(RenderManager.SpriteBatch, _icon);
                    }
                    break;
                } case RenderMode.Hover: {
                    if (Checked) {
                        _hoverChecked.Render(RenderManager.SpriteBatch, _icon);                        
                    } else {
                        _hover.Render(RenderManager.SpriteBatch, _icon);
                    }
                    break;
                } case RenderMode.Pressed: {
                    if (Checked) {
                        _pressedChecked.Render(RenderManager.SpriteBatch, _icon);
                    } else {
                        _pressed.Render(RenderManager.SpriteBatch, _icon);
                    }
                    break;
                }
            }

            TextRenderer.Render(
                RenderManager.SpriteBatch,
                Label,
                Area,
                TextHorizontal.RightAligned,
                TextVertical.CenterAligned);
        }

        public override void DrawNoClipping() { }
    }
}