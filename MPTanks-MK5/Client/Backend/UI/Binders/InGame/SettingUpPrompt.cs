using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.UI.Binders
{
    public class SettingUpPrompt : BinderBase
    {
        private string _secondsRemaining;
        public string SecondsRemainingText
        {
            get
            {
                return _secondsRemaining;
            }
            set
            {
                SetProperty(ref _secondsRemaining, value, nameof(SecondsRemainingText));
            }
        }

        private TimeSpan _time;
        public TimeSpan TimeRemaining
        {
            get { return _time; }
            set
            {
                _time = value;
                SecondsRemainingText = $"{_time.TotalSeconds.ToString("N1")} seconds remaining";
            }
        }
    }
}
