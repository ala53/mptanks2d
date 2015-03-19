using Microsoft.Xna.Framework;

namespace Ruminate.GUI.Content {

    public sealed class VerticalScrollBarRenderRule : ScrollBarRenderRule {

        private float _barOffset;
        public override float BarOffset {
            get {
                return _barOffset;
            } set {
                _barOffset = MathHelper.Clamp(value, 0, MaxBarOffset);
                barArea.Y = holderArea.Top + Holder.Edge + (int)(_barOffset);
            }
        }

        private Rectangle _area;
        public override Rectangle Area {
            get {
                return _area;
            } set {
                _area = value;
                decreaseArea.X = _area.Right - Holder.Across; //Top button
                decreaseArea.Y = _area.Top;

                increaseArea.X = _area.Right - Holder.Across; //Bottom Button
                increaseArea.Y = _area.Bottom - Holder.Edge;

                holderArea.Width = increaseArea.Width = decreaseArea.Width = Holder.Across; //Width of everything except the bars
                increaseArea.Height = decreaseArea.Height = Holder.Edge; //Height of the buttons

                holderArea.X = _area.Right - Holder.Across; //Holder
                holderArea.Y = _area.Top;
                holderArea.Height = _area.Height;

                barArea.X = holderArea.Left; //Bar
                barArea.Y = holderArea.Top + Holder.Buffer + (int)BarOffset;
                barArea.Width = Bar.Across;

                if (Both) {                    
                    increaseArea.Y -= Holder.Across;
                    holderArea.Height -= Holder.Across;
                }
            }
        }

        public override void SetSize(int w, int h) {
            _area.Width = w;
            _area.Height = h;
        }

        protected override void LoadRenderers() {

            Bar = LoadRenderer<VerticalSlidingDoorRenderer>(Skin, "scrollbar_bar_vertical");
            Holder = LoadRenderer<VerticalSlidingDoorRenderer>(Skin, "scrollbar_holder_vertical");
        }

        public override void CalculateRatio(Rectangle childArea) {

            var holderSize = (holderArea.Height - (Holder.Buffer * 2));

            Ratio = (float)holderSize / (float)childArea.Height;
                        
            var barSize = (int)(holderSize * Ratio);
            MaxBarOffset = (int)(holderSize - barSize);

            if (Both) {
                Ratio = (float)MaxBarOffset / (childArea.Height - (Area.Height - Holder.Edge));
            } else {
                Ratio = (float)MaxBarOffset / (childArea.Height - Area.Height);
            }

            barArea.Height = barSize;
            Area = Area;
        }
    }
}