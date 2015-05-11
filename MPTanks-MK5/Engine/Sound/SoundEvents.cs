using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Sound
{
    public class SoundStartedEvent : EventArgs
    {
        public Sound Sound { get; set; }
    }
    public class SoundStoppedEvent : EventArgs
    {
        public Sound Sound { get; set; }
    }
    public class SoundChangedEvent : EventArgs
    {
        public Sound Sound { get; set; }
    }
}
