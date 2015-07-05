using MPTanks.Engine.Gamemodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Core.Events.Types.GameCore
{
    public class EndedEventArgs : EventArgs
    {
        public Team WinningTeam { get; set; }
        public bool IsDraw { get { return WinningTeam == Team.Indeterminate; } }
    }
}
