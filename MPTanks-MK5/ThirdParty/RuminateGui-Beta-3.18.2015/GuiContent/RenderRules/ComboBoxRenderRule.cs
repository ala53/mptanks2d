using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ruminate.GUI.Framework;

namespace Ruminate.GUI.Content {    

    public class ComboBoxRenderRule : RenderRule {

        private CardinalDirection _direction;
        public CardinalDirection OpenDirection {
            get {
                return _direction;
            } set {
                _direction = value;
                if (RenderManager != null) { LoadRenderers(); }
            }
        }

        public List<Tuple<Texture2D, string>> Items { get; set; }

        public bool Down { get; set; }

        public int HighlightItem { get; set; }

        private Text _textRenderer;
        private SlidingDoorRenderer _default, _hover, _pressed, _dropDownDark, _dropDownLight, _dropDownHover;
        private IconRenderer _dropDownRender;

        private Rectangle _area, _buttonArea, _textArea, _temp, _text, _dropDownIconArea;
        public override Rectangle Area {
            get {                                
                return _area;
            } set {                
                _textArea = value;
                _textArea.X += _default.Edge;

                _buttonArea = value;                
                _buttonArea.Width += _dropDownRender.Width + Edge;

                _dropDownIconArea.X = _buttonArea.Right - Edge - _dropDownRender.Width;
                _dropDownIconArea.Y = _buttonArea.Top + (_buttonArea.Height - _dropDownRender.Height) / 2;
                _dropDownIconArea.Width = _dropDownRender.Width;
                _dropDownIconArea.Height = _dropDownRender.Height;                

                _temp = _buttonArea;
                _temp.Height = _dropDownDark.Across;

                _text = _textArea;
                _text.Height = _dropDownDark.Across;

                _area = value;
                _area.Width += _dropDownIconArea.Width + _default.Edge;
            }
        }

        public override Rectangle SafeArea {
            get {
                if (Down) {
                    var temp = _area;
                    temp.Height = (_buttonArea.Height - 6) + (_default.Across - 4) * Items.Count;

                    if (_direction == CardinalDirection.North) {
                        temp.Y -= temp.Height;
                    }
                    return temp;
                }
                return _area;
            }
        }

        public override Rectangle ClippingArea {
            get {
                var temp = SafeArea;

                switch (_direction) {
                    case CardinalDirection.North: {
                            temp.Y -= (_buttonArea.Height - 6) + (_default.Across - 4) * Items.Count - _default.Across / 2 + 4 - _default.Across / 2;
                            temp.Height = (_buttonArea.Height - 6) + (_default.Across - 4) * Items.Count - _default.Across / 2 + 4;
                        break;
                    } case CardinalDirection.South: {
                        temp.Y += _default.Across / 2;
                        temp.Height = (_buttonArea.Height - 6) + (_default.Across - 4) * Items.Count - _default.Across / 2 + 4;
                        break;
                    }
                }

                return temp;
            }
        }

        public enum RenderMode { Default, Hover, Pressed }
        public RenderMode Mode { get; set; }

        public string Label { get; set; }
        public SpriteFont Font { get { return _textRenderer.SpriteFont; } }

        public int Height { get { return _default.Across; } }
        public int Edge { get { return _default.Edge; } }

        public ComboBoxRenderRule(string skin = null, CardinalDirection direction = CardinalDirection.South) {

            Items = new List<Tuple<Texture2D, string>>();
            HighlightItem = 0;

            OpenDirection = direction;

            Down = false;
            Skin = skin;
        }

        public override void SetSize(int w, int h) {
            _buttonArea.Width = w; 
            _buttonArea.Height = h;
        }

        protected override void LoadRenderers() {

            _default = LoadRenderer<SlidingDoorRenderer>(Skin, "button");
            _hover = LoadRenderer<SlidingDoorRenderer>(Skin, "button_hover");
            _pressed = LoadRenderer<SlidingDoorRenderer>(Skin, "button_pushed");

            switch (OpenDirection) {
                case CardinalDirection.North: {
                    _dropDownDark = LoadRenderer<SlidingDoorRenderer>(Skin, "dark_drop_down_inverted");
                    _dropDownLight = LoadRenderer<SlidingDoorRenderer>(Skin, "light_drop_down_inverted");
                    _dropDownHover = LoadRenderer<SlidingDoorRenderer>(Skin, "hover_drop_down_inverted");
                    break;
                } case CardinalDirection.South: {
                    _dropDownDark = LoadRenderer<SlidingDoorRenderer>(Skin, "dark_drop_down");
                    _dropDownLight = LoadRenderer<SlidingDoorRenderer>(Skin, "light_drop_down");
                    _dropDownHover = LoadRenderer<SlidingDoorRenderer>(Skin, "hover_drop_down");
                    break;
                }
            }            

            _dropDownRender = LoadRenderer<IconRenderer>(Skin, "drop_down");

            _textRenderer = RenderManager.Texts[RenderManager.DefaultText];
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
                _textArea,
                TextHorizontal.LeftAligned,
                TextVertical.CenterAligned);

            _dropDownRender.Render(RenderManager.SpriteBatch, _dropDownIconArea);
        }

        public override void DrawNoClipping() {
            
            if (!Down) { return; }

            switch (OpenDirection) {
                case CardinalDirection.North: {
                    _temp.Location = Area.Location;
                    _temp.Y -= _dropDownDark.Across - 6;

                    _text.Location = _textArea.Location;
                    _text.Y -= _dropDownDark.Across - 6;
                    break;
                } case CardinalDirection.South: {
                    _temp.Location = Area.Location;
                    _temp.Y += _default.Across - 6;

                    _text.Location = _textArea.Location;
                    _text.Y += _default.Across - 6;
                    break;
                }
            }

            var count = 0;
            var t = false;
            foreach (var item in Items) {
                if (count == HighlightItem) {
                    _dropDownHover.Render(RenderManager.SpriteBatch, _temp);
                } else if (t) {
                    _dropDownDark.Render(RenderManager.SpriteBatch, _temp);
                } else {
                    _dropDownLight.Render(RenderManager.SpriteBatch, _temp);
                }

                _textRenderer.Render(RenderManager.SpriteBatch, item.Item2, _text, TextHorizontal.LeftAligned, TextVertical.CenterAligned);

                switch (OpenDirection) {
                    case CardinalDirection.North:{
                        _temp.Y -= _default.Across - 4;
                        _text.Y -= _default.Across - 4;
                        t = !t;
                        break;
                    } case CardinalDirection.South: {
                        _temp.Y += _default.Across - 4;
                        _text.Y += _default.Across - 4;
                        t = !t;
                        break;
                    }
                }

                count++;
            }
        }

        public int? GetItem(Point clickLocation) {
            
            if (!Down) { return null; }

            switch (OpenDirection) {
                case CardinalDirection.North: {
                    _temp.Location = Area.Location;
                    _temp.Y -= _dropDownDark.Across - 6;
                    break;
                } case CardinalDirection.South: {
                    _temp.Location = Area.Location;
                    _temp.Y += _default.Across - 6;
                    break;
                }
            }

            for (var i = 0; i < Items.Count; i++) {
                if (_temp.Contains(clickLocation)) {
                    return i;
                }

                switch (OpenDirection) {
                    case CardinalDirection.North: {
                        _temp.Y -= _default.Across - 4;
                        break;
                    }
                    case CardinalDirection.South: {
                        _temp.Y += _default.Across - 4;
                        break;
                    }
                }                
            }

            return null;
        }
    }
}