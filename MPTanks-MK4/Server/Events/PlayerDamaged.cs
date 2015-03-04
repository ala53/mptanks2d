using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Events
{
    public class PlayerDamaged
    {
        public Engine.Players.Player DamagingPlayer { get; set; }
        public Engine.Players.Player DamagedPlayer { get; set; }
        public int HPLoss { get; set; }
    }
}
