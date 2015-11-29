using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.UI.Binders
{
    public class SettingUpPrompt : BinderBase
    {
        private string _header;
        public string Header
        {
            get
            {
                return _header;
            }
            set
            {
                SetProperty(ref _header, value);
            }
        }
        private string _content;
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                SetProperty(ref _content, value);
            }
        }
        private string _controlButtonText = "";
        public string ControlButtonText
        {
            get
            {
                return _controlButtonText ?? "";
            }
            set
            {
                SetProperty(ref _controlButtonText, value);
            }
        }

        public Action ControlCommand
        {
            set
            {
                if (value == null)
                {
                    ControlButtonVisibility = Visibility.Collapsed;
                }
                else
                {
                    ControlButtonVisibility = Visibility.Visible;
                    ControlButtonCommand = new RelayCommand(a => value());
                }
            }
        }

        private RelayCommand _controlCmd;
        public RelayCommand ControlButtonCommand
        {
            get { return _controlCmd; }
            set { SetProperty(ref _controlCmd, value); }
        }

        private Visibility _controlVisibility;
        public Visibility ControlButtonVisibility
        {
            get
            {
                return _controlVisibility;
            }
            set
            {
                SetProperty(ref _controlVisibility, value);
            }
        }
        public SettingUpPrompt()
        {
            ControlButtonVisibility = Visibility.Collapsed;
        }
    }
}
