using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Ruminate.GUI.Framework {

    public abstract class RenderRule {

        public bool Loaded { get; set; }

        internal protected RenderManager RenderManager { get; set; }

        public abstract void SetSize(int w, int h);

        public abstract Rectangle Area { get; set; }
        public virtual Rectangle SafeArea { get { return Area; } }
        public virtual Rectangle ClippingArea { get { return RenderManager.GraphicsDevice.Viewport.Bounds; } }

        protected string DefaultSkin {
            get {
                return RenderManager.DefaultSkin;
            }
        }        

        private string _skin;
        internal protected string Skin {
            get {
                return _skin;
            } set {
                if (value == null) { return; }
                _skin = value;
                if (RenderManager != null) { Load(); }
            }
        }

        protected string DefaultText {
            get {
                return RenderManager.DefaultText;
            }
        }

        private string _text;
        internal protected string Text {
            get {
                return _text;
            } set {
                if (value == null) { return; }
                _text = value;
                if (RenderManager != null) { Load(); }
            }
        }

        protected RenderRule() {
            Loaded = false;
        }

        protected T LoadRenderer<T>(string skin, string widget) where T : Renderer {
            return (T)RenderManager.Skins[skin].WidgetMap[widget];
        }

        protected abstract void LoadRenderers();
        public virtual void Load() {
            if (Skin == null) { Skin = DefaultSkin; }
            if (Text == null) { Text = DefaultText; }
            LoadRenderers();
            Loaded = true;
        
        }

        public abstract void Draw();

        public abstract void DrawNoClipping();
    }
}
