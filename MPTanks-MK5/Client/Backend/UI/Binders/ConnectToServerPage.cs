using EmptyKeys.UserInterface.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.UI.Binders
{
    public class ConnectToServerPage : BinderBase
    {
        private ICommand _goBackButtonCommand;
        public ICommand GoBackButton { get { return _goBackButtonCommand; } }

        private Action _goBackAction = delegate { };
        public Action GoBackAction
        {
            get { return _goBackAction; }
            set
            {
                _goBackAction = value;
                ICommand val = new RelayCommand((o) => _goBackAction());
                SetProperty(ref _goBackButtonCommand, val, nameof(GoBackButton));
            }
        }
        private ICommand _connectCommand;
        public ICommand ConnectButton { get { return _connectCommand; } }

        private Action _connectAction = delegate { };
        public Action ConnectAction
        {
            get { return _connectAction; }
            set
            {
                _connectAction = value;
                ICommand val = new RelayCommand((o) => _connectAction());
                SetProperty(ref _connectCommand, val, nameof(ConnectButton));
            }
        }
        private string _serverAddress = "localhost";
        public string ServerAddress
        {
            get { return _serverAddress; }
            set
            {
                SetProperty(ref _serverAddress, value);
            }
        }
        private string _serverPassword = "<none>";
        public string ServerPassword
        {
            get { return _serverPassword; }
            set
            {
                SetProperty(ref _serverPassword, value, nameof(ServerPassword));
            }
        }

        public string Address
        {
            get
            {
                return ServerAddress.Split(':')[0];
            }
        }
        public ushort Port
        {
            get
            {
                try
                {
                    return ushort.Parse(ServerAddress.Split(':')[1]);
                }
                catch
                {
                    return 33132;
                }

            }
        }
    }
}
