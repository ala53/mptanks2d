using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MPTanks.Client.Backend.UI;
using MPTanks.Engine.Settings;
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

            //Init
            try
            { ZSB.DrmClient.Initialize(GlobalSettings.Instance.StoredAccountInfo); }
            catch
            { ZSB.DrmClient.Initialize(); } //If an error occurs, clear info and restart
            ZSB.DrmClient.OnPersistentStorageChanged +=
                (a, b) => GlobalSettings.Instance.StoredAccountInfo.Value = ZSB.DrmClient.PersistentData;

            if (ZSB.DrmClient.LoggedIn)
                ShowMainMenu();
            else ShowLoginPage();

        }
        private void ShowLoginPage()
        {
            ui.GoToPage("loginform", page =>
            {
                page.Element<Button>("LoginBtn").Click += (a, b) =>
                {
                    var login = ZSB.DrmClient.LoginAsync(
                        page.Element<TextBox>("UsernameBox").Text,
                        page.Element<TextBox>("PasswordBox").Text);
                    login.ContinueWith(result =>
                    {
                        if (result.IsFaulted)
                        {
                            var ex = result.Exception.InnerException;
                            if (ex is ZSB.Drm.Client.Exceptions.AccountDetailsIncorrectException)
                                ui.ShowMessageBox("Error", "The username or password you entered was incorrect.",
                                    UserInterface.MessageBoxType.ErrorMessageBox, UserInterface.MessageBoxButtons.Ok);
                            if (ex is ZSB.Drm.Client.Exceptions.AccountEmailNotConfirmedException)
                                ui.ShowMessageBox("Error",
                                    "You must confirm the email address on the account before you can log in.",
                                    UserInterface.MessageBoxType.ErrorMessageBox, UserInterface.MessageBoxButtons.Ok);
                            if (ex is ZSB.Drm.Client.Exceptions.AccountServerException)
                                ui.ShowMessageBox("Error",
                                    "An internal error occurred. Try reinstalling the game or waiting a bit.",
                                    UserInterface.MessageBoxType.ErrorMessageBox, UserInterface.MessageBoxButtons.Ok);
                            if (ex is ZSB.Drm.Client.Exceptions.InvalidAccountServerResponseException)
                                ui.ShowMessageBox("Error",
                                    "An internal error occurred. Try reinstalling the game or waiting a bit.",
                                    UserInterface.MessageBoxType.ErrorMessageBox, UserInterface.MessageBoxButtons.Ok);
                            if (ex is ZSB.Drm.Client.Exceptions.UnableToAccessAccountServerException)
                                ui.ShowMessageBox("You're offline",
                                    "To log in, you must be connected to the internet. Please connect and try again.",
                                    UserInterface.MessageBoxType.ErrorMessageBox, UserInterface.MessageBoxButtons.Ok);
                        }

                        var res = result.Result;
                        if (res.FullUserInfo.Owns(Networking.Common.StaticSettings.MPTanksProductId))
                            ShowMainMenu();
                        else
                            ui.ShowMessageBox("Oh no!", "You don't seem to own MP Tanks. " +
                                "Click OK to go to the ZSB Store page or click cancel to close the game.",
                                UserInterface.MessageBoxType.WarningMessageBox,
                                UserInterface.MessageBoxButtons.OkCancel, (cb) =>
                                {
                                    switch (cb)
                                    {
                                        case UserInterface.MessageBoxResult.Ok:
                                            Process.Start("https://mptanks.zsbgames.me/buy");
                                            Exit();
                                            break;
                                        case UserInterface.MessageBoxResult.Cancel:
                                            Exit();
                                            break;
                                    }
                                });
                    });
                };
                page.Element<Button>("ForgotPasswordBtn").Click += (a, b) => { };
                page.Element<Button>("NoAccountBtn").Click +=
                (a, b) => Process.Start("https://mptanks.zsbgames.me/buy");
            });
        }
        private void ShowMainMenu()
        {
            ui.GoToPage("mainmenu", page =>
            {
                if (ZSB.DrmClient.Offline)
                {
                    page.Element<TextBlock>("_subtitle").Text += " - Offline Mode";
                    page.Element<TextBlock>("_subtitle").Foreground = new SolidColorBrush(new ColorW(255, 255, 0)); //yellow
                }

                page.Element<Button>("HostBtn").Click += (b, c) =>
                {
                    var game = new LiveGame(this, new Networking.Common.Connection.ConnectionInfo
                    {
                        IsHost = true
                    }, new string[] { });
                    game.RegisterExitCallback(d => ui.GoBack());
                    ui.GoToPage("mainmenuplayerisingamepage", inGamePage =>
                    {
                        inGamePage.Element<Button>("ForceCloseBtn").Click += (e, f) =>
                        {
                            game.Close();
                            ui.GoBack();
                        };
                    });
                    game.Run();
                };
                page.Element<Button>("JoinBtn").Click += (b, c) =>
                {
                    ui.GoToPage("connecttoserverpage", connectPage =>
                    {
                        connectPage.Element<Button>("GoBackBtn").Click += (e, f) => ui.GoBack();
                        connectPage.Element<Button>("ConnectBtn").Click += (e, f) =>
                        {
                            var unparsedAddress = connectPage.Element<TextBox>("ServerAddress").Text;
                            ushort port = 33132;
                            string address = unparsedAddress.Split(':')[0];
                            try { port = ushort.Parse(unparsedAddress.Split(':')[1]); } catch { }

                            var game = new LiveGame(this, new Networking.Common.Connection.ConnectionInfo
                            {
                                IsHost = false,
                                ServerAddress = address,
                                ServerPort = port,
                                Password = connectPage.Element<TextBox>("ServerPassword").Text
                            }, new string[] { });

                            game.RegisterExitCallback((g) => ui.GoBack());
                            ui.GoToPage("mainmenuplayerisingamepage", inGamePage =>
                            {
                                inGamePage.Element<Button>("ForceCloseBtn").Click += (h, i) =>
                                {
                                    game.Close();
                                    ui.GoBack();
                                };
                            });
                            game.Run();
                        };
                    });

                };

                page.Element<Button>("MapMakerBtn").Click += (b, c) =>
                {
                    var prc = Process.Start("MPTanks.Clients.MapMaker.exe");

                    ui.GoToPage("mainmenuplayerisingamepage", inGamePage =>
                    {
                        inGamePage.Element<Button>("ForceCloseBtn").Click += (h, i) =>
                        {
                            prc.Close();
                            ui.GoBack();
                        };
                    });

                    prc.Exited += (d, e) => { ui.GoBack(); };
                };
                page.Element<Button>("ExitBtn").Click += (b, c) =>
                {
                    ui.ShowMessageBox("Exit?", "Are you sure you wish to exit?",
                        UserInterface.MessageBoxType.WarningMessageBox,
                        UserInterface.MessageBoxButtons.YesNo, d =>
                        { if (d == UserInterface.MessageBoxResult.Yes) Exit(); });
                };


                //Col. 2
                page.Element<Button>("LogOutBtn").Click += (b, c) =>
                  {
                      ZSB.DrmClient.LogOut();
                      ui.UnwindAndEmpty();
                      ShowLoginPage();
                  };
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
