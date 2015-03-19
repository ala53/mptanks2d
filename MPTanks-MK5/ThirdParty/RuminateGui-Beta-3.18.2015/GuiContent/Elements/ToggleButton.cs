using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Ruminate.GUI.Framework;

namespace Ruminate.GUI.Content {

    public sealed class ToggleButton : WidgetBase<ButtonRenderRule> {

        /*####################################################################*/
        /*                               Variables                            */
        /*####################################################################*/

        private bool _innerIsToggled;

        /// <summary>
        /// Label displaced on the button.
        /// </summary>
        public string Label {
            get { return RenderRule.Label; }
            set { RenderRule.Label = value; }
        }

        /// <summary>
        /// The gap between the text and the edge of the button.
        /// </summary>
        private int _textPadding;
        public int TextPadding {
            get {
                return _textPadding;
            }
            set {
                _textPadding = value;
                _width = 0;
            }
        }

        /// <summary>
        /// The width of the button.
        /// </summary>
        private int _width;
        public int Width {
            get {
                return _width;
            }
            set {
                _width = value;
                _textPadding = 0;
            }
        }

        /// <summary>
        /// Even fired when the button is pressed.
        /// </summary>
        public WidgetEvent OnToggle { get; set; }

        /// <summary>
        /// Even fired when the button is released.
        /// </summary>
        public WidgetEvent OffToggle { get; set; }
        
        /// <summary>
        /// Returns true when the button is pressed and false when 
        /// released. Changing the value fires the OnToggle/OffToggle
        /// events.
        /// </summary>
        public bool IsToggled {
            get { return _innerIsToggled; }
            set {
                if (value != _innerIsToggled) {
                    if (value) {
                        if (OnToggle != null) {
                            OnToggle(this);
                            RenderRule.Mode = ButtonRenderRule.RenderMode.Pressed;
                        }
                    } else {
                        if (OffToggle != null) {
                            OffToggle(this);
                            RenderRule.Mode = ButtonRenderRule.RenderMode.Default;
                        }
                    }
                }

                _innerIsToggled = value;
            }
        }

        /*####################################################################*/
        /*                        Node Initialization                         */
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
        public ToggleButton(int x, int y, string label, int padding = 2) {

            Area = new Rectangle(x, y, 0, 0);

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
        public ToggleButton(int x, int y, int width, string label) {
            
            Area = new Rectangle(x, y, 0, 0);

            Width = width;
            Label = label;
        }

        protected override void Attach() {

            var minWidth = (int)RenderRule.Font.MeasureString(Label).X + (2 * RenderRule.Edge);

            if (TextPadding > 0) {
                Area = new Rectangle(Area.X, Area.Y, minWidth + (TextPadding * 2), RenderRule.Height);
            } else if (Width > 0) {
                Area = new Rectangle(Area.X, Area.Y, (minWidth > Width) ? minWidth : Width, RenderRule.Height);
            }
        }

        protected internal override void Update() { }

        /*####################################################################*/
        /*                                Events                              */
        /*####################################################################*/

        protected internal override void MouseClick(MouseEventArgs e) {
            IsToggled = !IsToggled;
            if (!IsToggled) {
                RenderRule.Mode = IsHover 
                    ? ButtonRenderRule.RenderMode.Hover 
                    : ButtonRenderRule.RenderMode.Default;
            } else {
                RenderRule.Mode = ButtonRenderRule.RenderMode.Pressed;
            }
        }

        protected internal override void EnterPressed() {
            RenderRule.Mode = ButtonRenderRule.RenderMode.Pressed;
        }

        protected internal override void ExitPressed() {
            if (!IsToggled) {
                RenderRule.Mode = ButtonRenderRule.RenderMode.Default;
            }
        }

        protected internal override void EnterHover() {
            if (!IsToggled) {
                RenderRule.Mode = ButtonRenderRule.RenderMode.Hover;
            }
        }

        protected internal override void ExitHover() {
            if (!IsToggled) {
                RenderRule.Mode = ButtonRenderRule.RenderMode.Default;
            }
        }
    }
}