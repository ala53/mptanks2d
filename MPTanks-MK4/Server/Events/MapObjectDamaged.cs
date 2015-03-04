using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Events
{
    public class MapObjectDamaged
    {
        public Engine.Players.Player DamagingPlayer { get; set; }
    }
}
