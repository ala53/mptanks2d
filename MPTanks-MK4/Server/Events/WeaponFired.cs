using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Events
{
    public class WeaponFired
    {
        public Engine.Objects.Tanks.Weapons.Weapon Weapon { get; set; }
        public Engine.Players.Player FiringPlayer { get; set; }
    }
}
