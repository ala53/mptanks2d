using Microsoft.Xna.Framework;
using Ruminate.GUI.Framework;

namespace Ruminate.GUI.Content {

    public class ScrollBarsRenderRule : RenderRule {        

        public override Rectangle Area {
            get { 
                return Horizontal.Area; 
            } set {
                Horizontal.Area = value;
                Vertical.Area = value;
                _safeArea = new Rectangle(
                    Area.X,
                    Area.Y,
                    (BarDirection == Direction.Both) ? Area.Width - Vertical.HolderArea.Width : Area.Width,
                    (BarDirection == Direction.Both) ? Area.Height - Horizontal.HolderArea.Height : Area.Height);
            }
        }

        private Rectangle _safeArea;
        public override Rectangle SafeArea {
            get {
                return _safeArea;
            }
        }

        public bool IsBuilt { get; set; }

        public Direction BarDirection { get; private set; }

        public HorizontalScrollBarRenderRule Horizontal {get; private set;}
        public VerticalScrollBarRenderRule Vertical { get; private set; }

        public ScrollBarsRenderRule() {

            IsBuilt = false;

            Horizontal = new HorizontalScrollBarRenderRule();
            Vertical = new VerticalScrollBarRenderRule();

            BarDirection = Direction.None;
        }

        public override void SetSize(int w, int h) {
            Horizontal.SetSize(w, h);
            Vertical.SetSize(w, h);
        }

        public void BuildBars(Rectangle childArea) {

            Vertical.Both = Horizontal.Both = false;

            if (childArea.Width > Area.Width && childArea.Height > Area.Height) {
                BarDirection = Direction.Both;
                Vertical.Both = Horizontal.Both = true;
            } else if (childArea.Width > Area.Width) {
                BarDirection = Direction.Horizontal;
            } else if (childArea.Height > Area.Height) {
                BarDirection = Direction.Vertical;
            }

            if (BarDirection == Direction.Horizontal || BarDirection == Direction.Both) {
                Horizontal.CalculateRatio(childArea);
            }
            if (BarDirection == Direction.Vertical || BarDirection == Direction.Both) {
                Vertical.CalculateRatio(childArea);
            }

            IsBuilt = true;
        }

        protected override void LoadRenderers() {

            Horizontal.SetRenderManager(RenderManager);
            Vertical.SetRenderManager(RenderManager);

            Horizontal.Load();
            Vertical.Load();
        }

        public override void Draw() {

            if (BarDirection == Direction.Horizontal || BarDirection == Direction.Both) {
                Horizontal.Draw();
            }
            if (BarDirection == Direction.Vertical || BarDirection == Direction.Both) {
                Vertical.Draw();
            }            
        }

        public override void DrawNoClipping() { }
    }
}