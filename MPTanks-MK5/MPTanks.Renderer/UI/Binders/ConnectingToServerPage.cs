using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Rendering.UI.Binders
{
    public class ConnectingToServerPage : BinderBase
    {
        #region Stuff that is there for the UI Layer and not the user
        public string ConnectingToServer
        {
            get
            {
                return Strings.ClientMenus.ConnectingToServerHeader;
            }
        }

        public string CancelButton
        {
            get
            {
                return Strings.ClientMenus.CancelButton;
            }
        }

        private string _addrLabel;
        public string AddressLabel
        {
            get
            {
                return _addrLabel;
            }
            private set
            {
                SetProperty<string>(ref _addrLabel, value, "AddressLabel");
            }
        }

        public string ReturnToMenu
        {
            get
            {
                return Strings.ClientMenus.ConnectingToServerReturnToMenu;
            }
        }

        public string ConnectionFailed
        {
            get
            {
                return Strings.ClientMenus.ConnectingToServerConnectionFailed;
            }
        }

        private Visibility _failureAreaVisibility;
        public Visibility FailureAreaVisibility
        {
            get
            {
                return _failureAreaVisibility;
            }
            set
            {
                SetProperty<Visibility>(ref _failureAreaVisibility, value, "FailureAreaVisibility");
            }
        }

        private Visibility _cancelAreaVisibility;
        public Visibility CancelAreaVisibility
        {
            get
            {
                return _cancelAreaVisibility;
            }
            set
            {
                SetProperty<Visibility>(ref _cancelAreaVisibility, value, "CancelAreaVisibility");
            }
        }
        #endregion

        private bool _connectFailed;
        /// <summary>
        /// Whether the connection has failed
        /// </summary>
        public bool HasConnectionFailed
        {
            get
            {
                return _connectFailed;
            }
            set
            {
                _connectFailed = value;
                if (_connectFailed)
                {
                    FailureAreaVisibility = Visibility.Visible;
                    CancelAreaVisibility = Visibility.Collapsed;
                }
                else
                {
                    FailureAreaVisibility = Visibility.Collapsed;
                    CancelAreaVisibility = Visibility.Visible;
                }
            }
        }

        private string _failureReason = "null";
        /// <summary>
        /// The reason that the connection failed, if it did.
        /// </summary>
        public string FailureReason
        {
            get
            {
                return _failureReason;
            }
            set
            {
                SetProperty<string>(ref _failureReason, value, "FailureReason");
            }
        }

        private string _connAddr = "null";
        /// <summary>
        /// The connection address
        /// </summary>
        public string ConnectionAddress
        {
            get
            {
                return _connAddr;
            }
            set
            {
                _connAddr = value;
                AddressLabel =
                    Strings.ClientMenus.ConnectingToServerAddress +
                    " " + ConnectionAddress + ":" + Port;
            }
        }

        private int _port;
        /// <summary>
        /// The port that is being connected to
        /// </summary>
        public int Port
        {
            get { return _port; }
            set
            {
                _port = value;
                AddressLabel =
                    Strings.ClientMenus.ConnectingToServerAddress +
                    " " + ConnectionAddress + ":" + Port;
            }
        }

        #region Events
        public event EventHandler OnCancelPressed = delegate { };
        public event EventHandler OnReturnToMenuPressed = delegate { };

        private ICommand _cancelCommand;
        public ICommand CancelButtonCommand
        {
            get
            {
                return _cancelCommand;
            }
            set
            {
                SetProperty<ICommand>(ref _cancelCommand, value, "CancelButtonCommand");
            }
        }

        private ICommand _returnCommand;
        public ICommand ReturnButtonCommand
        {
            get
            {
                return _returnCommand;
            }
            set
            {
                SetProperty<ICommand>(ref _returnCommand, value, "ReturnButtonCommand");
            }
        }
        #endregion

        private EventArgs _args = new EventArgs();
        public ConnectingToServerPage()
        {
            HasConnectionFailed = false;
            CancelButtonCommand = new RelayCommand((obj) => OnCancelPressed(obj, _args));
            ReturnButtonCommand = new RelayCommand((obj) => OnReturnToMenuPressed(obj, _args));
        }
    }
}
