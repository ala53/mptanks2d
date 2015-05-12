using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Core.Events.Types.Game
{
    public class Started : EventArgs
    {
        public GameCore Game { get; set; }
        public DateTime StartTime { get; set; }
    }
}
