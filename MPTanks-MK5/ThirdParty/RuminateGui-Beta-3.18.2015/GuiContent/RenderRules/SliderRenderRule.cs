using Microsoft.Xna.Framework;
using Ruminate.GUI.Framework;

namespace Ruminate.GUI.Content {

    public class SliderRenderRule : RenderRule {

        private IconRenderer _grip;
        private SlidingDoorRenderer _slider, _selectedSlider;

        private int _travel;
        public int Travel {
            get { 
                return _travel;
            } set {
                _travel = (int)MathHelper.Clamp(value, 0, _sliderArea.Width);
                _percentage = (float)_travel / _sliderArea.Width;
            }
        }

        private float _percentage;
        public float Percentage {
            get {
                return _percentage;
            } set {
                _percentage = MathHelper.Clamp(value, 0, 1);
                _travel = (int)(_percentage * _sliderArea.Width);
            }
        }

        public Point IconSize {
            get { return _grip.Size; }
        }

        public int SliderHeight {
            get { return _slider.Across; }
        }

        private Rectangle _area, _sliderArea, _selectedSliderArea;
        public override Rectangle Area {
            get {
                return _area;
            } set {
                _area = value;
                _sliderArea = new Rectangle(
                    _area.X + (_grip.Width / 2),
                    _area.Y + (_area.Height / 2) - (_slider.Across / 2),
                    _area.Width - _grip.Width,
                    _slider.Across);
                _selectedSliderArea = _sliderArea;
                _gripArea = new Rectangle(
                    _area.X,
                    _area.Y + (_area.Height / 2) - (_grip.Height / 2),
                    _grip.Width,
                    _grip.Height);
            }
        }

        private Rectangle _gripArea;
        public Rectangle GripArea {
            get {
                return _gripArea;
            } 
        }

        public SliderRenderRule(string skin = null) {
            Skin = skin;
        }

        public override void SetSize(int w, int h) {
            _area.Width = w;
            _area.Height = h;
        }

        protected override void LoadRenderers() {

            _slider = LoadRenderer<SlidingDoorRenderer>(Skin, "scrollbar_holder");
            _selectedSlider = LoadRenderer<SlidingDoorRenderer>(Skin, "scrollbar_bar");
            _grip = LoadRenderer<IconRenderer>(Skin, "grip");

        }

        public override void Draw() {

            _selectedSliderArea.Width = _travel;

            _slider.Render(RenderManager.SpriteBatch, _sliderArea);
            _selectedSlider.Render(RenderManager.SpriteBatch, _selectedSliderArea);
            _gripArea.X = Area.X + Travel;
            _grip.Render(RenderManager.SpriteBatch, GripArea);
        }

        public override void DrawNoClipping() { }
    }
}