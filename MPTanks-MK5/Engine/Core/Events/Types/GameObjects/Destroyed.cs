using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Core.Events.Types.GameObjects
{
    public class Destroyed : EventArgs
    {
        public GameObject DeadObject { get; set; }
        public GameObject Killer { get; set; }
    }
}
