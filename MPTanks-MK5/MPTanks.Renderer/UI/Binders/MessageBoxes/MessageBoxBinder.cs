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
    public class MessageBoxBinder : BinderBase
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
                SetProperty(ref _header, value, "Header");
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
                SetProperty(ref _content, value, "Content");
            }
        }

        private UserInterface.MessageBoxButtons _buttons;
        public UserInterface.MessageBoxButtons Buttons
        {
            get
            {
                return _buttons;
            }
            set
            {
                _buttons = value;
                CancelButtonVisibility = Visibility.Collapsed;
                YesButtonVisibility = Visibility.Collapsed;
                NoButtonVisibility = Visibility.Collapsed;
                OkButtonVisibility = Visibility.Collapsed;
                switch (_buttons)
                {
                    case UserInterface.MessageBoxButtons.Ok:
                        OkButtonVisibility = Visibility.Visible;
                        break;
                    case UserInterface.MessageBoxButtons.OkCancel:
                        OkButtonVisibility = Visibility.Visible;
                        CancelButtonVisibility = Visibility.Visible;
                        break;
                    case UserInterface.MessageBoxButtons.OkNo:
                        OkButtonVisibility = Visibility.Visible;
                        NoButtonVisibility = Visibility.Visible;
                        break;
                    case UserInterface.MessageBoxButtons.OkNoCancel:
                        OkButtonVisibility = Visibility.Visible;
                        NoButtonVisibility = Visibility.Visible;
                        CancelButtonVisibility = Visibility.Visible;
                        break;
                    case UserInterface.MessageBoxButtons.YesNo:
                        YesButtonVisibility = Visibility.Visible;
                        NoButtonVisibility = Visibility.Visible;
                        break;
                    case UserInterface.MessageBoxButtons.YesNoCancel:
                        YesButtonVisibility = Visibility.Visible;
                        NoButtonVisibility = Visibility.Visible;
                        CancelButtonVisibility = Visibility.Visible;
                        break;
                }
            }
        }

        private Visibility _cancelBtn;
        public Visibility CancelButtonVisibility
        {
            get
            {
                return _cancelBtn;
            }
            private set
            {
                SetProperty(ref _cancelBtn, value, "CancelButtonVisibility");
            }
        }

        private Visibility _okBtn;
        public Visibility OkButtonVisibility
        {
            get
            {
                return _okBtn;
            }
            private set
            {
                SetProperty(ref _okBtn, value, "OkButtonVisibility");
            }
        }

        private Visibility _yesBtn;
        public Visibility YesButtonVisibility
        {
            get
            {
                return _yesBtn;
            }
            private set
            {
                SetProperty(ref _yesBtn, value, "YesButtonVisibility");
            }
        }

        private Visibility _noBtn;
        public Visibility NoButtonVisibility
        {
            get
            {
                return _noBtn;
            }
            private set
            {
                SetProperty(ref _noBtn, value, "NoButtonVisibility");
            }
        }

        private RelayCommand _okCommand;
        public RelayCommand OkCommand
        {
            get
            {
                return _okCommand;
            }
            set
            {
                SetProperty(ref _okCommand, value, "OkCommand");
            }
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand;
            }
            set
            {
                SetProperty(ref _cancelCommand, value, "CancelCommand");
            }
        }

        private RelayCommand _yesCommand;
        public RelayCommand YesCommand
        {
            get
            {
                return _yesCommand;
            }
            set
            {
                SetProperty(ref _yesCommand, value, "YesCommand");
            }
        }

        private RelayCommand _noCommand;
        public RelayCommand NoCommand
        {
            get
            {
                return _noCommand;
            }
            set
            {
                SetProperty(ref _noCommand, value, "NoCommand");
            }
        }

    }
}
