using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MPTanks.Client.Backend.UI;
using System;

namespace MPTanks.Client
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ClientCore : Game
    {
        GraphicsDeviceManager graphics;

        private int nativeScreenWidth;
        private int nativeScreenHeight;


        private bool sizeDirty = true;
        UserInterface ui;

        public Point WindowSize
        {
            get { return new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight); }
        }

        private Rectangle? _windowRect;
        public bool WindowRectangleChanged
        {
            get
            {
                if (_windowRect == null)
                {
                    _windowRect = Window.ClientBounds;
                    return false;
                }

                if (_windowRect == Window.ClientBounds)
                    return false;
                else
                {
                    _windowRect = Window.ClientBounds;
                    return true;
                }
            }
        }

        public ClientCore()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "mgcontent";
            graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
            graphics.DeviceCreated += graphics_DeviceCreated;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            Window.AllowUserResizing = true;

        }

        public void Resize(int newWidth, int newHeight)
        {
            graphics.PreferredBackBufferHeight = newWidth;
            graphics.PreferredBackBufferWidth = newHeight;
            sizeDirty = true;
        }

        private int _queuedX, _queuedY, _queuedWidth, _queuedHeight;
        private bool _hasQueuedSizeChange;
        public void QueuePositionAndSizeSet(int x, int y, int width, int height)
        {
            _queuedX = x; _queuedY = y;
            _queuedWidth = width;
            _queuedHeight = height;
            _hasQueuedSizeChange = true;
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            sizeDirty = true;
        }

        void graphics_DeviceCreated(object sender, EventArgs e)
        {
        }

        private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            nativeScreenWidth = graphics.PreferredBackBufferWidth;
            nativeScreenHeight = graphics.PreferredBackBufferHeight;

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferMultiSampling = true;
            graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            IsMouseVisible = true;

            QueuePositionAndSizeSet(
                    ClientSettings.Instance.WindowRectangle.Value.X,
                    ClientSettings.Instance.WindowRectangle.Value.Y,
                    ClientSettings.Instance.WindowRectangle.Value.Width,
                    ClientSettings.Instance.WindowRectangle.Value.Height);

            ui = new UserInterface(Content, this);
            GoToMainMenuPage();
        }

        private void GoToMainMenuPage()
        {
            ui.SetPage("mainmenu");
            ui.ActiveBinder.ExitAction = (Action)Exit;
            ui.ActiveBinder.HostAction = (Action)(() =>
            {
                var game = new LiveGame(this, new Networking.Common.Connection.ConnectionInfo
                {
                    IsHost = true
                }, new string[] { });
                game.RegisterExitCallback((g) => GoToMainMenuPage());
                ui.SetPage("mainmenuplayerisingamepage");
                game.Run();
            });
            ui.ActiveBinder.JoinAction = (Action)(() =>
            {
                ui.SetPage("connecttoserverpage");
                ui.ActiveBinder.ConnectAction = (Action)(() =>
                {
                    var game = new LiveGame(this, new Networking.Common.Connection.ConnectionInfo
                    {
                        IsHost = false
                    }, new string[] { });
                    game.RegisterExitCallback((g) => GoToMainMenuPage());
                    ui.SetPage("mainmenuplayerisingamepage");
                    game.Run();
                });
                ui.ActiveBinder.GoBackAction = (Action)(() =>
                {
                    GoToMainMenuPage();
                });
            });
        }

        private void ExitEvent(object parameter)
        {
            Exit();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private float opacity = 1;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (sizeDirty)
            {
                graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
                graphics.ApplyChanges();
                sizeDirty = false;
            }

            if (_hasQueuedSizeChange)
            {
                Window.Position = new Point(_queuedX, _queuedY);
                graphics.PreferredBackBufferWidth = _queuedWidth;
                graphics.PreferredBackBufferHeight = _queuedHeight;
                _hasQueuedSizeChange = false;
                graphics.ApplyChanges();
            }

            if (WindowRectangleChanged)
                ClientSettings.Instance.WindowRectangle.Value = new Rectangle(
                    Window.Position,
                    Window.ClientBounds.Size);

            ui.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(15, 15, 15, 255));

            ui.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
