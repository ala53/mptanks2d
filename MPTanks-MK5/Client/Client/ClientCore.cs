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
using System.Linq;
using System.Dynamic;

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

        private LiveGame _activeGame;

        private bool sizeDirty = true;
        private GlitchShader _glitch;
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
            Content.RootDirectory = "assets/mgcontent";
            graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
            graphics.DeviceCreated += graphics_DeviceCreated;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            Window.AllowUserResizing = true;

            Window.Title = "MP Tanks 2D: " + GameSandbox.TitleCard.Option();
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

            _glitch = new GlitchShader(this);

            if (GlobalSettings.Instance.NoLoginMode)
            {
                ShowMainMenu();
                return;
            }

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
            _glitch.Enabled = false;
            ui.GoToPage("loginform", (page) =>
            {
                page.Element<TextBox>("PasswordBox").KeyDown += (a, b) =>
                { if (b.Key == KeyCode.Enter) DoLogin(page); };
                page.Element<TextBox>("UsernameBox").KeyDown += (a, b) =>
                { if (b.Key == KeyCode.Enter) DoLogin(page); };

                page.Element<Button>("LoginBtn").Click += (a, b) => DoLogin(page);
                page.Element<Button>("ForgotPasswordBtn").Click += (a, b) =>
                {
                    string username = page.Element<TextBox>("UsernameBox").Text;
                    if (username.Length < 1)
                    {
                        ui.ShowMessageBox("Error", "You must enter your email address!",
                            UserInterface.MessageBoxType.ErrorMessageBox, UserInterface.MessageBoxButtons.Ok);
                        return;
                    }

                    try
                    {
                        ZSB.DrmClient.Account.SendForgotPasswordEmail(username);
                        ui.ShowMessageBox("Sent!", "An email with a link to reset your password has been sent. See you soon!");
                    }
                    catch (AggregateException exception)
                    {
                        var ex = exception.InnerException;
                        if (ex is ZSB.Drm.Client.Exceptions.AccountDetailsIncorrectException)
                            ui.ShowMessageBox("Error", "There is no account with the email address you entered.",
                                UserInterface.MessageBoxType.ErrorMessageBox);
                        if (ex is ZSB.Drm.Client.Exceptions.InvalidAccountServerResponseException)
                            ui.ShowMessageBox("Error", "An internal error occurred. Try again later or reinstall the game.",
                                UserInterface.MessageBoxType.ErrorMessageBox);
                        if (ex is ZSB.Drm.Client.Exceptions.AccountServerException)
                            ui.ShowMessageBox("Error", "An internal error occurred. Try again later or reinstall the game.",
                                UserInterface.MessageBoxType.ErrorMessageBox);
                        if (ex is ZSB.Drm.Client.Exceptions.UnableToAccessAccountServerException)
                            ui.ShowMessageBox("Offline",
                                "We can't send you a validation email if you're not connected to the internet. Connect and try again.",
                                UserInterface.MessageBoxType.ErrorMessageBox);
                    }
                };
                page.Element<Button>("NoAccountBtn").Click +=
                (a, b) => Process.Start("https://mptanks.zsbgames.me/buy");
            });
        }
        private void DoLogin(UserInterfacePage page)
        {
            string username = page.Element<TextBox>("UsernameBox").Text,
            password = page.Element<TextBox>("PasswordBox").Text;
            if (username.Length < 1 || password.Length < 1)
            {
                ui.ShowMessageBox("Error", "You must enter a username and a password!",
                    UserInterface.MessageBoxType.ErrorMessageBox, UserInterface.MessageBoxButtons.Ok);
                return;
            }
            var login = ZSB.DrmClient.LoginAsync(username, password);
            ui.ShowMessageBox("Processing", "We're logging you in now. This may take a few seconds.",
                buttons: UserInterface.MessageBoxButtons.None);
            login.ContinueWith(result =>
            {
                ui.GoBack();
                if (result.IsFaulted)
                {
                    var ex = result.Exception.InnerException;
                    Logger.Error("Login exception", ex);
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
        }
        private void ShowMainMenu()
        {
            _glitch.Enabled = true;
            ui.GoToPage("mainmenu", page =>
            {
                if (ZSB.DrmClient.Offline)
                {
                    page.Element<TextBlock>("_subtitle").Text += " - Offline Mode";
                    page.Element<TextBlock>("_subtitle").Foreground = new SolidColorBrush(new ColorW(255, 255, 0)); //yellow
                }

                page.Element<Button>("HostBtn").Click += (b, c) =>
                {
                    _activeGame = new LiveGame(this, new Networking.Common.Connection.ConnectionInfo
                    {
                        IsHost = true
                    }, new string[] { });
                    _activeGame.RegisterExitCallback(d => ui.GoBack());
                    ui.GoToPage("mainmenuplayerisingamepage", inGamePage =>
                    {
                        inGamePage.Element<Button>("ForceCloseBtn").Click += (e, f) =>
                        {
                            _activeGame.Close();
                        };
                    });
                    _activeGame.Run();
                };
                page.Element<Button>("JoinBtn").Click += (b, c) =>
                {
                    ui.GoToPage("connecttoserverpage", connectPage =>
                    {
                        connectPage.Element<TextBox>("ServerAddress").Text = ClientSettings.Instance.StoredServerAddress.Value ?? "";

                        var discoveryHeader = connectPage.Element<TextBlock>("DiscoveryHeader");
                        var discoveryPanel = connectPage.Element<StackPanel>("DiscoveryPanel");

                        Action<System.Threading.Tasks.Task<Networking.Client.DiscoveryHelper.DiscoveryResponse[]>>
                        discoveryContinuation = null;
                        discoveryContinuation = a =>
                        {//Handle errors
                            if (!a.IsCompleted)
                            {
                                discoveryHeader.Text = "Error looking for hosts!";
                                discoveryPanel.Children.Clear();
                                return;
                            }
                            //Handle if we're no longer the active page
                            if (ui.CurrentPage != connectPage)
                                return;

                            //Do the UI update
                            discoveryPanel.Children.Clear();
                            foreach (var resp in a.Result)
                            {
                                var panel = new StackPanel();
                                var moreInfoPanel = new StackPanel();
                                bool expanded = false;
                                var expandBtn = new Button
                                {
                                    FontFamily = new FontFamily("JHUF"),
                                    FontSize = 12,
                                    Content = "More",
                                    Padding = new Thickness(10, 5, 10, 5),
                                    Margin = new Thickness(5, 0, 5, 0)
                                };

                                expandBtn.Click += (m, q) =>
                                {
                                    if (expanded)
                                    {
                                        moreInfoPanel.Visibility = Visibility.Collapsed;
                                        expandBtn.Content = "More";
                                        expanded = false;
                                    }
                                    else
                                    {
                                        moreInfoPanel.Visibility = Visibility.Visible;
                                        expandBtn.Content = "Less";
                                        expanded = true;
                                    }
                                };

                                var joinBtn = new Button
                                {
                                    FontFamily = new FontFamily("JHUF"),
                                    FontSize = 12,
                                    Content = "Join",
                                    Padding = new Thickness(10, 5, 10, 5),
                                    Margin = new Thickness(5, 0, 5, 0)
                                };

                                var buttonsPanel = new StackPanel();
                                buttonsPanel.Orientation = Orientation.Horizontal;
                                buttonsPanel.Children.Add(expandBtn);
                                buttonsPanel.Children.Add(joinBtn);

                                panel.Children.Add(new TextBlock
                                {
                                    FontFamily = new FontFamily("JHUF"),
                                    Text = resp.ServerName,
                                    FontSize = 16,
                                    Foreground = Brushes.White
                                });

                                panel.Children.Add(new TextBlock
                                {
                                    FontFamily = new FontFamily("JHUF"),
                                    Text = resp.Address + ":" + resp.Port,
                                    FontSize = 12,
                                    Foreground = Brushes.Gray
                                });

                                panel.Children.Add(buttonsPanel);

                                discoveryPanel.Children.Add(panel);
                            }

                            //Otherwise, update and do another round of discovery
                            Networking.Client.DiscoveryHelper.DoDiscoveryAsync().ContinueWith(discoveryContinuation);
                        };

                        Networking.Client.DiscoveryHelper.DoDiscoveryAsync().ContinueWith(discoveryContinuation);

                        connectPage.Element<Button>("GoBackBtn").Click += (e, f) => ui.GoBack();
                        connectPage.Element<Button>("ConnectBtn").Click += (e, f) =>
                        {
                            //Save server URL into settings
                            ClientSettings.Instance.StoredServerAddress.Value = connectPage.Element<TextBox>("ServerAddress").Text;

                            var unparsedAddress = connectPage.Element<TextBox>("ServerAddress").Text;
                            ushort port = 33132;
                            string address = unparsedAddress.Split(':')[0];
                            try { port = ushort.Parse(unparsedAddress.Split(':')[1]); } catch { }

                            _activeGame = new LiveGame(this, new Networking.Common.Connection.ConnectionInfo
                            {
                                IsHost = false,
                                ServerAddress = address,
                                ServerPort = port,
                                Password = connectPage.Element<TextBox>("ServerPassword").Text
                            }, new string[] { });

                            _activeGame.RegisterExitCallback((g) =>
                            {
                                ui.GoBack();
                            });
                            ui.GoToPage("mainmenuplayerisingamepage", inGamePage =>
                            {
                                inGamePage.Element<Button>("ForceCloseBtn").Click += (h, i) =>
                                {
                                    _activeGame.Close();
                                };
                            });
                            _activeGame.Run();
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
                if (Window.ClientBounds.Width < 800)
                    graphics.PreferredBackBufferWidth = 800;
                if (Window.ClientBounds.Height < 480)
                    graphics.PreferredBackBufferHeight = 480;
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

        private Color[] _possibleBackgroundColors = {
            Color.DarkSlateGray, Color.Blue,
            Color.MonoGameOrange, Color.Black,
            Color.DeepSkyBlue, Color.Aquamarine,
            Color.Red, Color.RosyBrown };
        private Color _beginBackgroundColor, _endBackgroundColor;
        private TimeSpan _backgroundTransitionStart = TimeSpan.FromSeconds(-100);
        private Random _backgroundColorRng = new Random();
        private void BlendBackgroundColor(GameTime gameTime)
        {
            var transitionLength = TimeSpan.FromSeconds(5);
            var amountTransitioned =
                (gameTime.TotalGameTime - _backgroundTransitionStart).TotalSeconds / transitionLength.TotalSeconds;
            if (amountTransitioned > 1)
            {
                _backgroundTransitionStart = gameTime.TotalGameTime;
                amountTransitioned = 0;
                _beginBackgroundColor = _endBackgroundColor;
                _endBackgroundColor =
                    _possibleBackgroundColors[_backgroundColorRng.Next(0, _possibleBackgroundColors.Length)];
            }

            var lerped = Color.Lerp(_beginBackgroundColor, _endBackgroundColor, (float)amountTransitioned);
            ui.CurrentPage.Page.Background = new SolidColorBrush(new ColorW(lerped.R, lerped.G, lerped.B));
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (!IsActive) return;
            BlendBackgroundColor(gameTime);
            _glitch.BeginDraw();
            GraphicsDevice.Clear(new Color(15, 15, 15, 255));

            ui.Draw(gameTime);

            _glitch.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
