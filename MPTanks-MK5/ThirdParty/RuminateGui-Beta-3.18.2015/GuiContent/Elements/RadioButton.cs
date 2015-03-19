using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Ruminate.GUI.Framework;
using Ruminate.Utils;

namespace Ruminate.GUI.Content {

    public sealed class RadioButton : WidgetBase<RadioButtonRenderRule> {        

        /*####################################################################*/
        /*                               Grouping                             */
        /*####################################################################*/
        #region Grouping
        //TODO: Don't like using statics, I would rather have them in a container.
        private static readonly Dictionary<string, List<RadioButton>> Groups = 
            new Dictionary<string, List<RadioButton>>();

        private static void AddRadioButton(string group, RadioButton button) {

            if (!Groups.ContainsKey(group)) { Groups.Add(group, new List<RadioButton>()); }

            Groups[group].Add(button);
        }
        #endregion

        /*####################################################################*/
        /*                               Variables                            */
        /*####################################################################*/

        /* ## Local Variables ## */
        private bool _innerIsChecked;
        private readonly string _group;

        /* ## Exposed Variables ## */
        public WidgetEvent OnCheck { get; set; }
        public WidgetEvent OnUnCheck { get; set; }

        public string Label {
            get { return RenderRule.Label; }
            set { RenderRule.Label = value; }
        }
        
        public bool IsChecked {
            get {
                return _innerIsChecked;
            } private set {

                if (!value) { throw new ArgumentException("IsChecked can only be set to true"); }

                _innerIsChecked = true;
                RenderRule.Checked = _innerIsChecked;
                if (OnCheck != null) { OnCheck(this); }

                foreach (var radioButton in Groups[_group]) {
                    if (radioButton == this) { continue; }

                    radioButton._innerIsChecked = false;
                    radioButton.RenderRule.Checked = radioButton._innerIsChecked;
                    if (radioButton.OnUnCheck != null) {
                        radioButton.OnUnCheck(radioButton);                        
                    }

                    radioButton.RenderRule.Mode = ElevatorRenderRule.RenderMode.Default;
                }
            }
        }

        /*####################################################################*/
        /*                           Initialization                           */
        /*####################################################################*/

        public RadioButton(int x, int y, string group, string label) {

            Area = new Rectangle(x, y, 0, 0);

            Label = label;
            _innerIsChecked = false;

            _group = group;
            AddRadioButton(group, this);
        }

        protected override RadioButtonRenderRule BuildRenderRule() {                      
            return new RadioButtonRenderRule();
        }

        protected override void Attach() {

            var size = RenderRule.Font.MeasureString(Label).ToPoint();
            var width = size.X + 2 + RenderRule.IconSize.X;
            var height = RenderRule.IconSize.Y;

            Area = new Rectangle(Area.X, Area.Y, width, height);
        }

        protected internal override void Update() { }

        /*####################################################################*/
        /*                                Events                              */
        /*####################################################################*/

        protected internal override void MouseClick(MouseEventArgs e) {
            IsChecked = true;
        }

        protected internal override void EnterPressed() {
            RenderRule.Mode = ElevatorRenderRule.RenderMode.Pressed;
        }

        protected internal override void EnterHover() {
            if (RenderRule.Mode != ElevatorRenderRule.RenderMode.Pressed) {
                RenderRule.Mode = ElevatorRenderRule.RenderMode.Hover;
            }
        }

        protected internal override void ExitHover() {
            if (RenderRule.Mode != ElevatorRenderRule.RenderMode.Pressed) {
                RenderRule.Mode = ElevatorRenderRule.RenderMode.Default;
            }
        }
    }
}