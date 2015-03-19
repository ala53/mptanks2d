using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Ruminate.GUI.Framework;

namespace Ruminate.GUI.Content {

    public sealed class Button : WidgetBase<ButtonRenderRule> {

        /*####################################################################*/
        /*                               Variables                            */
        /*####################################################################*/

        /// <summary>
        /// Label displaced on the button.
        /// </summary>
        public string Label {
            get {
                return RenderRule.Label;
            } set {
                RenderRule.Label = value;

                if (RenderRule.Loaded) { Resize(); }
            }
        }

        /// <summary>
        /// The gap between the text and the edge of the button.
        /// </summary>
        private int? _textPadding;
        public int? TextPadding {
            get {
                return _textPadding;
            } set {
                _textPadding = value;
                _width = null;

                if (RenderRule.Loaded) { Resize(); }
            }
        }

        /// <summary>
        /// The width of the button.
        /// </summary>
        private int? _width;
        public int? Width {
            get {
                return _width;
            } set {
                _width = value;
                _textPadding = null;

                if (RenderRule.Loaded) { Resize(); }
            }
        }

        /// <summary>
        /// Event fired when the widget is clicked.
        /// </summary>
        public WidgetEvent ClickEvent { get; set; }

        /*####################################################################*/
        /*                           Initialization                           */
        /*####################################################################*/

        protected override ButtonRenderRule BuildRenderRule() {
            return new ButtonRenderRule();
        }

        /// <summary>
        /// Creates a new button at the location specified. The button defaults to
        /// the height of the RenderRule and width of the label.
        /// </summary>
        /// <param name="x">The X coordinate of the widget.</param>
        /// <param name="y">The Y coordinate of the widget.</param>
        /// <param name="label">The label to be rendered on the button.</param>
        /// <param name="padding">If specified the padding on either side of the label.</param>
        /// <param name="buttonEvent">Event fired when the button is clicked.</param>
        public Button(int x, int y, string label, int padding = 2, WidgetEvent buttonEvent = null) {

            Area = new Rectangle(x, y, 0, 0);

            ClickEvent = buttonEvent;
            Label = label;

            TextPadding = padding;
        }

        /// <summary>
        /// Creates a new button at the location specified. The button defaults to
        /// the height of the RenderRule and width of the label.
        /// </summary>
        /// <param name="x">The X coordinate of the widget.</param>
        /// <param name="y">The Y coordinate of the widget.</param>
        /// <param name="width">The width of the Button. Ignored if the width is less that the width of the label.</param>
        /// <param name="label">The label to be rendered on the button.</param>
        /// <param name="buttonEvent">Event fired when the button is clicked.</param>
        public Button(int x, int y, int width, string label, WidgetEvent buttonEvent = null) {

            Area = new Rectangle(x, y, 0, 0);
            Width = width;
            ClickEvent = buttonEvent;
            Label = label;
        }        

        protected override void Attach() { }

        override protected void Resize() {

            var minWidth = (int)RenderRule.Font.MeasureString(Label).X + (2 * RenderRule.Edge);

            if (TextPadding.HasValue) {
                Area = new Rectangle(Area.X, Area.Y, minWidth + (TextPadding.Value * 2), RenderRule.Height);
            } else if (Width.HasValue) {
                Area = new Rectangle(Area.X, Area.Y, (minWidth > Width.Value) ? minWidth : Width.Value, RenderRule.Height);
            }

            base.Resize();
        }

        override protected internal void Update() { }

        /*####################################################################*/
        /*                                Events                              */
        /*####################################################################*/

        protected internal override void MouseClick(MouseEventArgs e) {
            if (ClickEvent != null) {
                ClickEvent(this);
            }
        }

        protected internal override void EnterPressed() {
            RenderRule.Mode = ButtonRenderRule.RenderMode.Pressed;
        }

        protected internal override void ExitPressed() {
            RenderRule.Mode = IsHover 
                ? ButtonRenderRule.RenderMode.Hover 
                : ButtonRenderRule.RenderMode.Default;
        }

        protected internal override void EnterHover() {
            if (RenderRule.Mode != ButtonRenderRule.RenderMode.Pressed) {
                RenderRule.Mode = ButtonRenderRule.RenderMode.Hover;
            }
        }

        protected internal override void ExitHover() {
            if (RenderRule.Mode != ButtonRenderRule.RenderMode.Pressed) {
                RenderRule.Mode = ButtonRenderRule.RenderMode.Default;
            }
        }
    }
}