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
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

            graphics.PreferMultiSampling = true;

            Window.AllowUserResizing = true;
            // IsMouseVisible = true;
            // IsFixedTimeStep = false;
            // graphics.SynchronizeWithVerticalRetrace = false;
            // graphics.ApplyChanges();
            // TargetElapsedTime = TimeSpan.FromMilliseconds(8);
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

        private float physicsMs = 0;
        private float renderMs = 0;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (game.IsCountingDownToStart)
            {
                loadingScreen.Value = 5 - game.RemainingCountdownSeconds;
                loadingScreen.Maximum = Settings.TimeToWaitBeforeStartingGame / 1000;
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
            }

            if (!_unlockCursor)
                LockCursor();

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

            base.Update(gameTime);
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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            timer.Restart(); //Stat tracking
            GraphicsDevice.Clear(Color.Black);

            if (!loading || loadingScreen.IsSlidingOut)
            { //in game
                drawRect = new RectangleF(
                    0,
                    0,
                    30 * zoom,
                    20 * zoom);
                if (game.Players.ContainsKey(player1Id))
                    drawRect = new RectangleF(
                        game.Players[player1Id].Position.X - (15 * zoom),
                        game.Players[player1Id].Position.Y - (10 * zoom),
                        30 * zoom,
                        20 * zoom);
                renderer.SetViewport(drawRect);
                renderer.Render(spriteBatch, gameTime);
            }
            //And draw the loading screen last so its over everything
            if (loading || loadingScreen.IsSlidingOut)
            {
                loadingScreen.Render(spriteBatch, gameTime);
            }
            DrawDebugInfo(gameTime);
            base.Draw(gameTime);
            timer.Stop();
            renderMs = (float)timer.Elapsed.TotalMilliseconds;

            if (ClientSettings.ForceGCEveryFrame)
                GC.Collect(0, GCCollectionMode.Forced, true);
        }

        #region Debug info
        private Process _prc;
        private void DrawDebugInfo(GameTime gameTime)
        {
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
            //Note: The debug screen generates a bunch of garbage so don't try to use it to nail down allocations
            //Disable it first and then see if there's still a problem
            spriteBatch.DrawString(font, "Tanks: " + tanksCount + ", Projectiles: " + projCount.ToString() +
                    ", Zoom: " + zoom.ToString("N2") +
                    ", Update: " + physicsMs.ToString("N2") + ", Render: " + renderMs.ToString("N2") +
                ",\nMouse: " + Mouse.GetState().Position.ToString() + ", Tank: " +
                (game.Players.ContainsKey(player1Id) ?
                    "{ " + game.Players[player1Id].Position.X.ToString("N1") + ", " +
                    game.Players[player1Id].Position.Y.ToString("N1") + " }" : "not spawned") +
                ", Active timers: " + game.TimerFactory.ActiveTimersCount + ", \nAnimation layers: " +
                game.AnimationEngine.Animations.Count + ", Particles: " +
                game.ParticleEngine.LivingParticlesCount + ", FPS: " + fps + " avg, " +
                (1000 / gameTime.ElapsedGameTime.TotalMilliseconds).ToString("N1") + " now"
                + ",\nGC (gen 0, 1, 2): " +
                GC.CollectionCount(0) + " " + GC.CollectionCount(1) + " " + GC.CollectionCount(2) + "," +
                " Memory: " + (GC.GetTotalMemory(false) / (1024f * 1024)).ToString("N1") + "MB used" +
                ",\nStatus: " + (game.IsWaitingForPlayers ? "waiting for players" : "") +
                (game.IsGameRunning ? "running" : "") +
                (game.Gamemode.GameEnded ? game.IsGameRunning ? ", game ended" : "game ended" : "") +
                (game.IsCountingDownToStart ? game.RemainingCountdownSeconds.ToString("N1") + "s until start" : "") +
                (game.Gamemode.WinningTeam == Engine.Gamemodes.Team.Null ? "" : ", Winner: " + game.Gamemode.WinningTeam.TeamName)
            , new Vector2(10, 10), Color.MediumPurple);
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
