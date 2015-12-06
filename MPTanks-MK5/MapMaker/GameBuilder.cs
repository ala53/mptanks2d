using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Client.Backend.Renderer;
using MPTanks.Client.GameSandbox;
using MPTanks.Client.GameSandbox.Mods;
using MPTanks.Engine;
using MPTanks.Engine.Gamemodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPTanks.Clients.MapMaker
{
    class GameBuilder : Game
    {
        private MenuForm _menus;
        private MapData.MapData _map;
        private GameCore _game;
        private GameCoreRenderer _renderer;
        private SpriteBatch _sb;
        protected override void Initialize()
        {
            _menus = new MenuForm(this);
            CoreModLoader.LoadTrustedMods(GameSettings.Instance);
            base.Initialize();
        }

        private void CreateMap()
        {
            _map = new MapData.MapData();
            UpdateModsList();
        }

        private void UpdateModsList()
        {
            _map.Mods.AddRange(Modding.ModDatabase.LoadedModules.Select(a => a.ModInfo));
            _map.Mods = _map.Mods.Distinct().ToList();
        }

        public void Restart()
        {
            Application.Restart();
            Application.Exit();
            Exit();
        }

        public void OnMapChanged()
        {
            var map = _map.GenerateMap();
            _game = new GameCore(null, new NullGamemode(), map);
            _game.BeginGame(true);
            _renderer.Game = _game;
        }

        protected override void LoadContent()
        {
            _renderer = new GameCoreRenderer(this, GameSettings.Instance.AssetSearchPaths, new[] { 0 });
            _renderer.Game = _game;
            base.LoadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            _game.Update(gameTime);
            base.Update(gameTime);
        }
        private RenderTarget2D _worldTarget;
        protected override void Draw(GameTime gameTime)
        {
            _renderer.Target = _worldTarget;
            //And draw the world
            _renderer.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(null);
            _sb.Begin();
            _sb.Draw(_worldTarget,
                new Rectangle(0, 0,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height), 
                Color.White);
            _sb.End();

            base.Draw(gameTime);
        }
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
    }
}
