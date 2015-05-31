using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Rendering.UI.Binders
{
    public class MainMenu : BinderBase
    {
        public Action ExitAction { get; set; }
        private RelayCommand _exitCommand;
        public ICommand ExitCommand
        {
            get
            {
                return _exitCommand;
            }
        }

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
        }
    }
}
