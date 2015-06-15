using EmptyKeys.UserInterface.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Rendering.UI.Binders
{
    public class MainMenuPlayerIsInGamePage : BinderBase
    {
        public Action ForceCloseClicked
        {
            set { ForciblyCloseButtonCommand = new RelayCommand((o) => value()); }
        }

        private ICommand _closeCommand;
        public ICommand ForciblyCloseButtonCommand
        {
            get { return _closeCommand; }
            set { SetProperty(ref _closeCommand, value, nameof(ForciblyCloseButtonCommand)); }
        }
    }
}
