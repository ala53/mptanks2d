using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Engine.Assets;
using MPTanks.Engine.Logging;
using MPTanks.Client.Backend.Renderer.Assets.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Renderer.Assets
{
    class AssetCache : IDisposable
    {
        public const string LoadingTextureSpriteName = "loading_texture_sprite";
        public const string MissingTextureSpriteName = "missing_texture_sprite";

        private GraphicsDevice _graphics;
        private AssetLoader _loader;
        private GameCoreRenderer _renderer;

        private SpriteSheet _internalSprites
        {
            get { return _spriteSheets["asset_cache_internal_spritesheet"]; }
        }

        /// <summary>
        /// A checkerboard purple and white sprite sheet to use when we can't find the sprite sheet or sprite
        /// </summary>
        public Sprite MissingTextureSprite
        {
            get { return _internalSprites[MissingTextureSpriteName]; }
        }
        /// <summary>
        /// A completely transparent, black, sprite to display while the actual texture is loading.
        /// </summary>
        public Sprite LoadingTextureSprite
        {
            get { return _internalSprites[LoadingTextureSpriteName]; }
        }

        private Dictionary<string, SpriteSheet> _spriteSheets = new Dictionary<string, SpriteSheet>(
            StringComparer.InvariantCultureIgnoreCase);
        private HashSet<string> _sheetsWhichHaveBeenLoaded = new HashSet<string>(
            StringComparer.InvariantCultureIgnoreCase);
        private HashSet<string> _sheetsThatAreCurrentlyLoading = new HashSet<string>(
            StringComparer.InvariantCultureIgnoreCase);


        public AssetCache(GraphicsDevice gd, AssetLoader loader, GameCoreRenderer renderer)
        {
            _graphics = gd;
            _loader = loader;
            _renderer = renderer;

            GenerateInternalSpriteSheet();
        }

        private void GenerateInternalSpriteSheet()
        {
            var tx = new Texture2D(_graphics, 64, 64);

            var data = new Color[64 * 64];

            //Color it transparent black
            for (var i = 0; i < data.Length; i++)
                data[i] = Color.TransparentBlack;

            const int checkerboardSize = 48;

            //O(horror movie) aka O(4) loop
            for (var y = 0; y < checkerboardSize; y += 8)
                for (var x = 0; x < checkerboardSize; x += 8)
                {
                    Color checkerBoardColor = y % 16 == 0 && x % 16 != 0 ? Color.Purple : Color.White;
                    //i is y offset, j is x offset
                    for (var j = 0; j < 8; j++)
                        for (var k = 0; k < 8; k++)
                            data[((y + j) * checkerboardSize) + x + k] = checkerBoardColor;
                }

            tx.SetData(data);

            var sprites = new Dictionary<string, Sprite>();
            sprites.Add(MissingTextureSpriteName, new Sprite(0, 0, 48, 48, MissingTextureSpriteName));
            sprites.Add(LoadingTextureSpriteName, new Sprite(52, 52, 54, 54, LoadingTextureSpriteName));

            _spriteSheets.Add("asset_cache_internal_spritesheet",
                new SpriteSheet(new Dictionary<string, Animation>(), sprites, tx, "asset_cache_internal_spritesheet"));
        }

        public Animation GetAnimation(string animationName, string sheetName)
        {
            var sheet = GetSpriteSheet(sheetName);
            if (sheet == null) return null;
            if (!sheet.AnimationsByName.ContainsKey(animationName))
                return sheet.AnimationsByName[animationName];

            return null;
        }

        public bool IsLoading(string sheetName) => _sheetsThatAreCurrentlyLoading.Contains(sheetName);
        public bool HasLoadStarted(string sheetName) => _sheetsWhichHaveBeenLoaded.Contains(sheetName);
        public bool IsLoaded(string sheetName) => HasLoadStarted(sheetName) && !IsLoading(sheetName);

        public Sprite GetSprite(SpriteInfo info) => GetSprite(info.FrameName, info.SheetName);
        public Sprite GetSprite(string spriteName, string sheetName)
        {
            //special cases
            if (spriteName == LoadingTextureSpriteName) return LoadingTextureSprite;
            if (spriteName == MissingTextureSpriteName) return MissingTextureSprite;

            if (IsLoading(sheetName)) return LoadingTextureSprite;

            var sheet = GetSpriteSheet(sheetName); //get the sheet
            if (sheet != null && sheet.ContainsKey(spriteName)) //And see if the sprite exists
                return _spriteSheets[sheetName][spriteName]; //so return it
            else return MissingTextureSprite; //otherwise, die horribly
        }

        private SpriteSheet GetSpriteSheet(string sheetName)
        {
            if (_spriteSheets.ContainsKey(sheetName)) //Loaded
                return _spriteSheets[sheetName]; // give them it
            else if (IsLoaded(sheetName)) return null; //loading; do nothing
            else if (!HasLoadStarted(sheetName)) //not loaded, tell it to load
            {
                LoadSpriteSheet(sheetName);
                return null;
            }
            else return null;
        }

        private void LoadSpriteSheet(string sheetName)
        {
            if (HasLoadStarted(sheetName))
            {
                _renderer.Logger.Error("Spritesheet load called more than once for " + sheetName);
                return;
            }

            _sheetsWhichHaveBeenLoaded.Add(sheetName);
            _sheetsThatAreCurrentlyLoading.Add(sheetName);

            _loader.DeferredLoadSpriteSheet(sheetName, (sheet) =>
            {
                _spriteSheets.Add(sheetName, sheet);
                _sheetsThatAreCurrentlyLoading.Remove(sheetName);
            }, () =>
            {
                _sheetsThatAreCurrentlyLoading.Remove(sheetName);
            }, MissingTextureSprite);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                foreach (var spriteSheet in _spriteSheets)
                    spriteSheet.Value.Texture.Dispose();

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
