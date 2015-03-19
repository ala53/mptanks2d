using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ruminate.GUI.Framework;

namespace Ruminate.GUI.Content {

    public sealed class Label : WidgetBase<LabelRenderRule> {

        /*####################################################################*/
        /*                               Variables                            */
        /*####################################################################*/

        public int Padding { get; private set; }

        public string Value {
            get {
                return RenderRule.Label;
            } set {                
                RenderRule.Label = value;
                if (RenderRule.Loaded) {
                    Attach();
                }
            }
        }        

        public Texture2D Icon {
            get {
                return RenderRule.Image;
            } set {
                RenderRule.Image = value;
                if (RenderRule.Loaded) {
                    Attach();
                }
            }
        }

        /*####################################################################*/
        /*                           Initialization                           */
        /*####################################################################*/

        protected override LabelRenderRule BuildRenderRule() {
            return new LabelRenderRule();
        }

        public Label(int x, int y, string value) {

            Area = new Rectangle(x, y, 0, 0);
            Value = value;
        }

        public Label(int x, int y, Texture2D icon, string value, int padding = 0) {

            Area = new Rectangle(x, y, 0, 0);

            Icon = icon;
            Value = value;
            Padding = padding;
        }        

        private void Resize() {
            var size = RenderRule.Font.MeasureString(RenderRule.Label);
            if (RenderRule.Image != null) {
                Area = new Rectangle(Area.X, Area.Y,
                    (int)size.X + Padding + RenderRule.Image.Width,
                    (size.Y > RenderRule.Image.Height) ? (int)size.Y : RenderRule.Image.Height);
            } else {
                Area = new Rectangle(Area.X, Area.Y, (int)size.X, (int)size.Y);
            }

            RenderRule.SetSize(Area.Width, Area.Height);
        }

        protected override void Attach() { Resize(); }

        protected internal override void Update() { }
    }
}