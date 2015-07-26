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
using MPTanks.Engine.Assets;

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
        public Sprite GetArtAsset(ref SpriteInfo spriteInfo, float timescale, GameTime gameTime)
        {
            //No texture is given, just return a generic white box
            if (spriteInfo.SheetName == null && spriteInfo.FrameName == null)
                return _blank;

            //Special path for animations.
            if (spriteInfo.IsAnimation)
                return GetAnimation(ref spriteInfo, timescale, gameTime);

            //Check if we've loaded the sprite sheet or not
            if (spriteInfo.SheetName != null && !HasSpriteSheetLoadBeenCalled(spriteInfo.SheetName))
            {
                AsyncLoadSpriteSheet(spriteInfo.SheetName); //If not, load it
                //and return
                return _loading;
            }
            //If loading but not loaded...
            if (!HasSpriteSheetLoadCompleted(spriteInfo.SheetName))
                return _loading;

            //Check if the load failed
            if (spriteInfo.SheetName != null && spriteInfo.FrameName != null &&
                spriteSheets.ContainsKey(spriteInfo.SheetName))
                if (spriteSheets[spriteInfo.SheetName].Sprites.ContainsKey(spriteInfo.FrameName)) //Check if the sprite exists
                    return spriteSheets[spriteInfo.SheetName].Sprites[spriteInfo.FrameName];

            //Texture missing, return MISSING_Texture texture
            return _missingTexture;
        }

        public Sprite GetAnimation(ref SpriteInfo spriteInfo, float timescale, GameTime gameTime)
        {
            //Check if the sprite sheet is loading or loaded
            if (!HasSpriteSheetLoadBeenCalled(spriteInfo.SheetName))
            {
                AsyncLoadSpriteSheet(spriteInfo.SheetName);
                return _loading;
            }
            //If loading but not loaded...
            if (!HasSpriteSheetLoadCompleted(spriteInfo.SheetName)) return _loading;

            //Safety dance
            if (!spriteSheets.ContainsKey(spriteInfo.SheetName)) return _missingTexture;
            //Get the sheet
            var sheet = spriteSheets[spriteInfo.SheetName];
            if (!sheet.Animations.ContainsKey(spriteInfo.FrameName)) return _missingTexture;
            //Get the animation
            var anim = sheet.Animations[spriteInfo.FrameName];

            //Check if ended 
            if ((anim.LengthMs * spriteInfo.LoopCount) <= spriteInfo.PositionInAnimationMs)
            {
                if (sheet.Sprites.ContainsKey(anim.FrameNames.Last())) return sheet.Sprites[anim.FrameNames.Last()];
                return _missingTexture;
            }
            //get the frame number
            var position = spriteInfo.PositionInAnimationMs;
            var frame = (int)(position / anim.FrameLengthMs) % anim.FrameNames.Length;

            //increment the animation
            spriteInfo.PositionInAnimationMs += ((float)gameTime.ElapsedGameTime.TotalMilliseconds * timescale) / 2;
            //and return the frame
            if (sheet.Sprites.ContainsKey(anim.FrameNames[frame]))
                return sheet.Sprites[anim.FrameNames[frame]];
            else return _missingTexture;
        }

        public bool AnimEnded(SpriteInfo spriteInfo)
        {
            //Check if the sprite sheet is loading or loaded
            if (!HasSpriteSheetLoadBeenCalled(spriteInfo.SheetName))
            {
                AsyncLoadSpriteSheet(spriteInfo.SheetName);
                return false; //Be conservative
            }
            //If loading but not loaded...
            if (!HasSpriteSheetLoadCompleted(spriteInfo.SheetName))
                return false; //Be conservative and don't say it ended until we're sure

            if (!spriteSheets.ContainsKey(spriteInfo.SheetName)) return true;
            //Get the sheet
            var sheet = spriteSheets[spriteInfo.SheetName];
            if (!sheet.Animations.ContainsKey(spriteInfo.FrameName)) return true;
            //And do the check
            var totalLength = sheet.Animations[spriteInfo.FrameName].LengthMs * spriteInfo.LoopCount;
            return (spriteInfo.PositionInAnimationMs >= totalLength);
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
