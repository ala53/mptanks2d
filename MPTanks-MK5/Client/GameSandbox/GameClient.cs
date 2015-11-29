#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MPTanks.Engine;
using System.Runtime;
using MPTanks.Client.Backend.UI;
using MPTanks.Engine.Core;
using MPTanks.Engine.Settings;
using MPTanks.Engine.Logging;
using MPTanks.Networking.Common.Game;
using MPTanks.Networking.Common;
using MPTanks.Client.GameSandbox.Mods;
using MPTanks.Client.Backend.Renderer;
using MPTanks.Client.GameSandbox.Input;
using MPTanks.Client.GameSandbox.UI;
using MPTanks.Networking.Server;
using System.Diagnostics;
using System.Threading.Tasks;
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
        public MPTanks.Networking.Client.Client Client { get; private set; }
        public MPTanks.Networking.Server.Server Server { get; private set; }
        public Backend.Sound.SoundPlayer SoundPlayer { get; private set; }
        public InputDriverBase InputDriver { get; private set; }
        public GameCoreRenderer GameRenderer { get; private set; }
        private UserInterface _ui;
        private AsyncModLoader _modLoader;
        internal DebugDrawer DebugDrawer { get; private set; }

        const string _settingUpPageName = "SettingUpPrompt";

        private Diagnostics _tmpDiagnosticsInstance = new Diagnostics();
        public Diagnostics Diagnostics => Client?.GameInstance?.Diagnostics ?? _tmpDiagnosticsInstance;

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

            _modLoader = AsyncModLoader.Create(GameSettings.Instance);
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
            _graphics.IsFullScreen = GameSettings.Instance.Fullscreen;
            _ssaaRate = GameSettings.Instance.SSAARate;

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

            _ui = new UserInterface(this);

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
        }

        private void CreateGame()
        {
            var game = new GameCore(
                new NLogLogger(Logger.Instance),
                Engine.Gamemodes.Gamemode.ReflectiveInitialize("DeathMatchGamemode"),
                Modding.ModLoader.LoadedMods["core-assets.mod"].GetAsset("testmap.json"),
                EngineSettings.GetInstance()
                );
            game.Authoritative = true;
            game.FriendlyFireEnabled = true;

            var host = CrossDomainObject.Instance.IsGameHost;

            //Log in
            ZSB.DrmClient.Initialize();
            ZSB.DrmClient.Login("test@zsbgames.me", "drowssap");

            //_ui.GoToPage(_settingUpPageName);
            //TEMP
            Client = new Networking.Client.Client(
                host ? "localhost" : CrossDomainObject.Instance.ServerIp,
                host ? (ushort)33132 : CrossDomainObject.Instance.ServerPort
                , new NLogLogger(Logger.Instance), "password");
            if (CrossDomainObject.Instance.IsGameHost)
                Server = new Networking.Server.Server(new Networking.Server.Configuration()
                {
                    MaxPlayers = 32,
                    Password = "password",
                    Port = 33132,
                    StateSyncRate = TimeSpan.FromMilliseconds(1000)
                }, game, true, new NLogLogger(Logger.Instance));

            GameRenderer = new GameCoreRenderer(this, game, GameSettings.Instance.AssetSearchPaths, new[] { 0 });
            SoundPlayer = new Backend.Sound.SoundPlayer();
            Client.GameInstance.GameChanged += delegate
            {
                GameRenderer.Game = Client.GameInstance.Game;
                SoundPlayer.Game = Client.GameInstance.Game;

            };
            GameRenderer.Game = Client.GameInstance.Game;
            SoundPlayer.Game = Client.GameInstance.Game;
            DebugDrawer?.Dispose();
            DebugDrawer = new DebugDrawer(this, Client);

            if (CrossDomainObject.Instance.IsGameHost)
                for (var i = 0; i < 5; i++)
                {
                    Server.AddPlayer(new ServerPlayer(Server,
                                   new NetworkPlayer()
                                   {
                                   }));
                }
            //Client.GameInstance.FullGameState = FullGameState.Create(Server.GameInstance.Game);

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
            if (e.Key == Keys.F7)
                if (Debugger.IsAttached) Debugger.Break();
            if (e.Key == Keys.M)
                Client.MessageProcessor.SendMessage(new Networking.Common.Actions.ToServer.RequestFullGameStateAction());
            if (e.Key == Keys.F6)
                Client.MessageProcessor.Diagnostics.Reset();
        }

        bool _hasExecutedPostModLoadTask = false;

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

            if (Client?.Player?.Tank != null && SoundPlayer != null)
            {
                SoundPlayer.PlayerPosition = Client.Player.Tank.Position;
                SoundPlayer.PlayerVelocity = Client.Player.Tank.LinearVelocity;
            }
            SoundPlayer?.Update(gameTime);

            Diagnostics.BeginMeasurement("Base.Update() & UI Update");
            _ui.Update(gameTime);
            base.Update(gameTime);
            Diagnostics.EndMeasurement("Base.Update() & UI Update");

            if (Client != null && !Client.IsInCountdown)
            {
                _ui.UnwindAndEmpty();
            }
            if (_modLoader.Running)
            {
                return;
            }
            else if (!_hasExecutedPostModLoadTask)
            {
                _hasExecutedPostModLoadTask = true;
                CreateGame();
            }

            InputDriver.Update(gameTime);
            var state = InputDriver.GetInputState();
            if (state != Client.Input)
                Client.Input = state;

            if (Client.IsInCountdown)
            {
             }


            if (CrossDomainObject.Instance.IsGameHost)
                Server.Update(gameTime);
            //     if (Keyboard.GetState().IsKeyDown(Keys.M))
            //Server.GameInstance.FullGameState.Apply(Client.GameInstance.Game);
            Client.Update(gameTime);
            //Client.GameInstance.Game.Authoritative = true;

            if (GameSettings.Instance.ForceFullGCEveryFrame)
                GC.Collect(2, GCCollectionMode.Forced, true);
            if (GameSettings.Instance.ForceGen0GCEveryFrame)
                GC.Collect(0, GCCollectionMode.Forced, true);
        }

        RenderTarget2D _worldRenderTarget;
        private float _currentMotionZoomLevel = 1;
        private Vector2 _currentCameraSwingOffset;
        public float _ssaaRate = 1.25f;
        private static readonly Vector2 _viewRectangle = new Vector2(40, 40);
        private static readonly Vector2 _halfViewRectangleSize = _viewRectangle / 2f;
        private static readonly Vector2 _quarterViewRectangleSize = _viewRectangle / 4f;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Diagnostics.BeginMeasurement("Rendering");
            GraphicsDevice.SetRenderTarget(null);

            var computedRenderSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) * _ssaaRate;
            //if we're in game
            //check if we need to remake the rendertarget
            if (GraphicsDevice.Viewport.Width > 0 && GraphicsDevice.Viewport.Height > 0)
            {
                if (_worldRenderTarget == null || _worldRenderTarget.Width != computedRenderSize.X ||
                    _worldRenderTarget.Height != computedRenderSize.Y)
                {
                    _worldRenderTarget?.Dispose();
                    //recreate with correct size
                    _worldRenderTarget = new RenderTarget2D(
                        GraphicsDevice, (int)computedRenderSize.X, (int)computedRenderSize.Y);
                }
            }
            GraphicsDevice.SetRenderTarget(_worldRenderTarget);
            GraphicsDevice.Clear(Client?.Game?.Map?.BackgroundColor ?? Color.Black);

            //set the render target

            if (Client?.Game != null)
            {
                RectangleF computedDrawRectangle = new RectangleF(0, 0, 1, 1);
                if (Client.Player?.Tank != null)
                {
                    _currentCameraSwingOffset =
                        Vector2.Lerp(_currentCameraSwingOffset, Client.Player.Tank.LinearVelocity / 2,
                        2f * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (_currentCameraSwingOffset.X > _quarterViewRectangleSize.X)
                        _currentCameraSwingOffset.X = _quarterViewRectangleSize.X;
                    else if (_currentCameraSwingOffset.X < -_quarterViewRectangleSize.X)
                        _currentCameraSwingOffset.X = -_quarterViewRectangleSize.X;
                    else if (_currentCameraSwingOffset.Y > _quarterViewRectangleSize.Y)
                        _currentCameraSwingOffset.Y = _quarterViewRectangleSize.Y;
                    else if (_currentCameraSwingOffset.Y < -_quarterViewRectangleSize.Y)
                        _currentCameraSwingOffset.Y = -_quarterViewRectangleSize.Y;

                    var targetMotionZoomLevel = 0.75f;
                    targetMotionZoomLevel += 1.25f * (Client.Player.Tank.LinearVelocity.Length() / 100f);
                    targetMotionZoomLevel *= GameSettings.Instance.Zoom;

                    _currentMotionZoomLevel = MathHelper.Lerp(_currentMotionZoomLevel, targetMotionZoomLevel,
                        2f * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    var calculatedWorldOffsetCenter = _currentCameraSwingOffset * _currentMotionZoomLevel + Client.Player.Tank.Position;

                    var aspectRatio =
                        (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;

                    computedDrawRectangle = new RectangleF(
                        calculatedWorldOffsetCenter.X -
                        ((_halfViewRectangleSize.X * aspectRatio) * _currentMotionZoomLevel),
                        calculatedWorldOffsetCenter.Y - (_halfViewRectangleSize.Y * _currentMotionZoomLevel),
                        (_viewRectangle.X * aspectRatio) * _currentMotionZoomLevel,
                        _viewRectangle.Y * _currentMotionZoomLevel);
                }
                Diagnostics.BeginMeasurement("World rendering", "Rendering");

                GameRenderer.Game = Client.Game;
                GameRenderer.View = computedDrawRectangle;
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
            DebugDrawer?.DrawDebugInfo(gameTime);
            Diagnostics.EndMeasurement("Draw debug text", "Rendering");
            //And render the draw
            _ui.Draw(gameTime);

            base.Draw(gameTime);
            Diagnostics.EndMeasurement("Rendering");
        }
    }
}
