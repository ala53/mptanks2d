using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Engine.Logging;
using MPTanks.Rendering.Renderer.Assets.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Rendering.Renderer.Assets
{
    class AssetCache
    {
        private GraphicsDevice _graphics;
        private AssetLoader _loader;
        private GameWorldRenderer _renderer;

        private Sprites.SpriteSheet _internalSprites
        {
            get { return _spriteSheets["asset_cache_internal_spritesheet"]; }
        }

        /// <summary>
        /// A checkerboard purple and white sprite sheet to use when we can't find the sprite sheet or sprite
        /// </summary>
        public Sprites.Sprite MissingTextureSprite
        {
            get { return _internalSprites["missing_texture_sprite"]; }
        }
        /// <summary>
        /// A completely transparent, black, sprite to display while the actual texture is loading.
        /// </summary>
        public Sprites.Sprite LoadingTextureSprite
        {
            get { return _internalSprites["loading_texture_sprite"]; }
        }

        private Dictionary<string, Sprites.SpriteSheet> _spriteSheets = new Dictionary<string, Sprites.SpriteSheet>();
        private HashSet<string> _sheetsWhichHaveBeenLoaded = new HashSet<string>();
        private HashSet<string> _sheetsThatAreCurrentlyLoading = new HashSet<string>();


        public AssetCache(GraphicsDevice gd, AssetLoader loader, GameWorldRenderer renderer)
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

            //O(infinity) aka O(horror movie)
            for (var y = 0; y < checkerboardSize; y += 8)
                for (var x = 0; x < checkerboardSize; x += 8)
                {
                    Color checkerBoardColor = y % 16 == 0 && x % 16 != 0 ? Color.Purple : Color.White;
                    //i is y offset, j is x offset
                    for (var i = 0; i < 8; i++)
                        for (var j = 0; j < 8; i++)
                        {
                            data[((y + i) * checkerboardSize) + x + j] = checkerBoardColor;
                        }
                }

            tx.SetData(data);

            var sprites = new Dictionary<string, Sprite>();
            sprites.Add("missing_texture_sprite", new Sprite(0, 0, 48, 48, "missing_texture_sprite"));
            sprites.Add("loading_texture_sprite", new Sprite(52, 52, 54, 54, "loading_texture_sprite"));

            _spriteSheets.Add("asset_cache_internal_spritesheet",
                new SpriteSheet(new Dictionary<string, Animation>(), sprites, tx, "asset_cache_internal_spritesheet"));
        }

        public Sprite GetSprite(string spriteName, string sheetName)
        {
            spriteName = spriteName.ToLower();
            sheetName = sheetName.ToLower();

            //First, check if it's loaded and if it is, just return it
            if (_spriteSheets.ContainsKey(sheetName)) //Loaded
            {
                if (_spriteSheets[sheetName].ContainsKey(spriteName)) //And the sprite exists
                    return _spriteSheets[sheetName][spriteName]; //so return it
                else return MissingTextureSprite; //otherwise, die horribly
            }

            //Then check if we've tried to load it already and are just waiting
            if (_sheetsThatAreCurrentlyLoading.Contains(sheetName))
                return LoadingTextureSprite;

            //Otherwise, check that we haven't tried to load it already and then load it
            if (!_sheetsWhichHaveBeenLoaded.Contains(sheetName))
            {
                LoadSpriteSheet(sheetName);
                return LoadingTextureSprite;
            }

            //Fall through and return something, at least
            //We hit this if something bad happens or if we've tried to load the sheet and couldn't find it
            return MissingTextureSprite;
        }

        private void LoadSpriteSheet(string sheetName)
        {
            if (_sheetsWhichHaveBeenLoaded.Contains(sheetName) || _sheetsThatAreCurrentlyLoading.Contains(sheetName))
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
    }
}
