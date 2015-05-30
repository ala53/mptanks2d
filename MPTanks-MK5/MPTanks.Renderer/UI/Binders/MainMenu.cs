using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Rendering.UI.Binders
{
    class MainMenu : ViewModelBase
    {
        public Action Exit { get; private set; }
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
            _exitCommand = new RelayCommand((obj) => { Exit(); });
        }
    }
}
