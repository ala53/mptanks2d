using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MPTanks.Client.Backend.Renderer;
using MPTanks.Client.Backend.UI;
using MPTanks.Client.GameSandbox;
using MPTanks.Client.GameSandbox.Mods;
using MPTanks.Engine;
using MPTanks.Engine.Core;
using MPTanks.Engine.Gamemodes;
using MPTanks.Engine.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.MapMaker
{
    public partial class GameBuilder : Game
    {
        private MapData.MapData _map;
        private GraphicsDeviceManager _graphics;
        private GameCore _game;
        private GameCoreRenderer _renderer;
        private SpriteBatch _sb;
        private UserInterface _ui;

        public ILogger Logger { get; private set; } = new NullLogger();

        private Vector2 _cameraPosition;
        private float _cameraZoom = 1;

        private bool __active = true;
        private bool _active
        {
            get { return __active && IsActive; }
            set { __active = value; }
        }

        public GameBuilder()
        {
            Content.RootDirectory = "assets/mgcontent";
            _graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            CoreModLoader.LoadTrustedMods(GameSettings.Instance);
            Components.Add(new Starbound.Input.KeyboardEvents(this));
            Components.Add(new Starbound.Input.MouseEvents(this));

            Starbound.Input.KeyboardEvents.KeyPressed += KeyboardEvents_KeyPressed;
            Starbound.Input.KeyboardEvents.KeyReleased += KeyboardEvents_KeyReleased;

            Starbound.Input.MouseEvents.ButtonDoubleClicked += MouseEvents_ButtonDoubleClicked;
            Starbound.Input.MouseEvents.ButtonClicked += MouseEvents_ButtonClicked;
            Starbound.Input.MouseEvents.ButtonReleased += MouseEvents_ButtonReleased; ;

            base.Initialize();
        }

        private void MouseEvents_ButtonReleased(object sender, Starbound.Input.MouseButtonEventArgs e)
        {
            UI_MouseReleased();
        }

        private void MouseEvents_ButtonClicked(object sender, Starbound.Input.MouseButtonEventArgs e)
        {
            UI_ProcessClickInGameArea(UI_MousePosition);
        }

        private void MouseEvents_ButtonDoubleClicked(object sender, Starbound.Input.MouseButtonEventArgs e)
        {
        }

        private void KeyboardEvents_KeyReleased(object sender, Starbound.Input.KeyboardEventArgs e)
        {
        }

        private void KeyboardEvents_KeyPressed(object sender, Starbound.Input.KeyboardEventArgs e)
        {
            if (e.Key == Keys.Escape)
                UI_EscapePressed();
        }

        private void CreateMap()
        {
            _map = new MapData.MapData();
            UpdateModsList();
            OpenNewMap();
            _ui.UpdateState(new object());
        }

        private void UpdateModsList()
        {
            _map.Mods.AddRange(Modding.ModDatabase.LoadedModulesList.Select(a => a.ModInfo));
            _map.Mods = _map.Mods.Distinct().ToList();
            _ui.UpdateState(new object());
        }

        public void Restart()
        {
            System.Windows.Forms.Application.Restart();
            System.Windows.Forms.Application.Exit();
            Exit();
        }

        public void OpenNewMap()
        {
            var map = MapData.MapData.Default;
            _game = new GameCore(null, new NullGamemode(), map);
            _game.Authoritative = true;
            _game.BeginGame(true);
            _renderer.Game = _game;
        }

        protected override void LoadContent()
        {
            _sb = new SpriteBatch(GraphicsDevice);
            _renderer = new GameCoreRenderer(this, GameSettings.Instance.AssetSearchPaths, new[] { 0 }, new NullLogger());
            _renderer.Game = _game;
            _ui = new UserInterface(this);
            CreateMap();
            UI_ShowPrimaryMenu();
            base.LoadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            if (_active)
            {
                var keyState = Keyboard.GetState();
                float spd = 0.5f * _cameraZoom;
                if (keyState.IsKeyDown(Keys.LeftShift))
                    spd = 0.15f * _cameraZoom;
                //Handle WASD
                if (keyState.IsKeyDown(Keys.W))
                    _cameraPosition.Y -= spd;
                if (keyState.IsKeyDown(Keys.S))
                    _cameraPosition.Y += spd;
                if (keyState.IsKeyDown(Keys.A))
                    _cameraPosition.X -= spd;
                if (keyState.IsKeyDown(Keys.D))
                    _cameraPosition.X += spd;

                float zoomSpeed = 0.05f;
                if (keyState.IsKeyDown(Keys.LeftShift))
                    zoomSpeed = 0.015f;
                //And Q/E for zoom
                if (_cameraZoom < 10 && keyState.IsKeyDown(Keys.E))
                    _cameraZoom += zoomSpeed * _cameraZoom;

                if (_cameraZoom > 0.05 && keyState.IsKeyDown(Keys.Q))
                    _cameraZoom -= zoomSpeed * _cameraZoom;
            }

            _ui.UpdateState(new object());
            _ui.Update(gameTime);

            UI_Update(gameTime);

            _game.Update(gameTime);
            base.Update(gameTime);
        }
        private RenderTarget2D _worldTarget;
        protected override void Draw(GameTime gameTime)
        {
            EnsureRenderTargetSizing();
            //Clear the render target
            GraphicsDevice.SetRenderTarget(_worldTarget);
            GraphicsDevice.Clear(_game.Map.BackgroundColor);

            _renderer.Target = _worldTarget;

            //And draw the world
            _renderer.Draw(gameTime);

            _renderer.View = ComputeDrawRectangle();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            _sb.Begin();
            _sb.Draw(_worldTarget,
                new Rectangle(0, 0,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height),
                Color.White);
            _sb.End();

            DrawGridLines();

            _ui.Draw(gameTime);

            UI_DrawXYPositionAndZoom();
            
            base.Draw(gameTime);
        }

        private RectangleF ComputeDrawRectangle()
        {
            var rectSizeX = 60 * _cameraZoom;
            var halfRectSizeX = rectSizeX / 2;
            var heightToWidth = (float)GraphicsDevice.Viewport.Height / GraphicsDevice.Viewport.Width;
            var rectSizeY = rectSizeX * heightToWidth;
            var halfRectSizeY = halfRectSizeX * heightToWidth;
            return new RectangleF(
                _cameraPosition.X - halfRectSizeX,
                _cameraPosition.Y - halfRectSizeY,
                rectSizeX, rectSizeY);
        }
        private void EnsureRenderTargetSizing()
        {
            if (_worldTarget == null || _worldTarget.Width != GraphicsDevice.Viewport.Width ||
                _worldTarget.Height != GraphicsDevice.Viewport.Height)
            {
                _worldTarget?.Dispose();
                _worldTarget = new RenderTarget2D(
                    GraphicsDevice,
                    GraphicsDevice.Viewport.Width,
                    GraphicsDevice.Viewport.Height);
            }
        }
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
    }
}
