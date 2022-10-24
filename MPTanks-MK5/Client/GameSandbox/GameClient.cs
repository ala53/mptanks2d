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
using System.Linq;
#endregion

namespace MPTanks.Client.GameSandbox
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameClient : Game
    {
        GraphicsDeviceManager _graphics;
        private BasicEffect _fx;
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
        public CrossProcessStartData AOTConfig { get; private set; }
        private UserInterface _ui;
        private AsyncModLoader _modLoader;
        internal DebugDrawer DebugDrawer { get; private set; }
        public Tank CurrentViewedTank { get; private set; }

        const string _settingUpPageName = "SettingUpPrompt";

        private Diagnostics _tmpDiagnosticsInstance = new Diagnostics();
        public Diagnostics Diagnostics => Client?.GameInstance?.Diagnostics ?? _tmpDiagnosticsInstance;

        #region Low level monogame and related initialization
        public GameClient(CrossProcessStartData aotConfig)
            : base()
        {
            AOTConfig = aotConfig;

            _graphics = new GraphicsDeviceManager(this);
            _graphics.HardwareModeSwitch = false;
            Content.RootDirectory = "assets/mgcontent";
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

            _graphics.PreferMultiSampling = true;
            _graphics.IsFullScreen = GameSettings.Instance.Fullscreen;
            _graphics.DeviceCreated += graphics_DeviceCreated;

            Window.AllowUserResizing = true;

            IsMouseVisible = true;

            IsFixedTimeStep = false;
            Window.Title = "MP Tanks 2D: " + TitleCard.Option();
        }

        void graphics_DeviceCreated(object sender, EventArgs e)
        {
            //Set startup properties from the parent's crossdomainobject
            _graphics.PreferredBackBufferWidth = AOTConfig.WindowWidth;
            _graphics.PreferredBackBufferHeight = AOTConfig.WindowHeight;
            Window.Position = new Point(
                AOTConfig.WindowPositionX,
                AOTConfig.WindowPositionY);

            _graphics.SynchronizeWithVerticalRetrace = GameSettings.Instance.VSync;
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
            Starbound.Input.MouseEvents.ButtonClicked += MouseEvents_ButtonClicked;

            base.Initialize();
        }

        private void MouseEvents_ButtonClicked(object sender, Starbound.Input.MouseButtonEventArgs e)
        {
            if (Client?.Player?.Tank != null && !Client.Player.Tank.Alive && e.Button == Starbound.Input.MouseButton.Left)
            {
                SpectateRandomTank();
            }
        }

        private void SpectateRandomTank()
        {
            var players = Client.Game.Players.Where(a => a.Tank != null && a.Tank.Alive);
            if (players.Count() == 0) return;
            CurrentViewedTank = players
                .ElementAt(new Random().Next(0, players.Count())).Tank;
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
            if (!ZSB.DrmClient.LoggedIn && !GlobalSettings.Instance.NoLoginMode)
            {
                Logger.Fatal("Not logged in. Panicking at the disco. Did you launch the game executable by accident?");
                Exit();
                return;
            }

            _fx = new BasicEffect(GraphicsDevice);

            //Create the user interface
            _ui = new UserInterface(this);

            //Initialize the input driver from the configuration
            InputDriver = InputDriverBase.GetDriver(GameSettings.Instance.InputDriverName, this);
            if (GameSettings.Instance.InputKeyBindings.Value != null)
                InputDriver.SetKeyBindings(GameSettings.Instance.InputKeyBindings);

            ShowSetupPrompt();
            //Initialize renderers
            GameRenderer = new GameCoreRenderer(this, GameSettings.Instance.AssetSearchPaths, new[] { 0 }, new NLogLogger(Logger.Instance));
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
                if (p.State<string>("Button") == null)
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
                Engine.Gamemodes.Gamemode.ReflectiveInitialize("CoreAssets+DeathMatchGamemode"),
                Modding.ModLoader.LoadedMods["core-assets.mod"].GetAsset("testmap.json"),
                EngineSettings.GetInstance()
                );

            //game.Timescale = GameCore.TimescaleValue.OneSixteenth;
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
                //TODO: Remove this DUMMY PLAYER

                /*






                Whitespace is here to make this super obvious
                We auto add a dummy player, but we NEED to remove it before shipping.








                */
                for (var i = 0; i < 10; i++)
                    Server.AddPlayer(new ServerPlayer(Server, new NetworkPlayer
                    {
                        Username = "RRRRR",
                        UniqueId = Guid.NewGuid(),
                        SelectedTankReflectionName = "BasicTankMPCopy",
                        IsReady = true
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
            if (AOTConfig.SandboxingEnabled)
                try
                {
                    AOTConfig.WindowPositionX = Window.Position.X;
                    AOTConfig.WindowPositionY = Window.Position.Y;
                    AOTConfig.WindowWidth = _graphics.PreferredBackBufferWidth;
                    AOTConfig.WindowHeight = _graphics.PreferredBackBufferHeight;
                }
                catch { }
            else Environment.Exit(0); //Force a graceful failure
        }

        private void KeyboardEvents_KeyPressed(object sender, Starbound.Input.KeyboardEventArgs e)
        {
            if ((e.Key == Keys.Enter && e.Modifiers.HasFlag(Starbound.Input.Modifiers.Alt)) || e.Key == Keys.F11)
            {
                GameSettings.Instance.Fullscreen.Value = !_graphics.IsFullScreen;
                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                _graphicsDeviceIsDirty = true;
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


        private bool _spectateHasSwitchedTank;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Update(GameTime gameTime)
        {
            _ui.Update(gameTime);
            SoundPlayer.Update(gameTime);

            if (!_hasInitialized || _closing) return;

            if (CurrentViewedTank != null && SoundPlayer != null)
            {
                SoundPlayer.PlayerPosition = CurrentViewedTank.Position;
                SoundPlayer.PlayerVelocity = CurrentViewedTank.LinearVelocity;
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
            else if (!_hasExecutedModLoaderInit)
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

            if (AOTConfig.IsGameHost)
                Server.Update(gameTime);

            Client.Update(gameTime);
            if (Client?.Player?.Tank != null && Client.Player.Tank.Alive)
                CurrentViewedTank = Client.Player.Tank;

            if (_isInPauseMenu)
                return; //Don't mess with the pause menu

            if (Client.IsInTankSelection)
            {
                DeactivateGameInput();
                _ui.GoToPageIfNotThere("tankselectionpromptwithcountdown", page =>
                {
                    page.Element<Button>("UnReadyButton").Click += (a, b) =>
                    {
                        page.Element<Button>("ConfirmButton").Visibility = EmptyKeys.UserInterface.Visibility.Visible;
                        page.Element<StackPanel>("tankselectionarea").Visibility = EmptyKeys.UserInterface.Visibility.Visible;
                        page.Element<StackPanel>("readyarea").Visibility = EmptyKeys.UserInterface.Visibility.Collapsed;
                        Client.PlayerIsReady = false;
                    };
                    page.Element<Button>("ConfirmButton").Click += (a, b) =>
                    {
                        page.Element<Button>("ConfirmButton").Visibility = EmptyKeys.UserInterface.Visibility.Collapsed;
                        page.Element<StackPanel>("tankselectionarea").Visibility = EmptyKeys.UserInterface.Visibility.Collapsed;
                        page.Element<StackPanel>("readyarea").Visibility = EmptyKeys.UserInterface.Visibility.Visible;

                        Client.PlayerIsReady = true;
                    };
                }, (page, state) =>
                {
                    page.Element<TextBlock>("Subscript").Text =
                        page.State<double>("RemainingCountdown").ToString("N0") + " seconds remaining " +
                        (Client.PlayerIsReady ? "until start" : "to choose");

                    //And update tank options
                    var options = page.Element<StackPanel>("tankoptions");
                    if (page.State<string[]>("TankOptions") != null)
                        foreach (var opt in page.State<string[]>("TankOptions"))
                        {
                            if (options.Children.FirstOrDefault(a => a.Name == "opt_" + opt) != null)
                            {
                                //already in there
                                //so we do nothing
                            }
                            else
                            {
                                string reflectionName = opt; //Copy to avoid problems with closures
                                var info = Engine.Helpers.ReflectionHelper.GetGameObjectInfo(reflectionName);
                                if (!info.Exists) continue; //If the type doesn't exist, continue without showing it
                                                            //not in there, so make it
                                var btn = new Button();
                                //Content is a stack panel
                                var stackPnl = new StackPanel();
                                stackPnl.Orientation = EmptyKeys.UserInterface.Orientation.Vertical;
                                stackPnl.HorizontalAlignment = EmptyKeys.UserInterface.HorizontalAlignment.Stretch;
                                stackPnl.Children.Add(new TextBlock
                                {
                                    FontFamily = new EmptyKeys.UserInterface.Media.FontFamily("JHUF"),
                                    FontSize = 20,
                                    Foreground = EmptyKeys.UserInterface.Media.Brushes.White,
                                    Text = info.DisplayName,
                                    Margin = new EmptyKeys.UserInterface.Thickness(0, 5, 0, 0),
                                    Width = 350,
                                    HorizontalAlignment = EmptyKeys.UserInterface.HorizontalAlignment.Stretch
                                });
                                stackPnl.Children.Add(new TextBlock
                                {
                                    FontFamily = new EmptyKeys.UserInterface.Media.FontFamily("JHUF"),
                                    FontSize = 16,
                                    Foreground = EmptyKeys.UserInterface.Media.Brushes.White,
                                    Margin = new EmptyKeys.UserInterface.Thickness(0, 5, 0, 5),
                                    Text = UserInterface.SplitStringIntoLines(info.DisplayDescription, 40),
                                    HorizontalAlignment = EmptyKeys.UserInterface.HorizontalAlignment.Center
                                });

                                btn.Background = EmptyKeys.UserInterface.Media.Brushes.Gray;
                                btn.Content = stackPnl;
                                btn.Name = "opt_" + reflectionName;
                                btn.HorizontalAlignment = EmptyKeys.UserInterface.HorizontalAlignment.Center;
                                btn.Width = 400;
                                btn.Click += (a, b) =>
                                {
                                    options.Children.Select(c => (c as Button).Background = EmptyKeys.UserInterface.Media.Brushes.Transparent);
                                    btn.Background = EmptyKeys.UserInterface.Media.Brushes.Green;

                                    Client.SelectTank(reflectionName);
                                };
                                options.Children.Add(btn);
                            }
                        }
                });
                if (Client.Player != null)
                    _ui.UpdateState(new
                    {
                        TankOptions = Client.Player.AllowedTankTypes,
                        RemainingCountdown = Client.RemainingCountdownTime.TotalSeconds
                    });

            }
            else if (Client.IsInGame && !Client.Game.Ended && !_ui.IsOnEmpty())
            {
                _ui.UnwindAndEmpty();
            }
            else if (Client.IsInGame)
            {
                ActivateGameInput();

                //Handle spectating
                if (!_spectateHasSwitchedTank && CurrentViewedTank != null && !CurrentViewedTank.Alive)
                {
                    Client.Game.TimerFactory.CreateTimer((t) =>
                    {
                        SpectateRandomTank();
                        _spectateHasSwitchedTank = false;
                    },
                        TimeSpan.FromSeconds(3));
                    _spectateHasSwitchedTank = true;
                }
                if (Client.Game.Ended)
                {
                    _ui.GoToPageIfNotThere("gameendedpage", page => { }, (page, state) =>
                    {
                        var winningTeam = Client.Game.Gamemode.WinningTeam;
                        bool winner = (Client.Player?.Tank.Team == winningTeam);

                        page.Element<TextBlock>("Header").Text = winner ? "You're Winner" : "You Tried";
                        if (winningTeam == Engine.Gamemodes.Team.Indeterminate)
                            page.Element<TextBlock>("Subscript").Text = "It's a draw";
                        else if (winningTeam == Engine.Gamemodes.Team.Null)
                            page.Element<TextBlock>("Subscript").Text = "This gamemode has a bug..."; //Wow, so much faith in the game's implementation
                        else
                            page.Element<TextBlock>("Subscript").Text = winningTeam.TeamName + " won";

                        page.Element<Image>("Star").Visibility =
                            winner ? EmptyKeys.UserInterface.Visibility.Collapsed :
                            EmptyKeys.UserInterface.Visibility.Visible;

                        if (winningTeam?.Players != null)
                            page.Element<TextBlock>("PlayerList").Text = string.Join("\n", winningTeam.Players.Select(a => a.Username));
                    }, new { });
                    _ui.UpdateState(new object());
                }
            }
            else
            {
                DeactivateGameInput();
                if (!_ui.IsOnPage("settingupprompt"))
                    ShowSetupPrompt();
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

            if (GameSettings.Instance.ForceFullGCEveryFrame)
                GC.Collect(2, GCCollectionMode.Forced, true);
        }

        RenderTarget2D _worldRenderTarget;
        private float _currentMotionZoomLevel = 1;
        private Vector2 _currentCameraSwingOffset;
        public float _ssaaRate = 1.25f;
        private static readonly Vector2 _viewRectangle = new Vector2(70, 70);
        private static readonly Vector2 _halfViewRectangleSize = _viewRectangle / 2f;
        private static readonly Vector2 _quarterViewRectangleSize = _viewRectangle / 4f;
        int _debugDiagnosticsCounter = 0;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Check for GD changes 
            //It's done here because applychanges can cause issues
            //when called repeatedly - Window Resize causes a stack overflow
            //Monogame is dumb :'(
            if (_graphicsDeviceIsDirty)
            {
                if (Window.ClientBounds.Width < 800)
                    _graphics.PreferredBackBufferWidth = 800;
                if (Window.ClientBounds.Height < 480)
                    _graphics.PreferredBackBufferHeight = 480;
                _graphics.ApplyChanges();
                _graphicsDeviceIsDirty = false;
            }

            if (!IsActive || !_hasInitialized) return; //No need to draw if we are not in focus or haven't initialized
            Diagnostics.BeginMeasurement("Rendering");

            //Write diagnostics to file every 300 frames in debug mode (for perf monitoring)
            _debugDiagnosticsCounter++;
            if (_debugDiagnosticsCounter == 300 && GlobalSettings.Debug) 
            {
                _debugDiagnosticsCounter = 0;
                Logger.Error(Diagnostics.ToString());
            }

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
                DrawPointingDirectionTriangle();
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

        private void DrawPointingDirectionTriangle()
        {
            if (CurrentViewedTank == null || !CurrentViewedTank.Alive) return;
            //This code does exactly what it sounds like.
            //It draws an arrow to show which direction the cursor is pointing.
            //It's here because turning has a delayed rection (intentional) so we wanna
            //show where the turret is going to end up.

            //First, get the view rectangle
            var viewRect = ComputeDrawRectangle(CurrentViewedTank);

            //Begin drawing in that area
            //_spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null,
            //   Matrix.CreateOrthographicOffCenter(viewRect.Left, viewRect.Right, viewRect.Bottom, viewRect.Top, -1, 1));

            var tankPos = CurrentViewedTank.Position;
            var lookPoint = InputDriver.GetInputState().LookDirection - MathHelper.PiOver2;
            //Check if we're spectating
            if (CurrentViewedTank != Client?.Player?.Tank)
                lookPoint = CurrentViewedTank.InputState.LookDirection - MathHelper.PiOver2;

            //Radius of circle = 5
            //And solve the triangle
            var cursorPos = new Vector2(
                5f * (float)Math.Cos(lookPoint),
                5f * (float)Math.Sin(lookPoint)
                ) + tankPos;

            //
            //     0,.5
            // 
            //-0.5,-.5|0,-0.5|.5,-.5
            //Triangle points
            var pts = new[] {
                new Vector2(0,0.5f ),
                new Vector2(-0.5f),
                new Vector2(.5f, -.5f)
            };

            pts = pts.Select(a => RotatePoint(a, lookPoint - MathHelper.PiOver2, Vector2.Zero) + cursorPos).ToArray();

            _fx.Alpha = 0.33f;
            _fx.VertexColorEnabled = true;
            _fx.World = Matrix.CreateOrthographicOffCenter(viewRect.Left, viewRect.Right, viewRect.Bottom, viewRect.Top, -1, 1);
            foreach (var pass in _fx.CurrentTechnique.Passes)
            {
                pass.Apply();
                var vpc = pts.Select(a => new VertexPositionColor(new Vector3(a, 0), Color.Red)).ToArray();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vpc, 0, 1);
            }
        }

        private Vector2 RotatePoint(Vector2 point, float amount, Vector2 center)
        {
            point -= center;
            float sin = (float)Math.Sin(amount), cos = (float)Math.Cos(amount);
            var rotatedX = point.X * cos - point.Y * sin;
            var rotatedY = point.X * sin + point.Y * cos;
            var centered = new Vector2(rotatedX, rotatedY);
            var transformed = centered + center;
            return transformed;
            //return new Vector2(
            //    (float)(center.X + (pt.X - center.X) * Math.Cos(amount) - (pt.Y - center.Y) * Math.Sin(amount)),
            //    (float)(center.Y + (pt.Y - center.Y) * Math.Sin(amount) + (pt.X - center.X) * Math.Cos(amount))
            //    );
        }
    }
}
