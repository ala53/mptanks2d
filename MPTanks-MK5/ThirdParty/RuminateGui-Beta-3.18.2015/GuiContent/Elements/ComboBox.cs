using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ruminate.GUI.Framework;

namespace Ruminate.GUI.Content {

    public sealed class ComboBox : WidgetBase<ComboBoxRenderRule> {

        /*####################################################################*/
        /*                               Variables                            */
        /*####################################################################*/

        private int? _selectedItem;
        private List<DropDownItem> _dropDownItems = new List<DropDownItem>();

        /// <summary>
        /// Get the item currently selected. If no item has been selected return null.
        /// </summary>
        public DropDownItem SelectedItem { 
            get { return _selectedItem.HasValue ? _dropDownItems[_selectedItem.Value] : null; }
        }

        /// <summary>
        /// Even fired when ever the selection is changed.
        /// </summary>
        public WidgetEvent OnSelectionChanged { get; set; }

        /// <summary>
        /// Returns true when the button is pressed and false when 
        /// released.
        /// </summary>
        public bool IsToggled {
            get {
                return RenderRule.Down;                
            } private set {
                BlocksInput = value;
                RenderRule.Down = value;
            }
        }   

        /// <summary>
        /// Return the Drop Down Box's 
        /// </summary>        
        public IList<DropDownItem> DropDownItems {
            get { return _dropDownItems.AsReadOnly(); }
        }

        /// <summary>
        /// Set the Drop Down Box items for the DropDown box.
        /// </summary>
        /// <param name="dropDownItems"></param>
        public void SetDropDownItems(List<DropDownItem> dropDownItems) {
            _dropDownItems = dropDownItems;

            RenderRule.Items.Clear();
            foreach (var dropDownItem in dropDownItems) {
                RenderRule.Items.Add(dropDownItem.Item);
            }
        }

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
        private int? _textPadding;
        public int? TextPadding {
            get {
                return _textPadding;
            } set {
                _textPadding = value;
                _width = null;
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
            }
        }

        /*####################################################################*/
        /*                           Initialization                           */
        /*####################################################################*/

        protected override ComboBoxRenderRule BuildRenderRule() {
            return new ComboBoxRenderRule();
        }

        public ComboBox(int x, int y, string label, int padding = 2, CardinalDirection direction = CardinalDirection.South, List<DropDownItem> dropDownItems = null) {

            Area = new Rectangle(x, y, 0, 0);
            Label = label;
            TextPadding = padding;

            RenderRule.OpenDirection = direction;

            if (dropDownItems != null) {
                SetDropDownItems(dropDownItems);
            }
        }

        public ComboBox(int x, int y, int width, string label, CardinalDirection direction = CardinalDirection.South, List<DropDownItem> dropDownItems = null) {

            Area = new Rectangle(x, y, 0, 0);
            Width = width;
            Label = label;

            RenderRule.OpenDirection = direction;

            if (dropDownItems != null) {
                SetDropDownItems(dropDownItems);
            }
        }        

        protected override void Attach() {

            var minWidth = (int)RenderRule.Font.MeasureString(Label).X + (2 * RenderRule.Edge);

            if (TextPadding.HasValue) {
                Area = new Rectangle(Area.X, Area.Y, minWidth + (TextPadding.Value * 2), RenderRule.Height);
            } else if (Width.HasValue) {
                Area = new Rectangle(Area.X, Area.Y, (minWidth > Width.Value) ? minWidth : Width.Value, RenderRule.Height);
            }
        }

        protected internal override void Update() { }

        /*####################################################################*/
        /*                                Events                              */
        /*####################################################################*/

        protected internal override void MouseClick(MouseEventArgs e) {

            if (IsToggled) {
                _selectedItem = RenderRule.GetItem(e.Location);
                if (_selectedItem.HasValue) {
                    if (_dropDownItems[_selectedItem.Value].OnClick != null) {
                        _dropDownItems[_selectedItem.Value].OnClick(this);
                    }
                    Label = _dropDownItems[_selectedItem.Value].Item.Item2;
                    if (OnSelectionChanged != null) {
                        OnSelectionChanged(this);
                    }
                }
            }

            IsToggled = !IsToggled;

            if (!IsToggled) {
                RenderRule.Mode = IsHover
                    ? ComboBoxRenderRule.RenderMode.Hover
                    : ComboBoxRenderRule.RenderMode.Default;
            } else {
                RenderRule.Mode = ComboBoxRenderRule.RenderMode.Pressed;
            }
        }

        protected internal override void EnterPressed() {
            RenderRule.Mode = ComboBoxRenderRule.RenderMode.Pressed;
        }

        protected internal override void ExitPressed() {
            if (!IsToggled) {
                RenderRule.Mode = ComboBoxRenderRule.RenderMode.Default;
            }
        }

        protected internal override void EnterHover() {
            if (!IsToggled) {
                RenderRule.Mode = ComboBoxRenderRule.RenderMode.Hover;
            }
        }

        protected internal override void ExitHover() {            
            if (!IsToggled) {
                RenderRule.Mode = ComboBoxRenderRule.RenderMode.Default;
            }
        }

        protected internal override void ExitFocus() {
            IsToggled = false;
            RenderRule.Mode = ComboBoxRenderRule.RenderMode.Default;
        }

        protected internal override void MouseMove(MouseEventArgs e) {
            
            RenderRule.HighlightItem = -1;

            if (!IsToggled) { return; }

            var number = RenderRule.GetItem(e.Location);
            if (number.HasValue) {
                RenderRule.HighlightItem = number.Value;
            }
        }

        // TODO: Texture2D is for icons. Not yet supported.

        public class DropDownItem {
            public Tuple<Texture2D, string> Item { get; set; }
            public WidgetEvent OnClick { get; set; }

            public DropDownItem(string label, Texture2D icon = null, WidgetEvent clickEvent = null) {
                Item = new Tuple<Texture2D, string>(icon, label);
                OnClick = clickEvent;
            }
        }
    }
}