//using EmptyKeys.UserInterface.Input;
//using EmptyKeys.UserInterface.Mvvm;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework;
//using EmptyKeys.UserInterface.Media;
//using EmptyKeys.UserInterface;

//namespace MPTanks.Client.Backend.UI.Binders
//{
//    public class MainMenu : BinderBase
//    {
//        public Action ExitAction { get; set; }
//        private RelayCommand _exitCommand;
//        public ICommand ExitCommand { get { return _exitCommand; } }

//        private Action _hostAction;
//        public Action HostAction
//        {
//            get { return _hostAction; }
//            set
//            {
//                _hostAction = value;
//                _hostCommand = new RelayCommand((o) => _hostAction());
//            }
//        }
//        private RelayCommand _hostCommand;
//        public ICommand HostGameCommand { get { return _hostCommand; } }


//        private Action _joinAction;
//        public Action JoinAction
//        {
//            get { return _hostAction; }
//            set
//            {
//                _joinAction = value;
//                _joinCommand = new RelayCommand((o) => _joinAction());
//            }
//        }
//        private RelayCommand _joinCommand;
//        public ICommand JoinGameCommand { get { return _joinCommand; } }


//        private Action _settingsAction;
//        public Action SettingsAction
//        {
//            get { return _hostAction; }
//            set
//            {
//                _settingsAction = value;
//                _settingsCommand = new RelayCommand((o) => _settingsAction());
//            }
//        }
//        private RelayCommand _settingsCommand;
//        public ICommand SettingsCommand { get { return _settingsCommand; } }

//        private SolidColorBrush _backgroundBrush;
//        private SolidColorBrush _titleBrush;
//        EmptyKeys.UserInterface.Generated.MainMenu _mainMenu =>
//            (EmptyKeys.UserInterface.Generated.MainMenu)(Owner.Page);
//        private Random _random;
//        public MainMenu()
//        {
//            _exitCommand = new RelayCommand((obj) =>
//            {
//                Owner.UserInterface.ShowMessageBox(
//                    "Exit", "Are you sure you want to exit to desktop?",
//                    UserInterface.MessageBoxType.WarningMessageBox,
//                    UserInterface.MessageBoxButtons.YesNo,
//                    (res) =>
//                    {
//                        if (res == UserInterface.MessageBoxResult.Yes)
//                            ExitAction();
//                    });
//            });

//            HostAction = delegate { };
//            JoinAction = delegate { };
//            SettingsAction = delegate { };

//            _backgroundBrush = new SolidColorBrush();
//            _titleBrush = new SolidColorBrush();

//            _random = new Random();
//        }

//        private Color _target = Color.White;
//        private Color _lastTarget = Color.Yellow;
//        private double _lastSecond = -1;
//        public override void Update(GameTime gameTime)
//        {
//            _backgroundBrush.Color = new ColorW(Color.Lerp(Color.SlateGray, Color.LightSlateGray,
//                (float)Math.Abs(
//                    Math.Sin(gameTime.TotalGameTime.TotalSeconds / 2) *
//                    Math.Cos(gameTime.TotalGameTime.TotalSeconds / 3)
//                    )).PackedValue);

//            Owner.Page.Background = _backgroundBrush;

//            if (Math.Floor(_lastSecond / 4) != Math.Floor(gameTime.TotalGameTime.TotalSeconds / 4))
//            {
//                _lastSecond = Math.Floor(gameTime.TotalGameTime.TotalSeconds);
//                _lastTarget = _target;

//                switch (_random.Next(9))
//                {
//                    case 1:
//                        _target = Color.White;
//                        break;
//                    case 2:
//                        _target = Color.Yellow;
//                        break;
//                    case 3:
//                        _target = Color.LimeGreen;
//                        break;
//                    case 4:
//                        _target = Color.SkyBlue;
//                        break;
//                    case 5:
//                        _target = Color.Red;
//                        break;
//                    case 6:
//                        _target = Color.MonoGameOrange;
//                        break;
//                }
//            }
//            var color = Color.Lerp(_lastTarget, _target,
//                (float)((gameTime.TotalGameTime.TotalSeconds - _lastSecond) / 4));
//            _titleBrush.Color = new ColorW(color.R, color.G, color.B);

//            _mainMenu.Title.Foreground = _titleBrush;
//            _mainMenu.Subtitle.Foreground = _titleBrush;
//            _mainMenu.HostButton.Foreground = _titleBrush;
//            _mainMenu.JoinButton.Foreground = _titleBrush;
//            _mainMenu.SettingsButton.Foreground = _titleBrush;
//            _mainMenu.ExitButton.Foreground = _titleBrush;

//            base.Update(gameTime);
//        }

//        public override void Recreated()
//        {
//            _titleBrush = new SolidColorBrush();
//            _backgroundBrush = new SolidColorBrush();
//        }
//    }
//}
