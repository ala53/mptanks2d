using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Events
{
    public class PlayerKilled
    {
        public Engine.Players.Player KillingPlayer { get; set; }
        public Engine.Players.Player KilledPlayer { get; set; }
        public Engine.Objects.Tanks.Weapons.Weapon Weapon { get; set; }
    }
}
