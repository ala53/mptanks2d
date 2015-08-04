#region Using Statements
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
#endregion

namespace MPTanks.Client.GameSandbox
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameClient : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private NetworkPlayer player1;
        private NetworkPlayer player2;
        private MPTanks.Engine.GameCore game { get; set; }
        private float zoom = 6.5f;
        private SpriteFont font;
        private Stopwatch timer = new Stopwatch();
        private RectangleF drawRect;
        private Screens.Screen currentScreen;
        private UIRoot root;
        private MonoGameEngine eng;
        private Texture2D _helperTexture;
        private Backend.Sound.SoundPlayer _soundPlayer;

        private GameCoreRenderer _gcRenderer;

        private bool _unlockCursor = false;

        private bool _graphicsDeviceIsDirty = false;

        public GameClient()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "assets/mgcontent";
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

            graphics.PreferMultiSampling = true;
            graphics.DeviceCreated += graphics_DeviceCreated;

            Window.AllowUserResizing = true;


            // IsMouseVisible = true;
            IsFixedTimeStep = false;
            // TargetElapsedTime = TimeSpan.FromMilliseconds(66.3333333);

            CoreModLoader.LoadTrustedMods(GameSettings.Instance);
        }

        void graphics_DeviceCreated(object sender, EventArgs e)
        {

            //Set startup properties from crossdomainobject
            graphics.PreferredBackBufferWidth = CrossDomainObject.Instance.WindowWidth;
            graphics.PreferredBackBufferHeight = CrossDomainObject.Instance.WindowHeight;
            Window.Position = new Point(
                CrossDomainObject.Instance.WindowPositionX,
                CrossDomainObject.Instance.WindowPositionY);

            graphics.SynchronizeWithVerticalRetrace = GameSettings.Instance.VSync;

            _graphicsDeviceIsDirty = true;
            //And the UI renderer
            eng = new MonoGameEngine(GraphicsDevice, 800, 480);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            //Set up resize handler
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            //Initialize input driver
            Components.Add(new Starbound.Input.KeyboardEvents(this));
            Components.Add(new Starbound.Input.MouseEvents(this));
            Components.Add(new Starbound.Input.GamePadEvents(PlayerIndex.One, this));

            Starbound.Input.KeyboardEvents.KeyPressed += KeyboardEvents_KeyPressed;
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            _graphicsDeviceIsDirty = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            currentScreen = new Screens.InGameScreen(this);
            SetupGame();
            font = Content.Load<SpriteFont>("font");

            _helperTexture = new Texture2D(GraphicsDevice, 1, 1);
            _helperTexture.SetData(new[] { Color.White });

            root = new EmptyKeys.UserInterface.Generated.CreateAccountPage(800, 480);

        }

        private void SetupGame()
        {
            game = new GameCore(
                new NLogLogger(Logger.Instance),
                Engine.Gamemodes.Gamemode.ReflectiveInitialize("TeamDeathMatchGamemode"),
                Modding.ModLoader.LoadedMods["core-assets.mod"].GetPackedFileString("testmap.json"),
                false,
                new EngineSettings("Engine Settings.json")
                );
            game.Authoritative = true;
            game.FriendlyFireEnabled = true;

            player1 = new NetworkPlayer()
            {
                Id = Guid.NewGuid()
            };
            player2 = new NetworkPlayer()
            {
                Id = Guid.NewGuid()
            };
            game.AddPlayer(player1);
            game.AddPlayer(player2);

            for (var i = 0; i < 5; i++)
            {
                game.AddPlayer(new NetworkPlayer { Id = Guid.NewGuid() });
            }
            if (_gcRenderer != null) _gcRenderer.Dispose();
            _gcRenderer = new GameCoreRenderer(this, game, GameSettings.Instance.AssetSearchPaths, new[] { 0 });
            if (_soundPlayer != null) _soundPlayer.Dispose();
            _soundPlayer = new Backend.Sound.SoundPlayer(game);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            if (!GlobalSettings.Debug)
            {
                try
                {
                    CrossDomainObject.Instance.WindowPositionX = Window.Position.X;
                    CrossDomainObject.Instance.WindowPositionY = Window.Position.Y;
                    CrossDomainObject.Instance.WindowWidth = graphics.PreferredBackBufferWidth;
                    CrossDomainObject.Instance.WindowHeight = graphics.PreferredBackBufferHeight;
                }
                catch { }
            }
        }

        private void KeyboardEvents_KeyPressed(object sender, Starbound.Input.KeyboardEventArgs e)
        {
            if (GlobalSettings.Debug && e.Key == Keys.F7)
                Debugger.Break();

            if (e.Key == Keys.F11)
            {
                graphics.ToggleFullScreen();
            }

            if (e.Key == Keys.RightControl)
            {
                _unlockCursor = !_unlockCursor;
                IsMouseVisible = !IsMouseVisible;
            }

            if (e.Key == Keys.LeftAlt && ((e.Modifiers & Starbound.Input.Modifiers.Control) == Starbound.Input.Modifiers.Control))
            {
                game = FullGameState.Create(game).CreateGameFromState(new NLogLogger(Logger.Instance), null, 0);
                player1 = (NetworkPlayer)game.PlayersById[player1.Id];
                player2 = (NetworkPlayer)game.PlayersById[player2.Id];

                game.Authoritative = true;

                //shouldTick = false;
                _gcRenderer.Dispose();
                _gcRenderer = new GameCoreRenderer(this, game, GameSettings.Instance.AssetSearchPaths, new[] { 0 });

                _soundPlayer = new Backend.Sound.SoundPlayer(game);
            }

            if (e.Key == Keys.N)
                shouldTick = !shouldTick;

            if (e.Key == Keys.M)
                game.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromMilliseconds(15)));

            if (e.Key == Keys.L)
                game.UnsafeTickGameWorld(0.5f);

            if (e.Key == Keys.F12)
                debugEnabled = !debugEnabled;
            if (e.Key == Keys.F10)
                drawGraphDebug = !drawGraphDebug;
            if (e.Key == Keys.F9)
                drawTextDebug = !drawTextDebug;
            if (e.Key == Keys.F8)
                debugOverlayGraphsVertical = !debugOverlayGraphsVertical;

            if (e.Key == Keys.Y)
            {
                timescaleIndex++;

                if (timescaleIndex < 0) timescaleIndex = 0;
                if (timescaleIndex >= GameCore.TimescaleValue.Values.Count)
                    timescaleIndex = GameCore.TimescaleValue.Values.Count - 1;
            }
            if (e.Key == Keys.U)
            {
                timescaleIndex--;
                if (timescaleIndex < 0) timescaleIndex = 0;
                if (timescaleIndex >= GameCore.TimescaleValue.Values.Count)
                    timescaleIndex = GameCore.TimescaleValue.Values.Count - 1;
            }
        }

        bool shouldTick = true;
        private int timescaleIndex = GameCore.TimescaleValue.One.Index;
        private long updateNumber;
        private long frameNumber;
        private float physicsMs = 0;
        private float renderMs = 0;
        private Stopwatch _gameTimer;
        private bool slow = false;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (_gameTimer == null)
                _gameTimer = new Stopwatch();

            var el = _gameTimer.Elapsed;
            _gameTimer.Restart();
            if (el.TotalMilliseconds > 19)
                slow = true;
            else slow = false;

            if (slow && GlobalSettings.Debug)
                Logger.Debug("Last frame was slow according to real time (" + el.TotalMilliseconds + "ms). Last frame was update " + updateNumber + ", frame " + frameNumber + " GameTime says: " +
          gameTime.ElapsedGameTime.TotalMilliseconds + "ms, running slowly: " + gameTime.IsRunningSlowly);

            updateNumber++;

            //Check for GD changes 
            //It's done here because applychanges can cause issues
            //when called repeatedly - Window Resize causes a stack overflow
            if (_graphicsDeviceIsDirty)
            {
                graphics.ApplyChanges();
                _graphicsDeviceIsDirty = false;
            }

            game.Timescale = GameCore.TimescaleValue.Values[timescaleIndex];
            timer.Restart();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.OemTilde))
                SetupGame(); //Start anew

            if (game.Running)
            {
                game.Diagnostics.BeginMeasurement("Input processing");
                var iState = new InputState();
                iState.LookDirection = player1.Tank.Rotation;

                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    iState.MovementSpeed = 1;
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    iState.MovementSpeed = -1;
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    iState.RotationSpeed = -1;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    iState.RotationSpeed = 1;

                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    iState.FirePressed = true;

                game.InjectPlayerInput(player1, iState);

                var iState2 = new InputState();
                iState2.LookDirection = player2.Tank.Rotation;

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    iState2.MovementSpeed = 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    iState2.MovementSpeed = -1;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    iState2.RotationSpeed = -1;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    iState2.RotationSpeed = 1;

                if (Keyboard.GetState().IsKeyDown(Keys.M))
                    iState2.FirePressed = true;

                if (Keyboard.GetState().IsKeyDown(Keys.V))
                {
                    game.ParticleEngine.CreateEmitter(0, new Engine.Assets.SpriteInfo(null, null),
                        Color.Gray, new RectangleF(50, 50, 20, 20), Vector2.One, true, 0, 0, 1000, Vector2.Zero,
                        Vector2.Zero, Vector2.Zero, 0, 0, 500, 10000000);
                }

                iState2.LookDirection = 1.2F;

                game.InjectPlayerInput(player2, iState2);

                //Complicated look state calcuation below
                //var screenCenter = new Vector2(GraphicsDevice.Viewport.Bounds.Width / 2, //vertex
                //    GraphicsDevice.Viewport.Bounds.Height / 2);
                //var mousePos = new Vector2(Mouse.GetState().Position.X,  //point a
                //    Mouse.GetState().Position.Y);
                //var ctr = screenCenter - mousePos;
                //iState.LookDirection = (float)-Math.Atan2(ctr.X, ctr.Y);
                game.Diagnostics.EndMeasurement("Input processing");
            }

            game.Diagnostics.BeginMeasurement("Lock cursor calls");
            if (!_unlockCursor)
                LockCursor();
            game.Diagnostics.EndMeasurement("Lock cursor calls");

            if (Keyboard.GetState().IsKeyDown(Keys.X))
                zoom += 0.1f;
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
                zoom -= 0.1f;


            if (Keyboard.GetState().IsKeyDown(Keys.B))
            {
                Logger.Info(game.Diagnostics.ToString());
            }
            if (shouldTick)
            {
                game.Update(gameTime);
                if (Keyboard.GetState().IsKeyDown(Keys.L))
                    PseudoFullGameWorldState.Create(game).Apply(game);
            }

            if (player1.Tank != null)
            {
                _soundPlayer.PlayerPosition = player1.Tank.Position;
                _soundPlayer.PlayerVelocity = player1.Tank.LinearVelocity;
            }
            _soundPlayer.Update(gameTime);

            game.Diagnostics.BeginMeasurement("Base.Update()");
            base.Update(gameTime);
            game.Diagnostics.EndMeasurement("Base.Update()");
            timer.Stop();
            physicsMs = (float)timer.Elapsed.TotalMilliseconds;
        }

        private void LockCursor()
        {

            const float outAmount = 100;
            var offset = new Vector2(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2);

            var rel = new Vector2(Math.Abs(Mouse.GetState().Position.X - offset.X),
                Math.Abs(Mouse.GetState().Position.Y - offset.Y));
            var relSigned = new Vector2(Mouse.GetState().Position.X - offset.X,
                Mouse.GetState().Position.Y - offset.Y);

            if (rel.X > outAmount || rel.Y > outAmount)
            {
                if (rel.X > rel.Y)
                {
                    var factor = outAmount / rel.X;
                    relSigned *= factor;
                    Mouse.SetPosition((int)(offset.X + relSigned.X), (int)(offset.Y + relSigned.Y));
                }
                else
                {
                    var factor = outAmount / rel.Y;
                    relSigned *= factor;
                    Mouse.SetPosition((int)(offset.X + relSigned.X), (int)(offset.Y + relSigned.Y));
                }
            }
        }

        RenderTarget2D _worldRenderTarget;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            frameNumber++;
            game.Diagnostics.BeginMeasurement("Rendering");
            timer.Restart(); //Stat tracking
            GraphicsDevice.Clear(Color.Black);
            //in game
            if (GraphicsDevice.Viewport.Width > 0 && GraphicsDevice.Viewport.Height > 0 &&
                (_worldRenderTarget == null ||
                    _worldRenderTarget.Width != GraphicsDevice.Viewport.Width ||
                    _worldRenderTarget.Height != GraphicsDevice.Viewport.Height))
            {
                if (_worldRenderTarget != null)
                    _worldRenderTarget.Dispose();

                _worldRenderTarget = new RenderTarget2D(
                    GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            }
            GraphicsDevice.SetRenderTarget(_worldRenderTarget);
            GraphicsDevice.Clear(new Color(120, 120, 120, 255));

            if (player1.Tank != null)
            {
                var widthHeightRelative =
                    (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;
                drawRect = new RectangleF(
                    player1.Tank.Position.X - ((10 * widthHeightRelative) * zoom),
                    player1.Tank.Position.Y - (10 * zoom),
                    (20 * widthHeightRelative) * zoom,
                    20 * zoom);
            }
            game.Diagnostics.BeginMeasurement("World rendering", "Rendering");
            _gcRenderer.View = drawRect;
            _gcRenderer.Target = _worldRenderTarget;
            _gcRenderer.Draw(gameTime);
            game.Diagnostics.EndMeasurement("World rendering", "Rendering");
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate);
            spriteBatch.Draw(_worldRenderTarget, _worldRenderTarget.Bounds, Color.White);
            spriteBatch.End();

            game.Diagnostics.BeginMeasurement("Draw debug text", "Rendering");
            DrawDebugInfo(gameTime);
            game.Diagnostics.EndMeasurement("Draw debug text", "Rendering");

            if (GameSettings.Instance.ForceFullGCEveryFrame)
                GC.Collect(2, GCCollectionMode.Forced, true);
            if (GameSettings.Instance.ForceGen0GCEveryFrame)
                GC.Collect(0, GCCollectionMode.Forced, true);

            root.Draw(gameTime.ElapsedGameTime.TotalMilliseconds);
            game.Diagnostics.BeginMeasurement("Base.Draw()", "Rendering");
            base.Draw(gameTime);
            game.Diagnostics.EndMeasurement("Base.Draw()", "Rendering");
            game.Diagnostics.EndMeasurement("Rendering");
            timer.Stop();
            renderMs = (float)timer.Elapsed.TotalMilliseconds;
        }

        #region Debug info

        private bool debugEnabled = true;
        private bool drawTextDebug = true;
        private bool drawGraphDebug = true;
        private bool debugOverlayGraphsVertical = true;
        private void DrawDebugInfo(GameTime gameTime)
        {
            if (!debugEnabled) return;
            LogDebugInfo(gameTime);
            if (drawTextDebug) DrawTextDebugInfo(gameTime);
            if (drawGraphDebug) DrawGraphDebugInfo(gameTime);
        }

        #region Data Logging
        private float[] _debugFrameTimes;
        private DebugMemoryUsageTick[] _debugMemoryUsages;
        private Stopwatch _frameTimesTimer = Stopwatch.StartNew();
        private void LogDebugInfo(GameTime gameTime)
        {
            if (_debugFrameTimes == null)
            {
                _debugFrameTimes = new float[graphWidth];
                for (var i = 0; i < _debugFrameTimes.Length; i++)
                    _debugFrameTimes[i] = 16.666666666666f;
            }

            if (_debugMemoryUsages == null)
                _debugMemoryUsages = new DebugMemoryUsageTick[graphWidth];

            if ((DateTime.Now - _debugMemoryUsages[_debugMemoryUsages.Length - 1].Measured).TotalMilliseconds > 0)
            {
                //Shift back
                for (var i = 1; i < _debugMemoryUsages.Length; i++)
                    _debugMemoryUsages[i - 1] = _debugMemoryUsages[i];
                //Measure
                var gcData = DebugDetectGC();
                _debugMemoryUsages[_debugMemoryUsages.Length - 1] = new DebugMemoryUsageTick
                {
                    Measured = DateTime.Now,
                    BytesUsed = GC.GetTotalMemory(false),
                    HasGen0GC = gcData.Gen0,
                    HasGen1GC = gcData.Gen1,
                    HasGen2GC = gcData.Gen2
                };
            }

            for (var i = 1; i < _debugFrameTimes.Length; i++)
                _debugFrameTimes[i - 1] = _debugFrameTimes[i];

            _debugFrameTimes[_debugFrameTimes.Length - 1] = (float)_frameTimesTimer.Elapsed.TotalMilliseconds;
            _frameTimesTimer.Restart();
        }
        private struct DebugMemoryUsageTick
        {
            public DateTime Measured;
            public long BytesUsed;
            public bool HasGen0GC;
            public bool HasGen1GC;
            public bool HasGen2GC;
        }


        private int _g0Gc = 0;
        private int _g1Gc = 0;
        private int _g2Gc = 0;
        private DebugGCTuple DebugDetectGC()
        {
            var returnValue = new DebugGCTuple();

            if (GC.MaxGeneration >= 0 && GC.CollectionCount(0) != _g0Gc)
                returnValue.Gen0 = true;
            if (GC.MaxGeneration >= 1 && GC.CollectionCount(1) != _g1Gc)
                returnValue.Gen1 = true;
            if (GC.MaxGeneration >= 2 && GC.CollectionCount(2) != _g2Gc)
                returnValue.Gen2 = true;

            if (GC.MaxGeneration >= 0)
                _g0Gc = GC.CollectionCount(0);
            if (GC.MaxGeneration >= 1)
                _g1Gc = GC.CollectionCount(1);
            if (GC.MaxGeneration >= 2)
                _g2Gc = GC.CollectionCount(2);

            return returnValue;
        }
        struct DebugGCTuple
        {
            public bool Gen0;
            public bool Gen1;
            public bool Gen2;
        }
        #endregion

        #region Text Debug
        private Process _prc;
        private StringBuilder _bldr = new StringBuilder(2000);
        private void DrawTextDebugInfo(GameTime gameTime)
        {
            _bldr.Clear();
            if (_prc == null)
                _prc = Process.GetCurrentProcess();

            spriteBatch.Begin();
            var tanksCount = 0;
            var projCount = 0;
            var mapObjectCount = 0;
            var otherCount = 0;
            foreach (var obj in game.GameObjects)
            {
                if (obj.GetType().IsSubclassOf(typeof(MPTanks.Engine.Tanks.Tank)))
                    tanksCount++;
                else if (obj.GetType().IsSubclassOf(typeof(MPTanks.Engine.Projectiles.Projectile)))
                    projCount++;
                else if (obj.GetType().IsSubclassOf(typeof(MPTanks.Engine.Maps.MapObjects.MapObject)))
                    mapObjectCount++;
                else otherCount++;
            }
            //Note: The debug screen generates a bit of garbage so don't try to use it to nail down allocations
            //Disable it first and then see if there's still a problem
            _bldr.Append("Tanks: ").Append(tanksCount)
                .Append(", Projectiles: ").Append(projCount)
                .Append(", Map Objects: ").Append(mapObjectCount)
                .Append(", Other: ").Append(otherCount)
                .Append(", Total: ").Append(tanksCount + projCount + mapObjectCount + otherCount)
                .Append("\n");
            _bldr.Append("Update: ").Append(physicsMs.ToString("N2"))
                        .Append(", Render: ").Append(renderMs.ToString("N2"))
                        .Append(", Update #: ").Append(updateNumber)
                        .Append(", Frame #: ").Append(frameNumber)
                        ;

            if (float.IsInfinity(CalculateAverageFPS()) || float.IsNaN(CalculateAverageFPS()))
                _bldr.Append(", FPS: ").Append("Calculation Error").Append(" avg, ");
            else
                _bldr.Append(", FPS: ").Append((CalculateAverageFPS().ToString("N1"))).Append(" avg, ");

            _bldr.Append((1000 / _debugFrameTimes[_debugFrameTimes.Length - 1]).ToString("N1")).Append(" now")
            .Append("\nMouse: ").Append(Mouse.GetState().Position.ToString());

            long maxMem = 0;
            foreach (var pt in _debugMemoryUsages)
                if (pt.BytesUsed > maxMem)
                    maxMem = pt.BytesUsed;

            _bldr.Append(", Timers: ").Append(game.TimerFactory.ActiveTimersCount)
                .Append(", Animations: ").Append(game.AnimationEngine.Animations.Count)
                .Append(", Particles: ").Append(game.ParticleEngine.LivingParticlesCount)
                .Append("\nSounds (Engine): ").Append(game.SoundEngine.SoundCount)
                .Append(", Sounds (Backend): ").Append(_soundPlayer.ActiveSoundCount);

            var info = _soundPlayer.Diagnostics;
            _bldr.Append("\nSound CPU (DSP, Streaming, Total): ")
            .Append(info.DSPCPU.ToString("N2")).Append("%, ")
            .Append(info.StreamCPU.ToString("N2")).Append("%, ")
            .Append(info.TotalCPU.ToString("N2")).Append("%");

            _bldr.Append("\nGC (gen 0, 1, 2): ").Append(GC.CollectionCount(0)).Append(" ")
                .Append(GC.CollectionCount(1)).Append(" ").Append(GC.CollectionCount(2))
                .Append(", Memory: ").Append((GC.GetTotalMemory(false) / (1024d * 1024)).ToString("N1")).Append("MB used")
                .Append(", ").Append((maxMem / (1024d * 1024)).ToString("N1")).Append("MB max");

            if (game.Running)
            {
                _bldr.Append("\nTimescale: " + GameCore.TimescaleValue.Values[timescaleIndex].DisplayString);
            }

            _bldr.Append(", Status: ");

            if (game.CountingDown)
                _bldr.Append("starting game");
            if (game.WaitingForPlayers)
                _bldr.Append(" waiting for players");
            if (game.Running)
                _bldr.Append(" running");
            if (game.GameEnded)
                _bldr.Append(" ended");

            if (game.Gamemode.WinningTeam != MPTanks.Engine.Gamemodes.Team.Null)
                _bldr.Append(", Winner: ").Append(game.Gamemode.WinningTeam.TeamName);

            _bldr.Append("\nF12: hide\n")
                .Append("F10: Enable/Disable graphs\n")
                .Append("F9: Enable/Disable debug text\n")
                .Append("F8: Switch between vertical and horizontal graphs\n")
                .Append("ESC: Exit\n");

            spriteBatch.DrawString(font, _bldr.ToString(), new Vector2(10, 10), Color.MediumPurple);
            spriteBatch.End();
        }
        #endregion
        #region FPS Calculations
        private float CalculateAverageFPS()
        {
            return _debugFrameTimes.Select(a => 1000 / a).Average();
        }

        #endregion

        const int graphHeight = 150;
        const int graphWidth = 400;
        const int graphOffset = 20;

        private Texture2D _graphTexture;
        private void DrawGraphDebugInfo(GameTime gameTime)
        {
            //3 graphs:
            //1) memory usage: capped to max, colored blue. Spikes are colored red
            //2) frame times: capped to 50ms, items above 20ms and below 6ms are highlighted red and yellow respectively
            //above 50 is black
            //3) frame rate: capped to 60fps, 60 is green, 20 is red - inbetween is lerped
            //above 60 is black

            //drawn at bottom left of screen, 150px high
            //1st graph is 0-200 pixels
            //2nd graph is 220-420 pixels
            //3rd graph is 440-660 pixels

            if (_graphTexture == null)
            {
                _graphTexture = new Texture2D(GraphicsDevice, 1, 1);
                _graphTexture.SetData(new[] { Color.White });
            }

            DrawMemoryUsageGraph();
            DrawFrameRateGraph();
        }

        private void DrawMemoryUsageGraph()
        {
            //Memory Usage graph
            var max = 0L;
            var average = 0L;

            var graphPosX = 0;
            var graphBottomY = GraphicsDevice.Viewport.Height;

            for (var i = 0; i < _debugMemoryUsages.Length; i++)
            {
                average += _debugMemoryUsages[i].BytesUsed;
                if (_debugMemoryUsages[i].BytesUsed > max)
                    max = _debugMemoryUsages[i].BytesUsed;
            }

            average /= _debugMemoryUsages.Length;
            var spikeMinimum = average * 1.5d;

            double pixelsPerByteH = (double)graphHeight / max;
            double pixelsPerDataPointWidth = (double)graphWidth / _debugMemoryUsages.Length;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
                SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone);
            int pixelsUsed = 0;
            for (var i = 0; i < _debugMemoryUsages.Length; i++)
            {
                var value = _debugMemoryUsages[i];
                var maxPixels = pixelsPerDataPointWidth * i;
                var width = (int)maxPixels - pixelsUsed;
                if (pixelsUsed < (int)maxPixels)
                {
                    var color = (value.BytesUsed > spikeMinimum) ? Color.Red : Color.Blue;
                    var height = (int)(pixelsPerByteH * value.BytesUsed);
                    spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed,
                        graphBottomY - height, width, height),
                        color);
                }

                //Draw the GC marker
                if (value.HasGen2GC)
                {
                    spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 3,
                        graphBottomY - 13, 8, 6), Color.Black);
                    spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 2,
                        graphBottomY - 12, 4, 4), Color.Red);
                }
                else if (value.HasGen1GC)
                {
                    spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 3,
                        GraphicsDevice.Viewport.Height - 13, 8, 6), Color.Black);
                    spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 2,
                        graphBottomY - 12, 4, 4), Color.Yellow);
                }
                else if (value.HasGen0GC)
                {
                    spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 3,
                        graphBottomY - 13, 8, 6), Color.Black);
                    spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 2,
                        graphBottomY - 12, 4, 4), Color.Green);
                }

                if (pixelsUsed < (int)maxPixels)
                    pixelsUsed += width;
            }
            //Draw the label 10, 10 px from the bottom left
            //Draw the label 10, 10 px from the bottom left
            var size = font.MeasureString("Memory Usage (managed only)");
            var pos = new Vector2(graphPosX, graphBottomY - size.Y);
            spriteBatch.DrawString(font, "Memory Usage (managed only)", pos,
                new Color(Color.Black, 150));
            spriteBatch.End();
        }
        private void DrawFrameRateGraph()
        {
            double pixelsPerFpsH = (double)graphHeight / 70;
            double pixelsPerDataPointWidth = (double)graphWidth / _debugFrameTimes.Length;

            var graphPosX = graphOffset + graphWidth;
            var graphBottomY = GraphicsDevice.Viewport.Height;
            if (debugOverlayGraphsVertical)
            {
                graphPosX = 0;
                graphBottomY -= graphOffset + graphHeight;
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone);
            int pixelsUsed = 0;
            for (var i = 0; i < _debugFrameTimes.Length; i++)
            {
                var value = 1000 / _debugFrameTimes[i];
                var maxPixels = pixelsPerDataPointWidth * i;
                var width = (int)maxPixels - pixelsUsed;
                if (pixelsUsed < (int)maxPixels)
                {
                    Color color = Color.Green;

                    if (value < 55 && value >= 30)
                        color = Color.Yellow;
                    else if (value < 30 && value >= 20)
                        color = Color.Orange;
                    else if (value < 20)
                        color = Color.Red;
                    var height = (int)(pixelsPerFpsH * value);
                    if (value > 70)
                    {
                        color = Color.LimeGreen;
                        height = graphHeight;
                    }
                    spriteBatch.Draw(_graphTexture, new Rectangle(pixelsUsed + graphPosX,
                        graphBottomY - height, width, height),
                        color);
                }

                if (pixelsUsed < (int)maxPixels)
                    pixelsUsed += width;
            }
            //Draw the label 10, 10 px from the bottom left
            var size = font.MeasureString("FPS: (max 70)");
            var pos = new Vector2(graphPosX, graphBottomY - size.Y);
            spriteBatch.DrawString(font, "FPS: (max 70)", pos,
                new Color(Color.Black, 150));
            spriteBatch.End();
        }
        #endregion
    }
}
