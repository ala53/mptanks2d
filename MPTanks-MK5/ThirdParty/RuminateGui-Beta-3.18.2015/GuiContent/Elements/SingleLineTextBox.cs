using Ruminate.GUI.Framework;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Ruminate.GUI.Content {

    public sealed class SingleLineTextBox : WidgetBase<PanelRenderRule> {

        private readonly TextBox _innerTextBox;

        override public string Skin { set { _innerTextBox.RenderRule.Skin = value; } }
        override public string Text { set { _innerTextBox.RenderRule.Text = value; } }

        public string Value {
            get { return _innerTextBox.RenderRule.Value; }
            set { _innerTextBox.RenderRule.Value = value; }
        }

        public int MaxCharacters {
            get { return _innerTextBox.RenderRule.MaxLength; }
            set { _innerTextBox.RenderRule.RecreateStringData(value); }
        }

        /*####################################################################*/
        /*                           Initialization                           */
        /*####################################################################*/

        public SingleLineTextBox(int x, int y, int width, short chars) {

            AddWidget(_innerTextBox = new TextBox(2, chars));
            Area = new Rectangle(x, y, width, 0);
        }

        protected override PanelRenderRule BuildRenderRule() {
            return new PanelRenderRule();
        }

        protected override void Attach() {
            var key = _innerTextBox.RenderRule.Text ?? RenderRule.RenderManager.DefaultSkin;
            var height = RenderRule.RenderManager.Texts[key].SpriteFont.LineSpacing + (2 * RenderRule.BorderWidth);
            Area = new Rectangle(Area.X, Area.Y, Area.Width, height);
        }

        internal override void Layout() {
            _innerTextBox.AbsoluteArea = new Rectangle(AbsoluteInputArea.X, AbsoluteInputArea.Y, Area.Width, Area.Height);
            _innerTextBox.SissorArea = Rectangle.Intersect(_innerTextBox.AbsoluteArea, SissorArea);

            base.Layout();
        }

        protected internal override void Update() { }
    }
}