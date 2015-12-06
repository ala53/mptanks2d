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
using MPTanks.Networking.Client;
using System.Diagnostics;
using System.Threading.Tasks;
using EmptyKeys.UserInterface.Controls;
using MPTanks.Client.Backend.Sound;
using MPTanks.Engine.Tanks;
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
        private bool _closing;
        private bool _isInPauseMenu;
        private bool _hasExecutedModLoaderInit;
        SpriteBatch _spriteBatch;
        public NetClient Client { get; private set; }
        public Server Server { get; private set; }
        public SoundPlayer SoundPlayer { get; private set; }
        public InputDriverBase InputDriver { get; private set; }
        public GameCoreRenderer GameRenderer { get; private set; }
        public bool IsHost => AOTConfig.IsGameHost;
        /// <summary>
        /// The ahead of time configuration / Cross domain shared object
        /// </summary>
        public CrossDomainObject AOTConfig => CrossDomainObject.Instance;
        private UserInterface _ui;
        private AsyncModLoader _modLoader;
        internal DebugDrawer DebugDrawer { get; private set; }
        public Tank CurrentViewedTank => Client?.Player?.Tank;

        const string _settingUpPageName = "SettingUpPrompt";

        private Diagnostics _tmpDiagnosticsInstance = new Diagnostics();
        public Diagnostics Diagnostics => Client?.GameInstance?.Diagnostics ?? _tmpDiagnosticsInstance;

        #region Low level monogame and related initialization
        public GameClient()
            : base()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "assets/mgcontent";
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

            _graphics.PreferMultiSampling = true;
            _graphics.DeviceCreated += graphics_DeviceCreated;

            Window.AllowUserResizing = true;

            IsMouseVisible = true;

            IsFixedTimeStep = false;
            Window.Title = "MP Tanks 2D: " + TitleCard.Option();
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

            //Initialize evented input layer
            Components.Add(new Starbound.Input.KeyboardEvents(this));
            Components.Add(new Starbound.Input.MouseEvents(this));
            Components.Add(new Starbound.Input.GamePadEvents(PlayerIndex.One, this));

            Starbound.Input.KeyboardEvents.KeyPressed += KeyboardEvents_KeyPressed;

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
            // Create a new SpriteBatch to draw to the screen.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            InitializeGame();
        }
        #endregion
        private bool _hasInitialized;
        /// <summary>
        /// Put init logic here. Will be called after LoadContent()
        /// </summary>
        private void InitializeGame()
        {
            //Log in to ZSB Servers
            try
            { ZSB.DrmClient.Initialize(GlobalSettings.Instance.StoredAccountInfo); }
            catch
            { ZSB.DrmClient.Initialize(); } //If an error occurs, clear info and restart
            ZSB.DrmClient.OnPersistentStorageChanged +=
                (a, b) => GlobalSettings.Instance.StoredAccountInfo.Value = ZSB.DrmClient.PersistentData;
            //Handle fatal login errors
            if (!ZSB.DrmClient.LoggedIn)
            {
                Logger.Fatal("Not logged in. Panicking at the disco. Did you launch the game executable by accident?");
                Exit();
                return;
            }

            //Create the user interface
            _ui = new UserInterface(this);

            //Initialize the input driver from the configuration
            InputDriver = InputDriverBase.GetDriver(GameSettings.Instance.InputDriverName, this);
            if (GameSettings.Instance.InputKeyBindings.Value != null)
                InputDriver.SetKeyBindings(GameSettings.Instance.InputKeyBindings);

            ShowSetupPrompt();
            //Initialize renderers
            GameRenderer = new GameCoreRenderer(this, GameSettings.Instance.AssetSearchPaths, new[] { 0 });
            SoundPlayer = new SoundPlayer();
            //And, finally, Begin the async mod loading
            _modLoader = AsyncModLoader.Create(GameSettings.Instance);
            _hasInitialized = true;
        }

        private void ShowSetupPrompt()
        {
            _ui.GoToPage("settingupprompt", p =>
            {
                p.Element<Button>("controlbutton").Visibility = EmptyKeys.UserInterface.Visibility.Collapsed;
            },
            (p, b) =>
            {
                p.Element<TextBlock>("Header").Text = p.State<string>("Header") ?? "";
                p.Element<TextBlock>("ContentT").Text = p.State<string>("Content") ?? "";

                p.Element<Button>("controlbutton").Content = p.State<string>("Button") ?? "";
                p.Element<Button>("controlbutton").Click += (d, e) => Exit();
                if (p.State<string>("Header") == null)
                    p.Element<Button>("controlbutton").Visibility = EmptyKeys.UserInterface.Visibility.Collapsed;
                else if (p.Element<Button>("controlbutton").Visibility
                        != EmptyKeys.UserInterface.Visibility.Visible) //flicker fix
                    p.Element<Button>("controlbutton").Visibility = EmptyKeys.UserInterface.Visibility.Visible;
            }, new { Header = "Initializing", Content = "If you see this, it's probably bad.", Button = (string)null });
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

            if (IsHost)
            {
                Client = new NetClient(
                    "localhost",
                    33132,
                    new NLogLogger(Logger.Instance),
                    AOTConfig.Password);
                //And set up server, default port
                Server = new Server(new Configuration()
                {
                    MaxPlayers = 32, //Non-configurable -- use dedicated server for more players
                    Password = AOTConfig.Password,
                    Port = 33132,
                    StateSyncRate = TimeSpan.FromMilliseconds(1000)
                }, game, true, new NLogLogger(Logger.Instance));

                Server.AddPlayer(new ServerPlayer(Server, new NetworkPlayer
                {
                    Username = "RRRRR",
                    UniqueId = Guid.NewGuid(),
                }));
            }
            else
            {
                Client = new NetClient(
                    AOTConfig.Ip,
                    AOTConfig.Port,
                    new NLogLogger(Logger.Instance),
                    AOTConfig.Password);
            }

            Client.GameInstance.GameChanged += delegate
            {
                GameRenderer.Game = Client.GameInstance.Game;
                SoundPlayer.Game = Client.GameInstance.Game;
            };
            GameRenderer.Game = Client.GameInstance.Game;
            SoundPlayer.Game = Client.GameInstance.Game;
            //And initialize the debug system
            DebugDrawer?.Dispose();
            DebugDrawer = new DebugDrawer(this, Client);
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
            if (DebugDrawer != null)
            {
                if (e.Key == Keys.F12)
                    DebugDrawer.DebugEnabled = !DebugDrawer.DebugEnabled;
                if (e.Key == Keys.F10)
                    DebugDrawer.DrawGraphDebug = !DebugDrawer.DrawGraphDebug;
                if (e.Key == Keys.F9)
                    DebugDrawer.DrawTextDebug = !DebugDrawer.DrawTextDebug;
                if (e.Key == Keys.F8)
                    DebugDrawer.DebugOverlayGraphsVertical = !DebugDrawer.DebugOverlayGraphsVertical;
            }
            if (e.Key == Keys.F7)
                if (Debugger.IsAttached) Debugger.Break();
            if (e.Key == Keys.F6)
                Client.MessageProcessor.Diagnostics.Reset();

            if (e.Key == Keys.Escape)
            {
                if (_isInPauseMenu) HideMenu();
                else
                {
                    DeactivateGameInput();
                    ShowMenu();
                }
            }
        }
        
        private void ShowMenu()
        {
            if (_isInPauseMenu) return;
            _isInPauseMenu = true;

            _ui.GoToPage("PauseMenu", p =>
            {
                p.Element<Button>("LeaveServerBtn").Click += (a, b) =>
                {
                    if (IsHost) Server.Close();
                    Client.Disconnect();
                    Exit();
                };
            });
        }

        private void HideMenu()
        {
            if (!_isInPauseMenu) return;
            _isInPauseMenu = false;

            _ui.GoBack();
        }

        private void DeactivateGameInput()
        {
            InputDriver?.Deactivate();
            IsMouseVisible = true;
        }
        private void ActivateGameInput()
        {
            InputDriver?.Activate();
            IsMouseVisible = false;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Update(GameTime gameTime)
        {
            _ui.Update(gameTime);
            SoundPlayer.Update(gameTime);

            //Check for GD changes 
            //It's done here because applychanges can cause issues
            //when called repeatedly - Window Resize causes a stack overflow
            if (_graphicsDeviceIsDirty)
            {
                _graphics.ApplyChanges();
                _graphicsDeviceIsDirty = false;
            }

            if (!_hasInitialized || _closing) return;

            if (Client?.Player?.Tank != null && SoundPlayer != null)
            {
                SoundPlayer.PlayerPosition = Client.Player.Tank.Position;
                SoundPlayer.PlayerVelocity = Client.Player.Tank.LinearVelocity;
            }
            SoundPlayer?.Update(gameTime);

            Diagnostics.BeginMeasurement("Base.Update() & UI Update");
            base.Update(gameTime);
            Diagnostics.EndMeasurement("Base.Update() & UI Update");

            if (_modLoader.Running)
            {
                _ui.UpdateState(new
                {
                    Header = "Loading mods...",
                    Content = _modLoader.Status,
                    Button = "Cancel"
                });
                return;
            }
            else if ( !_hasExecutedModLoaderInit)
            {
                _hasExecutedModLoaderInit = true;
                CreateGame();
                return;
            }
            if (!_isInPauseMenu)
            {
                InputDriver.Update(gameTime);
                var state = InputDriver.GetInputState();
                if (Client?.Input != null && state != Client.Input)
                    Client.Input = state;
            }

            if (CrossDomainObject.Instance.IsGameHost)
                Server.Update(gameTime);

            Client.Update(gameTime);

            if (_isInPauseMenu)
                return; //Don't mess with the pause menu

            if (Client.IsInGame && _ui.IsOnPage("settingupprompt"))
            {
                _ui.UnwindAndEmpty();
            }
            else if (Client.IsInGame) { ActivateGameInput(); }
            else
            {
                DeactivateGameInput();
                if (!_ui.IsOnPage("settingupprompt"))
                    ShowSetupPrompt();

                if (Client.IsInCountdown)
                {
                    _ui.UpdateState(new
                    {
                        Header = "Counting down to start...",
                        Content = $"{Client.RemainingCountdownTime.TotalSeconds.ToString("N0")} seconds remaining",
                        Button = "Leave server"
                    });
                }
                else
                {
                    switch (Client.Status)
                    {
                        case NetClient.ClientStatus.Authenticating:
                            _ui.UpdateState(new
                            {
                                Header = "Logging in...",
                                Content = "Authenticating with the ZSB servers",
                                Button = "Cancel"
                            });
                            break;
                        case NetClient.ClientStatus.Connected:
                            _ui.UpdateState(new
                            {
                                Header = "Connected...",
                                Content = "Waiting for the server to respond",
                                Button = "Leave server"
                            });
                            break;
                        case NetClient.ClientStatus.Connecting:
                            _ui.UpdateState(new
                            {
                                Header = "Connecting to the server...",
                                Content = Client.Message,
                                Button = "Abort"
                            });
                            break;
                        case NetClient.ClientStatus.Disconnected:
                            _closing = true;
                            _ui.ShowMessageBox("Disconnected", Client.Message, UserInterface.MessageBoxType.ErrorMessageBox,
                                UserInterface.MessageBoxButtons.Ok, a => Exit());
                            break;
                        case NetClient.ClientStatus.DownloadingMods:
                            _ui.UpdateState(new
                            {
                                Header = "Downloading mods...",
                                Content = "This may take a a while",
                                Button = "Leave server"
                            });
                            break;
                        case NetClient.ClientStatus.Errored:
                            _ui.UpdateState(new
                            {
                                Header = "A fatal error has occured",
                                Content = "",
                                Button = "Set my hair on fire and leave"
                            });
                            break;
                        case NetClient.ClientStatus.NotStarted:
                            _ui.UpdateState(new
                            {
                                Header = "Waiting to connect...",
                                Content = "This shouldn't usually happen",
                                Button = "Stare with contempt"
                            });
                            break;
                    }
                }
            }

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
            if (!IsActive || !_hasInitialized) return; //No need to draw if we are not in focus or haven't initialized
            Diagnostics.BeginMeasurement("Rendering");

            EnsureRenderTargetSizing();

            //set the render target
            GraphicsDevice.SetRenderTarget(_worldRenderTarget);
            GraphicsDevice.Clear(Client?.Game?.Map?.BackgroundColor ?? Color.Black);

            if (Client?.Game != null)
            {
                //Update the draw rectangle
                RectangleF computedDrawRectangle = new RectangleF();
                if (CurrentViewedTank != null)
                {
                    UpdateCameraSwingAndMotionZoom(CurrentViewedTank, gameTime);
                    computedDrawRectangle = ComputeDrawRectangle(CurrentViewedTank);
                }

                Diagnostics.BeginMeasurement("World rendering", "Rendering");

                //Tell the game world renderer what to do
                GameRenderer.Game = Client.Game;
                GameRenderer.View = computedDrawRectangle;
                GameRenderer.Target = _worldRenderTarget;
                GameRenderer.Draw(gameTime);

                Diagnostics.EndMeasurement("World rendering", "Rendering");
                //And draw to screen
                Diagnostics.BeginMeasurement("Copy to screen", "Rendering");

                //Blit to screen
                GraphicsDevice.SetRenderTarget(null);
                _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied,
                    SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone);
                _spriteBatch.Draw(_worldRenderTarget, GraphicsDevice.Viewport.Bounds, Color.White);
                _spriteBatch.End();

                Diagnostics.EndMeasurement("Copy to screen", "Rendering");
            }
            //And draw to the screen
            GraphicsDevice.SetRenderTarget(null);

            Diagnostics.BeginMeasurement("Draw debug text", "Rendering");
            DebugDrawer?.DrawDebugInfo(gameTime);
            Diagnostics.EndMeasurement("Draw debug text", "Rendering");
            //And render the draw
            _ui.Draw(gameTime);

            base.Draw(gameTime);
            Diagnostics.EndMeasurement("Rendering");
        }
        private void EnsureRenderTargetSizing()
        {
            var computedRenderSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) * _ssaaRate;

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
        }
        private void UpdateCameraSwingAndMotionZoom(Tank tank, GameTime gameTime)
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
        }

        private RectangleF ComputeDrawRectangle(Tank tank)
        {
            var aspectRatio =
                (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;
            var calculatedWorldOffsetCenter =
                _currentCameraSwingOffset * _currentMotionZoomLevel + tank.Position;

            return new RectangleF(
                calculatedWorldOffsetCenter.X -
                ((_halfViewRectangleSize.X * aspectRatio) * _currentMotionZoomLevel),
                calculatedWorldOffsetCenter.Y - (_halfViewRectangleSize.Y * _currentMotionZoomLevel),
                (_viewRectangle.X * aspectRatio) * _currentMotionZoomLevel,
                _viewRectangle.Y * _currentMotionZoomLevel);
        }
    }
}
