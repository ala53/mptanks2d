using EmptyKeys.UserInterface.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyKeys.UserInterface.Generated
{
    public partial class MainMenu
    {
        public TextBlock Title => _title;
        public TextBlock Subtitle => _subtitle;
        public Button HostButton => _hostBtn;
        public Button JoinButton => _joinBtn;
        public Button SettingsButton => _settingsBtn;
        public Button ExitButton => _exitBtn;
    }
}
