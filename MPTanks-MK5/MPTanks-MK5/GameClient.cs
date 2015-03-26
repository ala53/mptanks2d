#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using MPTanks_MK5.Rendering;
using Engine.Tanks;
using Engine;
using System.Diagnostics;
using System.Runtime;
using System.Text;
#endregion

namespace MPTanks_MK5
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameClient : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private GameWorldRenderer renderer;
        private Guid player1Id;
        private Guid player2Id;
        private Engine.GameCore game;
        private float zoom = 6.5f;
        private SpriteFont font;
        private Stopwatch timer = new Stopwatch();
        private RectangleF drawRect;
        private bool loading { get { return !loadingScreen.Completed || !loadingScreen.IsSlidingOut; } }
        private LoadingScreen loadingScreen;
        private Screens.Screen currentScreen;

        private bool _unlockCursor = false;

        private bool _graphicsDeviceIsDirty = false;

        public GameClient()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            GCSettings.LatencyMode = GCLatencyMode.Batch;

            graphics.PreferMultiSampling = true;

            Window.AllowUserResizing = true;
            // IsMouseVisible = true;
            //IsFixedTimeStep = false;
            //graphics.SynchronizeWithVerticalRetrace = false;
            // graphics.ApplyChanges();
            // TargetElapsedTime = TimeSpan.FromMilliseconds(33.3333333);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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
            loadingScreen = new LoadingScreen(this);
            font = Content.Load<SpriteFont>("font");
        }

        private void SetupGame()
        {
            game = new Engine.GameCore(new EngineInterface.FileLogger(), new Engine.Gamemodes.TeamDeathMatchGamemode(), "");
            game.Authoritative = true;
            //game.FriendlyFireEnabled = true;

            game.AddGameObject(
                new Engine.Maps.MapObjects.Static.Animated.SatelliteDishLarge(
                    game, true, new Vector2(10, 20), 33), null, true);
            game.AddGameObject(new Engine.Maps.MapObjects.Building(game, true, new Vector2(50, 50), 33), null, true);
            game.AddGameObject(new Engine.Maps.MapObjects.Building(game, true, new Vector2(150, 30), 33), null, true);
            game.AddGameObject(new Engine.Maps.MapObjects.Building(game, true, new Vector2(30, 80), 33), null, true);

            player1Id = Guid.NewGuid();
            player2Id = Guid.NewGuid();
            game.AddPlayer(player1Id);
            game.AddPlayer(player2Id);
            for (var i = 0; i < 0; i++)
                game.AddPlayer(Guid.NewGuid());

            //Set up rendering
            if (renderer != null)
                renderer.Destroy();
            renderer = new GameWorldRenderer(currentScreen, game);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void KeyboardEvents_KeyPressed(object sender, Starbound.Input.KeyboardEventArgs e)
        {
#if DEBUG
            if (e.Key == Keys.F7)
                Debugger.Break();
#endif
            if (e.Key == Keys.F11)
            {
                graphics.ToggleFullScreen();
            }

            if (e.Key == Keys.RightControl)
            {
                _unlockCursor = !_unlockCursor;
                IsMouseVisible = !IsMouseVisible;
            }
        }

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

            if (slow) Logger.Debug("Last frame was slow according to real time (" + el.TotalMilliseconds + "ms). Last frame was update " + updateNumber + ", frame " + frameNumber + " GameTime says: " +
                gameTime.ElapsedGameTime.TotalMilliseconds + "ms, running slowly: " + gameTime.IsRunningSlowly);

            updateNumber++;
            DetectGC();
            if (game.IsCountingDownToStart)
            {
                loadingScreen.Value = 5 - game.RemainingCountdownSeconds;
                loadingScreen.Maximum = Engine.Settings.TimeToWaitBeforeStartingGame / 1000;
                loadingScreen.Status = game.RemainingCountdownSeconds.ToString("N1") + " seconds remaining";
                loadingScreen.Billboard = "Setting up...";
            }

            //Check for GD changes 
            //It's done here because applychanges can cause issues
            //when called repeatedly - Window Resize causes a stack overflow
            if (_graphicsDeviceIsDirty)
            {
                graphics.ApplyChanges();
                _graphicsDeviceIsDirty = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Y))
                game.Timescale = 1 / 32f;
            else
                game.Timescale = 1f;
            timer.Restart();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.OemTilde))
                SetupGame(); //Start anew

            if (game.IsGameRunning)
            {
                game.Diagnostics.BeginMeasurement("Input processing");
                var iState = new InputState();
                iState.LookDirection = game.Players[player1Id].Rotation;

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

                game.InjectPlayerInput(player1Id, iState);

                var iState2 = new InputState();
                iState2.LookDirection = game.Players[player2Id].Rotation;

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

                game.InjectPlayerInput(player2Id, iState2);

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

            if (Keyboard.GetState().IsKeyDown(Keys.V))
            {
                for (var i = 0; i < 50; i++)
                {
                    game.ParticleEngine.CreateEmitter(0.2f, Engine.Assets.BasicTank.MainGunSparks, Color.Green, new Engine.Core.RectangleF(20, 20, 10, 10), new Vector2(0.05f));
                }
            }

            game.Update(gameTime);

            game.Diagnostics.BeginMeasurement("Base.Update()");
            base.Update(gameTime);
            game.Diagnostics.EndMeasurement("Base.Update()");
            timer.Stop();
            physicsMs = (float)timer.Elapsed.TotalMilliseconds;
            DetectGC();
        }

        private int _g0Gc = 0;
        private int _g1Gc = 0;
        private int _g2Gc = 0;
        private void DetectGC()
        {
            var g0 = GC.CollectionCount(0);
            var g1 = GC.CollectionCount(1);
            var g2 = GC.CollectionCount(2);
            if (g2 != _g2Gc)
            {
                Logger.Debug("Generation 2 GC (Update: " + updateNumber + ", Frame: " + frameNumber + ")");
            }
            else if (g1 != _g1Gc)
            {
                Logger.Debug("Generation 1 GC (Update: " + updateNumber + ", Frame: " + frameNumber + ")");
            }
            else if (g0 != _g0Gc)
            {
                Logger.Debug("Generation 0 GC (Update: " + updateNumber + ", Frame: " + frameNumber + ")");
            }

            _g0Gc = g0;
            _g1Gc = g1;
            _g2Gc = g2;
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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            DetectGC();
            frameNumber++;
            game.Diagnostics.BeginMeasurement("Rendering");
            timer.Restart(); //Stat tracking
            GraphicsDevice.Clear(Color.Black);

            if (!loading || loadingScreen.IsSlidingOut)
            { //in game
                drawRect = new RectangleF(
                    0,
                    0,
                    30 * zoom,
                    20 * zoom);
                //if (game.Players.ContainsKey(player1Id))
                //    drawRect = new RectangleF(
                //        game.Players[player1Id].Position.X - (15 * zoom),
                //        game.Players[player1Id].Position.Y - (10 * zoom),
                //        30 * zoom,
                //        20 * zoom);
                game.Diagnostics.BeginMeasurement("World rendering", "Rendering");
                renderer.SetViewport(drawRect);
                renderer.Render(spriteBatch, gameTime);
                game.Diagnostics.EndMeasurement("World rendering", "Rendering");
            }
            //And draw the loading screen last so its over everything
            if (loading || loadingScreen.IsSlidingOut)
            {
                loadingScreen.Render(spriteBatch, gameTime);
            }
            game.Diagnostics.BeginMeasurement("Draw debug text", "Rendering");
            DrawDebugInfo(gameTime);
            game.Diagnostics.EndMeasurement("Draw debug text", "Rendering");

            if (ClientSettings.ForceFullGCEveryFrame)
                GC.Collect(2, GCCollectionMode.Forced, true);
            if (ClientSettings.ForceGen0GCEveryFrame)
                GC.Collect(0, GCCollectionMode.Forced, true);

            game.Diagnostics.BeginMeasurement("Base.Draw()", "Rendering");
            base.Draw(gameTime);
            game.Diagnostics.EndMeasurement("Base.Draw()", "Rendering");
            game.Diagnostics.EndMeasurement("Rendering");
            timer.Stop();
            renderMs = (float)timer.Elapsed.TotalMilliseconds;
            if (physicsMs + renderMs > 10 && game.IsGameRunning)
            {
                Logger.Debug("Frame took too long! (Update: " + updateNumber + ", Frame: " + frameNumber + ")");
                Logger.Debug("\n\n\n" + game.Diagnostics.ToString());
            }
        }

        #region Debug info
        private Process _prc;
        private StringBuilder _bldr = new StringBuilder(1000);
        private void DrawDebugInfo(GameTime gameTime)
        {
            _bldr.Clear();
            if (_prc == null)
                _prc = Process.GetCurrentProcess();

            spriteBatch.Begin();
            var tanksCount = 0;
            var projCount = 0;
            foreach (var obj in game.GameObjects)
            {
                if (obj.GetType().IsSubclassOf(typeof(Engine.Tanks.Tank)))
                    tanksCount++;
                if (obj.GetType().IsSubclassOf(typeof(Engine.Projectiles.Projectile)))
                    projCount++;
            }
            var fps = CalculateAverageFPS((float)gameTime.ElapsedGameTime.TotalMilliseconds).ToString("N1");
            //Note: The debug screen generates a bit of garbage so don't try to use it to nail down allocations
            //Disable it first and then see if there's still a problem
            _bldr.Append("Tanks: ").Append(tanksCount)
            .Append(", Projectiles: ").Append(projCount)
            .Append(", Zoom: ").Append(zoom.ToString("N2"))
            .Append(", Update: ").Append(physicsMs.ToString("N2"))
            .Append(", Render: ").Append(renderMs.ToString("N2"))
            .Append(",\nMouse: ").Append(Mouse.GetState().Position.ToString())
            .Append(", Tank: ");

            if (game.Players.ContainsKey(player1Id))
                _bldr.Append("{ ").Append(game.Players[player1Id].Position.X.ToString("N1"))
                  .Append(", ").Append(game.Players[player1Id].Position.Y.ToString("N1"))
                  .Append(" }");
            else _bldr.Append("not spawned");

            _bldr.Append(", Active Timers: ").Append(game.TimerFactory.ActiveTimersCount)
                .Append(",\nAnimation layers: ").Append(game.AnimationEngine.Animations.Count)
                .Append(", Particles: ").Append(game.ParticleEngine.LivingParticlesCount)
                .Append(", FPS: ").Append(fps).Append(" avg, ")
                .Append((1000 / gameTime.ElapsedGameTime.TotalMilliseconds).ToString("N1")).Append(" now")
                .Append(",\nGC (gen 0, 1, 2): ").Append(GC.CollectionCount(0)).Append(" ")
                .Append(GC.CollectionCount(1)).Append(" ").Append(GC.CollectionCount(2))
                .Append(", Memory: ").Append((GC.GetTotalMemory(false) / (1024d * 1024)).ToString("N1")).Append("MB used")
                .Append(",\nStatus:");

            if (game.IsWaitingForPlayers)
                _bldr.Append(" waiting for players");
            if (game.IsGameRunning)
                _bldr.Append(" running");
            if (game.Gamemode.GameEnded)
                _bldr.Append(" ended");

            if (game.Gamemode.WinningTeam != Engine.Gamemodes.Team.Null)
                _bldr.Append(", Winner").Append(game.Gamemode.WinningTeam.TeamName);

            spriteBatch.DrawString(font, _bldr.ToString(), new Vector2(10, 10), (slow ? Color.Red : Color.MediumPurple));
            spriteBatch.End();
        }
        private float[] _fps;

        private float CalculateAverageFPS(float deltaMs)
        {
            if (_fps == null)
            {
                _fps = new float[30];
                for (int i = 0; i < _fps.Length; i++)
                    _fps[i] = 16.666666f;
            }

            for (int i = 0; i < _fps.Length - 1; i++)
                _fps[i] = _fps[i + 1];

            _fps[_fps.Length - 1] = deltaMs;
            return GetFps();
        }

        private float GetFps()
        {
            if (_fps == null)
            {
                _fps = new float[30];
                for (int i = 0; i < _fps.Length; i++)
                    _fps[i] = 16.666666f;
            }

            float tot = 0;
            foreach (var f in _fps)
                tot += f;
            return 1000 / (tot / _fps.Length);

        }
        #endregion
    }
}
