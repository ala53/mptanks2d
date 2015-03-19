using Ruminate.GUI.Framework;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Ruminate.GUI.Content {

    public sealed class Panel : WidgetBase<PanelRenderRule> {

        /*####################################################################*/
        /*                           Initialization                           */
        /*####################################################################*/

        public Panel(int x, int y, int width, int height) {

            Area = new Rectangle(x, y, width, height);
        }        

        protected override PanelRenderRule BuildRenderRule() {
            return new PanelRenderRule();
        }

        protected override void Attach() { }

        internal override void Layout() {

            foreach (var widget in Children) {                
                widget.AbsoluteArea = new Rectangle(
                    widget.Area.X + AbsoluteInputArea.X,
                    widget.Area.Y + AbsoluteInputArea.Y,
                    widget.Area.Width,
                    widget.Area.Height);
                if (Parent != null) {
                    widget.SissorArea = Rectangle.Intersect(widget.AbsoluteArea, SissorArea);
                }
            }

            base.Layout();
        }

        protected internal override void Update() { }       
    }
}