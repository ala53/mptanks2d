using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Ruminate.GUI.Framework;
using Ruminate.Utils;

namespace Ruminate.GUI.Content {

    public sealed class CheckBox : WidgetBase<CheckBoxRenderRule> {

        /*####################################################################*/
        /*                               Variables                            */
        /*####################################################################*/

        public WidgetEvent OnToggle { get; set; }
        public WidgetEvent OffToggle { get; set; }

        private Point _location;
        public Point Location {
            get { return _location; } 
            set { _location = value; }
        }

        public string Label {
            get { return RenderRule.Label; }
            set { RenderRule.Label = value; }
        }

        private bool _innerIsToggled;
        public bool IsToggled {
            get { return _innerIsToggled; }
            set {
                if (value != _innerIsToggled) 
                {
                    if (value) { if (OnToggle != null) { OnToggle(this); } } 
                    else { if (OffToggle != null) { OffToggle(this); } }
                }

                _innerIsToggled = value;
                RenderRule.Checked = _innerIsToggled;
            }
        }

        /// <summary>
        /// Sets whether or not the CheckBox is toggled without firing the 
        /// OnToggle or OffToggle events.
        /// </summary>
        /// <param name="toggle">Whether or not the CheckBox is checked.</param>
        public void SetToggle(bool toggle) 
        {
            _innerIsToggled = toggle;
            RenderRule.Checked = _innerIsToggled;
        }

        /*####################################################################*/
        /*                           Initialization                           */
        /*####################################################################*/

        protected override CheckBoxRenderRule BuildRenderRule() 
        {
            return new CheckBoxRenderRule();
        }

        public CheckBox(int x, int y, string label) 
        {
            Area = new Rectangle(x, y, 0, 0);

            Label = label;
            _innerIsToggled = false;
        }        

        protected override void Attach() 
        {
            var size = RenderRule.Font.MeasureString(Label).ToPoint();
            var width = size.X + 2 + RenderRule.IconSize.X;
            var height = RenderRule.IconSize.Y;

            Area = new Rectangle(Area.X, Area.Y, width, height);
        }

        protected internal override void Update() { }

        /*####################################################################*/
        /*                                Events                              */
        /*####################################################################*/

        protected internal override void MouseClick(MouseEventArgs e)
        {
            IsToggled = !IsToggled;
        }

        protected internal override void EnterPressed()
        {
            RenderRule.Mode = ElevatorRenderRule.RenderMode.Pressed;
        }

        protected internal override void ExitPressed()
        {
            RenderRule.Mode = IsHover
                ? ElevatorRenderRule.RenderMode.Hover
                : ElevatorRenderRule.RenderMode.Default;
        }

        protected internal override void EnterHover()
        {
            if (RenderRule.Mode != ElevatorRenderRule.RenderMode.Pressed) {
                RenderRule.Mode = ElevatorRenderRule.RenderMode.Hover;
            }
        }

        protected internal override void ExitHover()
        {
            if (RenderRule.Mode != ElevatorRenderRule.RenderMode.Pressed) {
                RenderRule.Mode = ElevatorRenderRule.RenderMode.Default;
            }
        }
    }
}