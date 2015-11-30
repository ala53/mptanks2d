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
using System.Diagnostics;

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

            ui = new UserInterface(this);
            ui.GoToPage("mainmenu", a =>
            {
                a.Element<Button>("HostBtn").Click += (b, c) =>
                {
                    var game = new LiveGame(this, new Networking.Common.Connection.ConnectionInfo
                    {
                        IsHost = true
                    }, new string[] { });
                    game.RegisterExitCallback(d => ui.GoBack());
                    ui.GoToPage("mainmenuplayerisingamepage", d =>
                    {
                        d.Element<Button>("ForceCloseBtn").Click += (e, f) =>
                        {
                            game.Close();
                            ui.GoBack();
                        };
                    });
                    game.Run();
                };
                a.Element<Button>("JoinBtn").Click += (b, c) =>
                {
                    ui.GoToPage("connecttoserverpage", d =>
                    {
                        d.Element<Button>("GoBackBtn").Click += (e, f) => ui.GoBack();
                        d.Element<Button>("ConnectBtn").Click += (e, f) =>
                        {
                            var unparsedAddress = d.Element<TextBox>("ServerAddress").Text;
                            ushort port = 33132;
                            string address = unparsedAddress.Split(':')[0];
                            try { port = ushort.Parse(unparsedAddress.Split(':')[1]); } catch { }

                            var game = new LiveGame(this, new Networking.Common.Connection.ConnectionInfo
                            {
                                IsHost = false,
                                ServerAddress = address,
                                ServerPort = port,
                                Password = d.Element<TextBox>("ServerPassword").Text
                            }, new string[] { });
                            game.RegisterExitCallback((g) => ui.GoBack());
                            ui.GoToPage("mainmenuplayerisingamepage", g =>
                            {
                                g.Element<Button>("ForceCloseBtn").Click += (h, i) =>
                                {
                                    game.Close();
                                    ui.GoBack();
                                };
                            });
                            game.Run();
                        };
                    });

                };

                a.Element<Button>("MapMakerBtn").Click += (b, c) => {
                    var prc = Process.Start("MPTanks.Clients.MapMaker.exe");

                    ui.GoToPage("mainmenuplayerisingamepage", g =>
                    {
                        g.Element<Button>("ForceCloseBtn").Click += (h, i) =>
                        {
                            prc.Close();
                            ui.GoBack();
                        };
                    });

                    prc.Exited += (d, e) => { ui.GoBack(); };
                };
                a.Element<Button>("ExitBtn").Click += (b, c) =>
                {
                    ui.ShowMessageBox("Exit?", "Are you sure you wish to exit?",
                        UserInterface.MessageBoxType.WarningMessageBox,
                        UserInterface.MessageBoxButtons.YesNo, d =>
                        { if (d == UserInterface.MessageBoxResult.Yes) Exit(); });
                };
            });
        }

        //private void GoToMainMenuPage()
        //{
        //    ui.GoToPage("mainmenu");
        //    ui.ActiveBinder.ExitAction = (Action)Exit;
        //    ui.ActiveBinder.HostAction = (Action)(() =>
        //    {
        //        var game = new LiveGame(this, new Networking.Common.Connection.ConnectionInfo
        //        {
        //            IsHost = true
        //        }, new string[] { });
        //        game.RegisterExitCallback(a => ui.GoBack());
        //        ui.GoToPage("mainmenuplayerisingamepage");
        //        game.Run();
        //    });
        //    ui.ActiveBinder.JoinAction = (Action)(() =>
        //    {
        //        ui.GoToPage("connecttoserverpage");
        //        ui.ActiveBinder.ConnectAction = (Action)(() =>
        //        {
        //            var game = new LiveGame(this, new Networking.Common.Connection.ConnectionInfo
        //            {
        //                IsHost = false,
        //                ServerAddress = ui.ActiveBinder.Address,
        //                ServerPort = ui.ActiveBinder.Port,
        //                Password = ui.ActiveBinder.ServerPassword
        //            }, new string[] { });
        //            game.RegisterExitCallback((g) => ui.GoBack());
        //            ui.GoToPage("mainmenuplayerisingamepage");
        //            game.Run();
        //        });
        //        ui.ActiveBinder.GoBackAction = (Action)ui.GoBack;
        //    });
        //}

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
