using Microsoft.Xna.Framework.Graphics;

namespace Ruminate.GUI.Framework {

    public abstract class FontRenderRule : RenderRule {

        protected Text TextRenderer { get; set; }
        public SpriteFont Font { get { return TextRenderer.SpriteFont; } }

        public override void Load() {
            if (Skin == null) { Skin = DefaultSkin; }
            if (Text == null) { Text = DefaultText; }
            TextRenderer = RenderManager.Texts[Text];
            LoadRenderers();
            Loaded = true;
        } 
    }
}
