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
    }
}
