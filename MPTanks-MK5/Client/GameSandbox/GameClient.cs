﻿#region Using Statements
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using MPTanks.Engine.Tanks;
using MPTanks.Engine;
using System.Diagnostics;
using System.Runtime;
using System.Text;
using MPTanks.Engine.Rendering.Particles;
using MPTanks.Client.Backend.UI;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Media;
using MPTanks.Engine.Core;
using MPTanks.Engine.Settings;
using MPTanks.Engine.Logging;
using MPTanks.Networking.Common.Game;
using MPTanks.Networking.Common;
using System.Threading.Tasks;
using MPTanks.Client.GameSandbox.Mods;
using MPTanks.Client.Backend.Renderer;
using MPTanks.Client.GameSandbox.Input;
using MPTanks.Client.GameSandbox.UI;
#endregion

namespace MPTanks.Client.GameSandbox
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameClient : Game
    {
        GraphicsDeviceManager _graphics;
        private bool _graphicsDeviceIsDirty = false;
        SpriteBatch _spriteBatch;
        private NetworkPlayer _player;
        public MPTanks.Networking.Client.Client Client { get; private set; }
        public MPTanks.Networking.Server.Server Server { get; private set; }
        public Backend.Sound.SoundPlayer SoundPlayer { get; private set; }
        public InputDriverBase InputDriver { get; private set; }
        public GameCoreRenderer GameRenderer { get; private set; }
        private UserInterface _ui;
        internal DebugDrawer DebugDrawer { get; private set; }

        const string _settingUpPageName = "SettingUpPrompt";

        public Diagnostics Diagnostics => Client?.GameInstance?.Diagnostics;

        public GameClient()
            : base()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "assets/mgcontent";
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

            _graphics.PreferMultiSampling = true;
            _graphics.DeviceCreated += graphics_DeviceCreated;

            Window.AllowUserResizing = true;

            IsFixedTimeStep = false;

            GameSettings.Instance.InputDriverName.Value = KeyboardMouseInputDriver.Name;
            CoreModLoader.LoadTrustedMods(GameSettings.Instance);

        }

        void graphics_DeviceCreated(object sender, EventArgs e)
        {
            //Set startup properties from the parent's crossdomainobject
            _graphics.PreferredBackBufferWidth = CrossDomainObject.Instance.WindowWidth;
            _graphics.PreferredBackBufferHeight = CrossDomainObject.Instance.WindowHeight;
            Window.Position = new Point(
                CrossDomainObject.Instance.WindowPositionX,
                CrossDomainObject.Instance.WindowPositionY);

            _graphics.SynchronizeWithVerticalRetrace = GameSettings.Instance.VSync;

            _graphicsDeviceIsDirty = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Set up resize handler
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            //Initialize evented input
            Components.Add(new Starbound.Input.KeyboardEvents(this));
            Components.Add(new Starbound.Input.MouseEvents(this));
            Components.Add(new Starbound.Input.GamePadEvents(PlayerIndex.One, this));

            Starbound.Input.KeyboardEvents.KeyPressed += KeyboardEvents_KeyPressed;

            _ui = new UserInterface(Content, this);

            //initialize the game input driver
            InputDriver = InputDriverBase.GetDriver(GameSettings.Instance.InputDriverName, this);
            if (GameSettings.Instance.InputKeyBindings.Value != null)
                InputDriver.SetKeyBindings(GameSettings.Instance.InputKeyBindings);
            base.Initialize();
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            _graphicsDeviceIsDirty = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            CreateGame();
        }

        private void CreateGame()
        {
            var game = new GameCore(
                new NLogLogger(Logger.Instance),
                Engine.Gamemodes.Gamemode.ReflectiveInitialize("DeathMatchGamemode"),
                Modding.ModLoader.LoadedMods["core-assets.mod"].GetAsset("testmap.json"),
                false,
                new EngineSettings("enginesettings.json")
                );
            game.Authoritative = true;
            game.FriendlyFireEnabled = true;
            
            _ui.SetPage(_settingUpPageName);

            _player = (NetworkPlayer)game.AddPlayer(new NetworkPlayer()
            {
                Id = Guid.NewGuid()
            });

            for (var i = 0; i < 5; i++)
            {
                game.AddPlayer(new NetworkPlayer { Id = Guid.NewGuid() });
            }
            //TEMP
            Client = new Networking.Client.Client("", 0);
            Server = new Networking.Server.Server(33132, "password", game);
            Client.GameInstance.GameChanged += delegate
            {
                GameRenderer?.Dispose();
                GameRenderer = new GameCoreRenderer(
                    this, Client.GameInstance.Game, GameSettings.Instance.AssetSearchPaths, new[] { 0 });
                SoundPlayer?.Dispose();
                SoundPlayer = new Backend.Sound.SoundPlayer(Client.GameInstance.Game);
                DebugDrawer?.Dispose();
                DebugDrawer = new DebugDrawer(this, Client.GameInstance.Game, Client.Player);

            };
            Client.WaitForConnection();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

            //On close, try to update the window position
            if (CrossDomainObject.Instance.SandboxingEnabled)
                try
                {
                    CrossDomainObject.Instance.WindowPositionX = Window.Position.X;
                    CrossDomainObject.Instance.WindowPositionY = Window.Position.Y;
                    CrossDomainObject.Instance.WindowWidth = _graphics.PreferredBackBufferWidth;
                    CrossDomainObject.Instance.WindowHeight = _graphics.PreferredBackBufferHeight;
                }
                catch { }
            else Environment.Exit(0); //Force a graceful failure
        }

        private void KeyboardEvents_KeyPressed(object sender, Starbound.Input.KeyboardEventArgs e)
        {
            if (e.Key == Keys.F11)
            {
                _graphics.ToggleFullScreen();
                GameSettings.Instance.Fullscreen.Value = _graphics.IsFullScreen;
            }
            if (e.Key == Keys.F12)
                DebugDrawer.DebugEnabled = !DebugDrawer.DebugEnabled;
            if (e.Key == Keys.F10)
                DebugDrawer.DrawGraphDebug = !DebugDrawer.DrawGraphDebug;
            if (e.Key == Keys.F9)
                DebugDrawer.DrawTextDebug = !DebugDrawer.DrawTextDebug;
            if (e.Key == Keys.F8)
                DebugDrawer.DebugOverlayGraphsVertical = !DebugDrawer.DebugOverlayGraphsVertical;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Check for GD changes 
            //It's done here because applychanges can cause issues
            //when called repeatedly - Window Resize causes a stack overflow
            if (_graphicsDeviceIsDirty)
            {
                _graphics.ApplyChanges();
                _graphicsDeviceIsDirty = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.OemTilde))
                CreateGame(); //Start anew

            if (Client.GameInstance.Game != null && Client.GameInstance.Game.Running)
            {
                Diagnostics.BeginMeasurement("Input processing");
                if (IsActive)
                    _player?.Tank?.Input(InputDriver.GetInputState());
                Diagnostics.EndMeasurement("Input processing");
            }


            if (_player.Tank != null && SoundPlayer != null)
            {
                SoundPlayer.PlayerPosition = _player.Tank.Position;
                SoundPlayer.PlayerVelocity = _player.Tank.LinearVelocity;
            }
            SoundPlayer?.Update(gameTime);
            Client.GameInstance.Game?.Update(gameTime);

            InputDriver.Update(gameTime);

            Diagnostics.BeginMeasurement("Base.Update()");
            base.Update(gameTime);
            Diagnostics.EndMeasurement("Base.Update()");

            if (Client.GameInstance.Game.CountingDown && _ui.PageName != _settingUpPageName)
                _ui.SetPage(_settingUpPageName);

            if (Client.GameInstance.Game.CountingDown)
                _ui.ActiveBinder.TimeRemaining = Client.GameInstance.Game.RemainingCountdownTime;

            if (!Client.GameInstance.Game.CountingDown && _ui.PageName == _settingUpPageName)
                _ui.UIPage = UserInterfacePage.GetEmptyPageInstance();

            _ui.Update(gameTime);

            if (GameSettings.Instance.ForceFullGCEveryFrame)
                GC.Collect(2, GCCollectionMode.Forced, true);
            if (GameSettings.Instance.ForceGen0GCEveryFrame)
                GC.Collect(0, GCCollectionMode.Forced, true);
        }

        RenderTarget2D _worldRenderTarget;
        private float _zoom = 1;
        private Vector2 _currentOffset;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Diagnostics.BeginMeasurement("Rendering");

            GraphicsDevice.SetRenderTarget(_worldRenderTarget);
            GraphicsDevice.Clear(Color.Gray);
            //if we're in game
            //check if we need to remake the rendertarget
            if (GraphicsDevice.Viewport.Width > 0 && GraphicsDevice.Viewport.Height > 0 &&
                (_worldRenderTarget == null ||
                    _worldRenderTarget.Width != GraphicsDevice.Viewport.Width ||
                    _worldRenderTarget.Height != GraphicsDevice.Viewport.Height))
            {
                if (_worldRenderTarget != null)
                    _worldRenderTarget.Dispose();
                //recreate with correct size
                _worldRenderTarget = new RenderTarget2D(
                    GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            }

            //set the render target

            if (Client.GameInstance.Game != null)
            {
                RectangleF drawRect = new RectangleF(0, 0, 1, 1);
                if (_player.Tank != null)
                {
                    _currentOffset =
                        Vector2.Lerp(_currentOffset, _player.Tank.LinearVelocity / 2,
                        2f * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (_currentOffset.X > 10)
                        _currentOffset.X = 10;
                    else if (_currentOffset.X < -10)
                        _currentOffset.X = -10;
                    else if (_currentOffset.Y > 10)
                        _currentOffset.Y = 10;
                    else if (_currentOffset.Y < -10)
                        _currentOffset.Y = -10;

                    var targetZoom = 0.75f;
                    targetZoom += 1.25f * (_player.Tank.LinearVelocity.Length() / 100f);
                    targetZoom *= GameSettings.Instance.Zoom;

                    _zoom = MathHelper.Lerp(_zoom, targetZoom,
                        2f * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    var offset = _currentOffset * _zoom + _player.Tank.Position;

                    var widthHeightRelative =
                        (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;

                    drawRect = new RectangleF(
                        offset.X -
                        ((20 * widthHeightRelative) * _zoom),
                        offset.Y - (20 * _zoom),
                        (40 * widthHeightRelative) * _zoom,
                        40 * _zoom);
                }
                Diagnostics.BeginMeasurement("World rendering", "Rendering");

                GameRenderer.View = drawRect;
                GameRenderer.Target = _worldRenderTarget;
                GameRenderer.Draw(gameTime);

                Diagnostics.EndMeasurement("World rendering", "Rendering");
                //And draw to screen
                Diagnostics.BeginMeasurement("Copy to screen", "Rendering");

                GraphicsDevice.SetRenderTarget(null);
                _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied,
                    SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone);
                _spriteBatch.Draw(_worldRenderTarget, GraphicsDevice.Viewport.Bounds, Color.White);
                _spriteBatch.End();

                Diagnostics.EndMeasurement("Copy to screen", "Rendering");
            }

            Diagnostics.BeginMeasurement("Draw debug text", "Rendering");
            DebugDrawer.DrawDebugInfo(gameTime);
            Diagnostics.EndMeasurement("Draw debug text", "Rendering");

            _ui.Draw(gameTime);

            base.Draw(gameTime);
            Diagnostics.EndMeasurement("Rendering");
        }
    }
}
