using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.UI.Binders
{
    public class MainMenu : BinderBase
    {
        public Action ExitAction { get; set; }
        private RelayCommand _exitCommand;
        public ICommand ExitCommand { get { return _exitCommand; } }

        private Action _hostAction;
        public Action HostAction
        {
            get { return _hostAction; }
            set
            {
                _hostAction = value;
                _hostCommand = new RelayCommand((o) => _hostAction());
            }
        }
        private RelayCommand _hostCommand;
        public ICommand HostGameCommand { get { return _hostCommand; } }


        private Action _joinAction;
        public Action JoinAction
        {
            get { return _hostAction; }
            set
            {
                _joinAction = value;
                _joinCommand = new RelayCommand((o) => _joinAction());
            }
        }
        private RelayCommand _joinCommand;
        public ICommand JoinGameCommand { get { return _joinCommand; } }


        private Action _settingsAction;
        public Action SettingsAction
        {
            get { return _hostAction; }
            set
            {
                _settingsAction = value;
                _settingsCommand = new RelayCommand((o) => _settingsAction());
            }
        }
        private RelayCommand _settingsCommand;
        public ICommand SettingsCommand { get { return _settingsCommand; } }

        public MainMenu()
        {
            _exitCommand = new RelayCommand((obj) =>
            {
                Owner.UserInterface.ShowMessageBox(
                    "Exit", "Are you sure you want to exit to desktop?",
                    UserInterface.MessageBoxType.WarningMessageBox,
                    UserInterface.MessageBoxButtons.YesNo,
                    (res) =>
                    {
                        if (res == UserInterface.MessageBoxResult.Yes)
                            ExitAction();
                    });
            });

            HostAction = delegate { };
            JoinAction = delegate { };
            SettingsAction = delegate { };
        }
    }
}
