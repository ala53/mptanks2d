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
using MPTanks.Client.GameSandbox.Input;
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
        private MPTanks.Engine.GameCore _game;
        private SpriteFont _debugFont;
        private Backend.Sound.SoundPlayer _soundPlayer;
        private InputDriverBase _inputDriver;
        private GameCoreRenderer _gcRenderer;
        private UserInterface _ui;

        const string _settingUpPageName = "SettingUpPrompt";

        public Diagnostics Diagnostics => _game?.Diagnostics;

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
            _inputDriver = InputDriverBase.GetDriver(GameSettings.Instance.InputDriverName, this);
            if (GameSettings.Instance.InputKeyBindings.Value != null)
                _inputDriver.SetKeyBindings(GameSettings.Instance.InputKeyBindings);
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
            SetupGame();
            _debugFont = Content.Load<SpriteFont>("font");

        }

        private void SetupGame()
        {
            _game = new GameCore(
                new NLogLogger(Logger.Instance),
                Engine.Gamemodes.Gamemode.ReflectiveInitialize("TeamDeathMatchGamemode"),
                Modding.ModLoader.LoadedMods["core-assets.mod"].GetPackedFileString("testmap.json"),
                false,
                new EngineSettings("enginesettings.json")
                );
            _game.Authoritative = true;
            _game.FriendlyFireEnabled = true;

            _ui.SetPage(_settingUpPageName);

            _player = (NetworkPlayer)_game.AddPlayer(new NetworkPlayer()
            {
                Id = Guid.NewGuid()
            });

            for (var i = 0; i < 5; i++)
            {
                _game.AddPlayer(new NetworkPlayer { Id = Guid.NewGuid() });
            }
            if (_gcRenderer != null) _gcRenderer.Dispose();
            _gcRenderer = new GameCoreRenderer(this, _game, GameSettings.Instance.AssetSearchPaths, new[] { 0 });
            if (_soundPlayer != null) _soundPlayer.Dispose();
            _soundPlayer = new Backend.Sound.SoundPlayer(_game);
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

        private int _gameTimescaleIndex;
        private void KeyboardEvents_KeyPressed(object sender, Starbound.Input.KeyboardEventArgs e)
        {
            if (e.Key == Keys.F11)
            {
                _graphics.ToggleFullScreen();
                GameSettings.Instance.Fullscreen.Value = _graphics.IsFullScreen;
            }
            if (e.Key == Keys.F12)
                debugEnabled = !debugEnabled;
            if (e.Key == Keys.F10)
                drawGraphDebug = !drawGraphDebug;
            if (e.Key == Keys.F9)
                drawTextDebug = !drawTextDebug;
            if (e.Key == Keys.F8)
                debugOverlayGraphsVertical = !debugOverlayGraphsVertical;

            if (e.Key == Keys.F7)
            {
                _gameTimescaleIndex++;

                if (_gameTimescaleIndex < 0) _gameTimescaleIndex = 0;
                if (_gameTimescaleIndex >= GameCore.TimescaleValue.Values.Count)
                    _gameTimescaleIndex = GameCore.TimescaleValue.Values.Count - 1;
                _game.Timescale = GameCore.TimescaleValue.Values[_gameTimescaleIndex];
            }
            if (e.Key == Keys.F6)
            {
                _gameTimescaleIndex--;
                if (_gameTimescaleIndex < 0) _gameTimescaleIndex = 0;
                if (_gameTimescaleIndex >= GameCore.TimescaleValue.Values.Count)
                    _gameTimescaleIndex = GameCore.TimescaleValue.Values.Count - 1;
                _game.Timescale = GameCore.TimescaleValue.Values[_gameTimescaleIndex];
            }
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

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.OemTilde))
                SetupGame(); //Start anew

            if (_game != null && _game.Running)
            {
                Diagnostics.BeginMeasurement("Input processing");
                if (IsActive)
                    _player?.Tank?.Input(_inputDriver.GetInputState());
                Diagnostics.EndMeasurement("Input processing");
            }


            if (_player.Tank != null && _soundPlayer != null)
            {
                _soundPlayer.PlayerPosition = _player.Tank.Position;
                _soundPlayer.PlayerVelocity = _player.Tank.LinearVelocity;
            }
            _soundPlayer?.Update(gameTime);
            _game?.Update(gameTime);

            _inputDriver.Update(gameTime);

            Diagnostics.BeginMeasurement("Base.Update()");
            base.Update(gameTime);
            Diagnostics.EndMeasurement("Base.Update()");

            if (_game.CountingDown && _ui.PageName != _settingUpPageName)
                _ui.SetPage(_settingUpPageName);

            if (_game.CountingDown)
                _ui.ActiveBinder.TimeRemaining = _game.RemainingCountdownTime;

            if (!_game.CountingDown && _ui.PageName == _settingUpPageName)
                _ui.UIPage = UserInterfacePage.GetEmptyPageInstance();

            _ui.Update(gameTime);

            if (GameSettings.Instance.ForceFullGCEveryFrame)
                GC.Collect(2, GCCollectionMode.Forced, true);
            if (GameSettings.Instance.ForceGen0GCEveryFrame)
                GC.Collect(0, GCCollectionMode.Forced, true);
        }

        RenderTarget2D _worldRenderTarget;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Diagnostics.BeginMeasurement("Rendering");

            GraphicsDevice.Clear(Color.DarkGray);
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
            GraphicsDevice.SetRenderTarget(_worldRenderTarget);

            if (_game != null)
            {
                RectangleF drawRect = new RectangleF(0, 0, 1, 1);
                if (_player.Tank != null)
                {
                    var widthHeightRelative =
                        (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;

                    drawRect = new RectangleF(
                        _player.Tank.Position.X - ((30 * widthHeightRelative) * GameSettings.Instance.Zoom),
                        _player.Tank.Position.Y - (30 * GameSettings.Instance.Zoom),
                        (60 * widthHeightRelative) * GameSettings.Instance.Zoom,
                        60 * GameSettings.Instance.Zoom);
                }
                Diagnostics.BeginMeasurement("World rendering", "Rendering");

                _gcRenderer.View = drawRect;
                _gcRenderer.Target = _worldRenderTarget;
                _gcRenderer.Draw(gameTime);

                Diagnostics.EndMeasurement("World rendering", "Rendering");
                //And draw to screen
                Diagnostics.BeginMeasurement("Copy to screen", "Rendering");

                GraphicsDevice.SetRenderTarget(null);
                _spriteBatch.Begin(SpriteSortMode.Immediate);
                _spriteBatch.Draw(_worldRenderTarget, _worldRenderTarget.Bounds, Color.White);
                _spriteBatch.End();

                Diagnostics.EndMeasurement("Copy to screen", "Rendering");
            }

            Diagnostics.BeginMeasurement("Draw debug text", "Rendering");
            DrawDebugInfo(gameTime);
            Diagnostics.EndMeasurement("Draw debug text", "Rendering");

            _ui.Draw(gameTime);

            base.Draw(gameTime);
            Diagnostics.EndMeasurement("Rendering");
        }

        #region Debug info

        private bool debugEnabled = false;
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

            _spriteBatch.Begin();
            var tanksCount = 0;
            var projCount = 0;
            var mapObjectCount = 0;
            var otherCount = 0;
            foreach (var obj in _game.GameObjects)
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
                .Append(", Total: ").Append(tanksCount + projCount + mapObjectCount + otherCount);

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

            _bldr.Append(", Timers: ").Append(_game.TimerFactory.ActiveTimersCount)
                .Append(", Animations: ").Append(_game.AnimationEngine.Animations.Count)
                .Append(", Particles: ").Append(_game.ParticleEngine.LivingParticlesCount)
                .Append("\nSounds (Engine, Backend): ").Append(_game.SoundEngine.SoundCount)
                .Append(", ").Append(_soundPlayer.ActiveSoundCount)
                .Append(", Volumes (Background, Effects, Voice): ")
                .Append((_soundPlayer.BackgroundVolume * 100).ToString("N0")).Append("%")
                .Append(", ").Append((_soundPlayer.EffectVolume * 100).ToString("N0")).Append("%")
                .Append(", ").Append((_soundPlayer.VoiceVolume * 100).ToString("N0")).Append("%");

            var info = _soundPlayer.Diagnostics;
            _bldr.Append("\nSound CPU (DSP, Streaming, Update, Total): ")
            .Append(info.DSPCPU.ToString("N2")).Append("%, ")
            .Append(info.StreamCPU.ToString("N2")).Append("%, ")
            .Append(info.UpdateCPU.ToString("N2")).Append("%, ")
            .Append(info.TotalCPU.ToString("N2")).Append("%");

            _bldr.Append("\nGC (gen 0, 1, 2): ").Append(GC.CollectionCount(0)).Append(" ")
                .Append(GC.CollectionCount(1)).Append(" ").Append(GC.CollectionCount(2))
                .Append(", Memory: ").Append((GC.GetTotalMemory(false) / (1024d * 1024)).ToString("N1")).Append("MB used")
                .Append(", ").Append((maxMem / (1024d * 1024)).ToString("N1")).Append("MB max");

            if (_game.Running)
            {
                _bldr.Append("\nTimescale: " + GameCore.TimescaleValue.Values[_gameTimescaleIndex].DisplayString);
            }

            _bldr.Append(", Status: ");

            if (_game.CountingDown)
                _bldr.Append("starting game");
            if (_game.WaitingForPlayers)
                _bldr.Append(" waiting for players");
            if (_game.Running)
                _bldr.Append(" running");
            if (_game.GameEnded)
                _bldr.Append(" ended");

            if (_game.Gamemode.WinningTeam != MPTanks.Engine.Gamemodes.Team.Null)
                _bldr.Append(", Winner: ").Append(_game.Gamemode.WinningTeam.TeamName);

            _bldr.Append("\nF12: hide\n")
                .Append("F10: Enable/Disable graphs\n")
                .Append("F9: Enable/Disable debug text\n")
                .Append("F8: Switch between vertical and horizontal graphs\n")
                .Append("ESC: Exit\n");

            _spriteBatch.DrawString(_debugFont, _bldr.ToString(), new Vector2(10, 10), Color.MediumPurple);
            _spriteBatch.End();
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

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
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
                    _spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed,
                        graphBottomY - height, width, height),
                        color);
                }

                //Draw the GC marker
                if (value.HasGen2GC)
                {
                    _spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 3,
                        graphBottomY - 13, 8, 6), Color.Black);
                    _spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 2,
                        graphBottomY - 12, 4, 4), Color.Red);
                }
                else if (value.HasGen1GC)
                {
                    _spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 3,
                        GraphicsDevice.Viewport.Height - 13, 8, 6), Color.Black);
                    _spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 2,
                        graphBottomY - 12, 4, 4), Color.Yellow);
                }
                else if (value.HasGen0GC)
                {
                    _spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 3,
                        graphBottomY - 13, 8, 6), Color.Black);
                    _spriteBatch.Draw(_graphTexture, new Rectangle(graphPosX + pixelsUsed - 2,
                        graphBottomY - 12, 4, 4), Color.Green);
                }

                if (pixelsUsed < (int)maxPixels)
                    pixelsUsed += width;
            }
            //Draw the label 10, 10 px from the bottom left
            //Draw the label 10, 10 px from the bottom left
            var size = _debugFont.MeasureString("Memory Usage (managed only)");
            var pos = new Vector2(graphPosX, graphBottomY - size.Y);
            _spriteBatch.DrawString(_debugFont, "Memory Usage (managed only)", pos,
                new Color(Color.Black, 150));
            _spriteBatch.End();
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

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
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
                    _spriteBatch.Draw(_graphTexture, new Rectangle(pixelsUsed + graphPosX,
                        graphBottomY - height, width, height),
                        color);
                }

                if (pixelsUsed < (int)maxPixels)
                    pixelsUsed += width;
            }
            //Draw the label 10, 10 px from the bottom left
            var size = _debugFont.MeasureString("FPS: (max 70)");
            var pos = new Vector2(graphPosX, graphBottomY - size.Y);
            _spriteBatch.DrawString(_debugFont, "FPS: (max 70)", pos,
                new Color(Color.Black, 150));
            _spriteBatch.End();
        }
        #endregion
    }
}
