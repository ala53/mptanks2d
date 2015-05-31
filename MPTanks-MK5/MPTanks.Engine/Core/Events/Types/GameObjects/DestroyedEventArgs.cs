using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Core.Events.Types.GameObjects
{
    public class DestroyedEventArgs
    {
        public GameObject Destroyed { get; set; }
        public GameObject Destroyer { get; set; }
        public DateTime Time { get; set; }
    }
}
