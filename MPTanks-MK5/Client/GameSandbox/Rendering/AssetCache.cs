using MPTanks.Engine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Client.GameSandbox.Rendering.Sprites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Engine.Settings;
using System.Runtime.CompilerServices;

namespace MPTanks.Client.GameSandbox.Rendering
{
    /// <summary>
    /// An internal cache so that multiple gameobjects using the same art asset can run
    /// </summary>
    class AssetCache
    {
        private Dictionary<string, Sprites.SpriteSheet> spriteSheets =
            new Dictionary<string, Sprites.SpriteSheet>();
        //The helper texture sprite sheet
        private Sprites.SpriteSheet _blankSpriteSheet;
        private Sprite _missingTexture;
        private Sprite _loading;
        private Sprite _blank;

        private Game _game;

        public AssetCache(Game game)
        {
            _game = game;

            var texture = new Texture2D(_game.GraphicsDevice, 4, 4);

            //Create a checkerboard pattern (and transparency on the side)
            // |w|w|p|p|
            // |w|w|p|p|
            // |p|p|w|w|
            // |p|p|w|w|
            texture.SetData(new[] {
                Color.White, Color.White, Color.Purple, Color.Purple, Color.TransparentBlack, Color.TransparentBlack,
                Color.White, Color.White, Color.Purple, Color.Purple, Color.TransparentBlack, Color.TransparentBlack,
                Color.Purple, Color.Purple, Color.White, Color.White, Color.TransparentBlack, Color.TransparentBlack,
                Color.Purple, Color.Purple, Color.White, Color.White, Color.TransparentBlack, Color.TransparentBlack
            });

            var sprites = new Dictionary<string, Rectangle>();
            sprites.Add("texture_missing", new Rectangle(0, 0, 6, 4));
            sprites.Add("blank", new Rectangle(0, 0, 1, 1));
            sprites.Add("loading", new Rectangle(5, 0, 1, 1));
            _blankSpriteSheet = new SpriteSheet("missing_texture_sheet", texture, sprites);

            _loading = _blankSpriteSheet.Sprites["loading"];
            _blank = _blankSpriteSheet.Sprites["blank"];
            _missingTexture = _blankSpriteSheet.Sprites["texture_missing"];
        }


        /// <summary>
        /// Gets an art asset by sheet and asset name, handling animations
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="assetName"></param>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public Sprite GetArtAsset(string sheetName, string assetName, GameTime gameTime)
        {
            //No texture is given, just return a generic white box
            if (assetName == null && sheetName == null)
                return _blank;

            //For safety, don't allow the sheet name to be null
            if (sheetName == null) sheetName = "";

            //Special path for animations.
            if (assetName.StartsWith("[animation]"))
                return GetAnimation(assetName, gameTime);

            //Check if we've loaded the sprite sheet or not
            if (!HasSpriteSheetLoadBeenCalled(sheetName))
            {
                AsyncLoadSpriteSheet(sheetName); //If not, load it
                //and return
                return _loading;
            }
            //If loading but not loaded...
            if (!HasSpriteSheetLoadCompleted(sheetName))
                return _loading;

            //Check if the load failed
            if (spriteSheets.ContainsKey(sheetName))
                if (spriteSheets[sheetName].Sprites.ContainsKey(assetName)) //Check if the sprite exists
                    return spriteSheets[sheetName].Sprites[assetName];

            //Texture missing, return MISSING_Texture texture
            return _missingTexture;
        }

        /// <summary>
        /// Gets an animation by asset name, at time t
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        private Sprite GetAnimation(string assetName, GameTime gameTime)
        {
            //Get the sheet name from the animation
            var sheetName = Animation.Animation.GetSheetName(assetName);
            //Check if the sprite sheet is loading or loaded
            if (!HasSpriteSheetLoadBeenCalled(sheetName))
            {
                AsyncLoadSpriteSheet(sheetName);
                return _loading;
            }
            //If loading but not loaded...
            if (!HasSpriteSheetLoadCompleted(sheetName)) return _loading;

            //get the sheet
            var sheet = spriteSheets[sheetName];

            //Get the animation ticked to the current frame
            assetName = Animation.Animation.AdvanceAnimation(
                assetName, (float)gameTime.ElapsedGameTime.TotalMilliseconds, sheet.Animations);
            //and get the frame sprite
            var anim = Animation.Animation.GetFrame(assetName, sheet.Animations);
            //check if the animation was null
            if (anim == null)
            {
                //If so, log it
                Logger.Error("Missing texture for animation: " + assetName);
                //Texture missing, return MISSING_Texture texture
                return _missingTexture;
            }
            //If not, return the sprite
            return anim;
        }

        public Sprite GetAnimation(string animName, float positionMs, string sheetName = null)
        {
            //Check if the sprite sheet is loading or loaded
            if (!HasSpriteSheetLoadBeenCalled(sheetName))
            {
                AsyncLoadSpriteSheet(sheetName);
                return _loading;
            }
            //If loading but not loaded...
            if (!HasSpriteSheetLoadCompleted(sheetName)) return _loading;
            //Get the sheet
            var sheet = spriteSheets[sheetName];
            //And the animation frame
            var anim = Animation.Animation.GetFrame(animName, sheet.Animations, positionMs);
            //Check if the animation exists
            if (anim == null)
            {
                //If not, log it
                Logger.Error("Missing texture for animation: " +
                    animName);
                //Texture missing, return MISSING_Texture texture
                return _missingTexture;
            }
            //If it exists, return it
            return anim;
        }

        public bool AnimEnded(string animName, float positionMs, string sheetName = null, float loopCount = 1)
        {
            //Check if the sprite sheet is loading or loaded
            if (!HasSpriteSheetLoadBeenCalled(sheetName))
            {
                AsyncLoadSpriteSheet(sheetName);
                return false; //Be conservative
            }
            //If loading but not loaded...
            if (!HasSpriteSheetLoadCompleted(sheetName))
                return false; //Be conservative and don't say it ended until we're sure

            //Get the sheet
            var sheet = spriteSheets[sheetName];
            //And do the check
            return Animation.Animation.Ended(animName, sheet.Animations, positionMs, loopCount);
        }

        /// <summary>
        /// A list of sprite sheets that LoadSpriteSheet() was called and whether the call was completed.
        /// </summary>
        private Dictionary<string, bool> _sheetsWithLoadCalled = new Dictionary<string, bool>();
        private void AsyncLoadSpriteSheet(string sheetName)
        {
            //Do not do duplicate loads
            if (HasSpriteSheetLoadBeenCalled(sheetName)) return;
            //Note that we have called load on it
            lock (_sheetsWithLoadCalled)
                _sheetsWithLoadCalled.Add(sheetName, false);
            //And async load
            if (GlobalSettings.Debug)
            {
                AsyncLoadFunction(sheetName);
                lock (_sheetsWithLoadCalled)
                    _sheetsWithLoadCalled[sheetName] = true;
            }
            else
                Task.Run(() =>
                {

                    try
                    {
                        AsyncLoadFunction(sheetName);
                    }
                    catch (Exception e)
                    {
                        //If something goes wrong...log it 
                        Logger.Error($"Texture Load Failed! {sheetName}");
                        Logger.Error(e.ToString());
                    }
                    finally
                    {
                        lock (_sheetsWithLoadCalled)
                            _sheetsWithLoadCalled[sheetName] = true;
                    }
                });
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private void AsyncLoadFunction(string sheetName)
        {
            if (!File.Exists(sheetName) || !File.Exists(sheetName + ".json"))
                throw new FileNotFoundException($"{sheetName} or {sheetName}.json does not exist.");
            sheetName = AssetResolver.Resolve(sheetName);
            //Open a file stream to the sheet
            FileStream fStream = null;
            fStream = File.OpenRead(sheetName);
            //And upload to the GPU
            var texture = Texture2D.FromStream(_game.GraphicsDevice, fStream);
            //Parse the Sprite sheet's metadata (from a file named *.*.json)
            var sheet = Newtonsoft.Json.JsonConvert.DeserializeObject<JSONSpriteSheet>(
                File.ReadAllText(sheetName + ".json"));

            //Build the sprite tree from the metadata
            var sprites = new Dictionary<string, Rectangle>();
            foreach (var sprite in sheet.Sprites)
                sprites.Add(sprite.Name, new Rectangle(
                    sprite.X, sprite.Y, sprite.Width, sprite.Height));

            //Load animations, if any
            Dictionary<string, Animation.Animation> _animations = null;
            if (sheet.Animations != null)
            {
                _animations = new Dictionary<string, Animation.Animation>();
                foreach (var anim in sheet.Animations) //Go through each and create the animation object
                    _animations.Add(anim.Name,
                        new Animation.Animation(anim.Friendly, anim.Frames, anim.FrameRate));
            }

            //And build the spritesheet
            var spriteSheet = new SpriteSheet(sheet.Name, texture, sprites, _animations);
            spriteSheets.Add(sheetName, spriteSheet); //And add it to the global table

            fStream.Dispose(); //And finally, dispose of the file stream
        }

        private bool HasSpriteSheetLoadBeenCalled(string sheetName)
        {
            lock (_sheetsWithLoadCalled)
                return _sheetsWithLoadCalled.ContainsKey(sheetName);
        }

        private bool HasSpriteSheetLoadCompleted(string sheetName)
        {
            lock (_sheetsWithLoadCalled)
                return HasSpriteSheetLoadBeenCalled(sheetName) && _sheetsWithLoadCalled[sheetName];
        }

        public void Dispose()
        {
            foreach (var sheet in spriteSheets)
                sheet.Value.Texture.Dispose();

            _blankSpriteSheet.Texture.Dispose();
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
