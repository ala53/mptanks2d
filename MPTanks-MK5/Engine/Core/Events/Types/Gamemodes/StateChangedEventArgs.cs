using MPTanks.Engine.Gamemodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Core.Events.Types.Gamemodes
{
    public class StateChangedEventArgs : EventArgs
    {
        public Gamemode Gamemode { get; set; }
        public byte[] State { get; set; }
    }
}
