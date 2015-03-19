using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Ruminate.GUI.Framework;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Ruminate.GUI.Content {

    public class ScrollBars : WidgetBase<ScrollBarsRenderRule> {

        float _yF, _xF;

        public int ScrollSpeed { get; set; }
        public int WheelSpeed { get; set; }

        private enum DragState { None, VBar, HBar, Up, Down, Left, Right }
        
        private DragState State { get; set; }
        private Pin Pin { get; set; }

        /*####################################################################*/
        /*                           Initialization                           */
        /*####################################################################*/

        protected override ScrollBarsRenderRule BuildRenderRule() {
            return new ScrollBarsRenderRule();
        }

        public ScrollBars() {

            Pin = new Pin();
            State = DragState.None;
        }

        protected override void Attach() {

            if (Parent != null) {
                var a = Parent.AbsoluteInputArea;
                Area = new Rectangle(0, 0, a.Width, a.Height);
            } else {
                Area = Owner.ScreenBounds;
            }

            ScrollSpeed = Owner.DefaultScrollSpeed;
            WheelSpeed = Owner.DefaultWheelSpeed;
        }

        internal override void Layout() {

            // Pixel size of panel to contain child widgets
            var outerArea = BuildContainerRenderArea();
            RenderRule.BuildBars(outerArea);

            foreach (var widget in Children) {
                widget.AbsoluteArea = new Rectangle(
                    widget.Area.X + AbsoluteInputArea.X - (int)(RenderRule.Horizontal.ChildOffset),
                    widget.Area.Y + AbsoluteInputArea.Y - (int)(RenderRule.Vertical.ChildOffset),
                    widget.Area.Width,
                    widget.Area.Height);
                widget.SissorArea = AbsoluteInputArea;
                if (Parent != null) {
                    widget.SissorArea = Rectangle.Intersect(widget.SissorArea, SissorArea);
                }
            }

            base.Layout();
        }

        /*####################################################################*/
        /*                                Logic                               */
        /*####################################################################*/

        protected internal override void Update() {

            if (!IsHover) { return; }

            if (Owner.NewState.ScrollWheelValue != Owner.OldState.ScrollWheelValue) {
                RenderRule.Vertical.BarOffset -= ((Owner.NewState.ScrollWheelValue - Owner.OldState.ScrollWheelValue) / 120f) * WheelSpeed;
                RenderRule.Vertical.BarOffset = MathHelper.Clamp(RenderRule.Vertical.BarOffset, 0, RenderRule.Vertical.MaxBarOffset);
            } else {
                switch (State) {
                    case DragState.None:
                        return;
                    case DragState.VBar:
                        RenderRule.Vertical.BarOffset = _yF - Pin.Shift.Y;
                        break;
                    case DragState.HBar:
                        RenderRule.Horizontal.BarOffset = _xF - Pin.Shift.X;
                        break;
                    case DragState.Up:
                        RenderRule.Vertical.BarOffset += ScrollSpeed;
                        break;
                    case DragState.Down:
                        RenderRule.Vertical.BarOffset -= ScrollSpeed;
                        break;
                    case DragState.Right:
                        RenderRule.Horizontal.BarOffset += ScrollSpeed;
                        break;
                    case DragState.Left:
                        RenderRule.Horizontal.BarOffset -= ScrollSpeed;
                        break;
                }
            }

            GetTreeNode().DfsOperation(node=>node.Data.Layout());
        }

        /*####################################################################*/
        /*                               Helpers                              */
        /*####################################################################*/

        private Rectangle BuildContainerRenderArea() {

            if (Children == null || !Children.Any()) { return Rectangle.Empty; }

            var left = int.MaxValue;
            var right = int.MinValue;
            var top = int.MaxValue;
            var bottom = int.MinValue;

            foreach (var child in Children) {

                var childLeft = child.Area.X;
                var childRight = child.Area.X + child.Area.Width;
                var childTop = child.Area.Y;
                var childBottom = child.Area.Y + child.Area.Height;

                left = (left < childLeft) ? left : childLeft;
                right = (right > childRight) ? right : childRight;
                top = (top < childTop) ? top : childTop;
                bottom = (bottom > childBottom) ? bottom : childBottom;
            }

            return new Rectangle(0, 0, right + left, bottom + top);
        }

        /*####################################################################*/
        /*                                 Events                             */
        /*####################################################################*/

        protected internal override void MouseDoubleClick(MouseEventArgs e) {
            MouseDown(e);
        }

        protected internal override void MouseDown(MouseEventArgs e) {

            var mouse = e.Location;

            if (RenderRule.BarDirection == Direction.Vertical || RenderRule.BarDirection == Direction.Both) {
                if (RenderRule.Vertical.IncreaseArea.Contains(mouse)) {
                    State = DragState.Up;
                } else if (RenderRule.Vertical.DecreaseArea.Contains(mouse)) {
                    State = DragState.Down;
                } else if (RenderRule.Vertical.BarArea.Contains(mouse)) {
                    _yF = RenderRule.Vertical.BarOffset;
                    Pin.Push();
                    State = DragState.VBar;
                } 
            }

            if (RenderRule.BarDirection == Direction.Horizontal || RenderRule.BarDirection == Direction.Both) {
                if (RenderRule.Horizontal.IncreaseArea.Contains(mouse)) {
                    State = DragState.Right;
                } else if (RenderRule.Horizontal.DecreaseArea.Contains(mouse)) {
                    State = DragState.Left;
                } else if (RenderRule.Horizontal.BarArea.Contains(mouse)) {
                    _xF = RenderRule.Horizontal.BarOffset;
                    Pin.Push();
                    State = DragState.HBar;
                }
            }
        }

        protected internal override void MouseUp(MouseEventArgs e) {
            Pin.Pull();
            State = DragState.None;
        }
    }
}