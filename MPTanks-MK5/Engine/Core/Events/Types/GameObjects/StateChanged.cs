using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Core.Events.Types.GameObjects
{
    public class StateChanged : EventArgs
    {
        public byte[] StateData {get; set;}
        public GameObject GameObject { get; set; }
    }
}
