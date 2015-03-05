using Engine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MPTanks_MK5.Rendering.Sprites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK5.Rendering
{
    /// <summary>
    /// An internal cache so that multiple gameobjects using the same art asset can run
    /// </summary>
    class AssetCache
    {
        //The TEXTURE-MISSING texture sprite sheet
        private Sprites.SpriteSheet blankSpriteSheet;

        private Game game;

        public AssetCache(Game _game)
        {
            game = _game;

            var texture = new Texture2D(game.GraphicsDevice, 4, 4);

            //Create a checkerboard pattern
            // |w|w|p|p|
            // |w|w|p|p|
            // |p|p|w|w|
            // |p|p|w|w|
            texture.SetData(new[] {
                Color.White, Color.White, Color.Purple, Color.Purple,
                Color.White, Color.White, Color.Purple, Color.Purple,
                Color.Purple, Color.Purple, Color.White, Color.White,
                Color.Purple, Color.Purple, Color.White, Color.White,
            });

            var sprites = new Dictionary<string, Rectangle>();
            sprites.Add("missing_texture", new Rectangle(0, 0, 4, 4));
            sprites.Add("1px_blank", new Rectangle(0, 0, 1, 1));
            blankSpriteSheet = new SpriteSheet("missing_texture_sheet", texture, sprites);
        }

        private Dictionary<string, Sprites.SpriteSheet> spriteSheets =
            new Dictionary<string, Sprites.SpriteSheet>();
        private Dictionary<string, Animation.Animation> animations =
            new Dictionary<string, Animation.Animation>();
        public Sprite GetArtAsset(RenderableComponent component, GameTime gameTime)
        {

            if (component.SpriteSheetName == null) //Untextured, return white box
                return blankSpriteSheet.Sprites["1px_blank"];

            //Special path for animations.
            if (component.AssetName.StartsWith("[animation]"))
                return GetAnimation(component, gameTime);

            if (!spriteSheets.ContainsKey(component.SpriteSheetName))
                LoadSpriteSheet(component.SpriteSheetName);

            if (spriteSheets.ContainsKey(component.SpriteSheetName))
                if (spriteSheets[component.SpriteSheetName].Sprites.ContainsKey(component.AssetName))
                    return spriteSheets[component.SpriteSheetName].Sprites[component.AssetName];

            Logger.Error("Missing texture: " +
                component.SpriteSheetName + "/" + component.AssetName);
            //Texture missing, return MISSING_Texture texture
            return blankSpriteSheet.Sprites["missing_texture"];
        }

        private Sprite GetAnimation(RenderableComponent component, GameTime gameTime)
        {
            LoadSpriteSheet(Animation.Animation.GetSheetName(component.AssetName));
            component.AssetName = Animation.Animation.AdvanceAnimation(
                component.AssetName, (float)gameTime.ElapsedGameTime.TotalMilliseconds, animations);
            var anim = Animation.Animation.GetFrame(component.AssetName, animations);

            if (anim == null)
            {
                Logger.Error("Missing texture for animation: " +
                    component.SpriteSheetName + "/" + component.AssetName);
                //Texture missing, return MISSING_Texture texture
                return blankSpriteSheet.Sprites["missing_texture"];
            }
            return anim;
        }

        public Sprite GetAnimation(string animName, float positionMs, string sheetName = null)
        {
            if (sheetName != null && sheetName != "" && !spriteSheets.ContainsKey(sheetName))
                LoadSpriteSheet(sheetName);
            var anim = Animation.Animation.GetFrame(animName, animations, positionMs);
            if (anim == null)
            {
                Logger.Error("Missing texture for animation: " +
                    animName);
                //Texture missing, return MISSING_Texture texture
                return blankSpriteSheet.Sprites["missing_texture"];
            }
            return anim;
        }

        public bool AnimEnded(string animName, float positionMs, string sheetName = null, float loopCount = 1)
        {
            if (sheetName != null && sheetName != "" && !spriteSheets.ContainsKey(sheetName))
                LoadSpriteSheet(sheetName);
            return Animation.Animation.Ended(animName, animations, positionMs, loopCount);
        }

        private void LoadSpriteSheet(string sheetName)
        {
            try
            {
                if (spriteSheets.ContainsKey(sheetName))
                    return;

                FileStream fStream = null;
                fStream = System.IO.File.OpenRead(sheetName);
                var texture = Texture2D.FromStream(game.GraphicsDevice, fStream);

                var sheet = Newtonsoft.Json.JsonConvert.DeserializeObject<JSONSpriteSheet>(
                    System.IO.File.ReadAllText(sheetName + ".json"));

                //Load sprites
                var sprites = new Dictionary<string, Rectangle>();
                foreach (var sprite in sheet.Sprites)
                    sprites.Add(sprite.Name, new Rectangle(
                        sprite.X, sprite.Y, sprite.Width, sprite.Height));

                //Load animations, if any
                Dictionary<string, Animation.Animation> _animations = null;
                if (sheet.Animations != null)
                {
                    _animations = new Dictionary<string, Animation.Animation>();
                    foreach (var anim in sheet.Animations)
                        _animations.Add(anim.Name,
                            new Animation.Animation(anim.Friendly, anim.Frames, anim.FrameRate));

                    //And add to the global table
                    foreach (var anim in _animations)
                        animations.Add(anim.Key, anim.Value);
                }

                //Build spritesheet
                var spriteSheet = new SpriteSheet(sheet.Name, texture, sprites, _animations);
                spriteSheets.Add(sheetName, spriteSheet);

                fStream.Dispose();
            }
            catch
            {
                Logger.Error("Texture Load Failed! File: " + sheetName);
            }
        }

        public void Dispose()
        {
            foreach (var sheet in spriteSheets)
                sheet.Value.Texture.Dispose();

            blankSpriteSheet.Texture.Dispose();
        }

        private class JSONSpriteSheet
        {
            public string Name = null;

            public JSONSprite[] Sprites = null;
            public class JSONSprite
            {
                public string Name = null;
                public int X = 0;
                public int Y = 0;
                public int Width = 0;
                public int Height = 0;

            }

            public JSONAnimation[] Animations = null;
            public class JSONAnimation
            {
                public string Name = null;
                public string Friendly = null;
                public float FrameRate = 0;
                public string[] Frames = null;
            }
        }
    }
}
