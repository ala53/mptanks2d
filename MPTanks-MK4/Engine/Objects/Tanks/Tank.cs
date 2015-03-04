using Engine.Objects.Tanks.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Objects.Tanks
{
    class Tank
    {
        public TankWeapon PrimaryWeapon { get; private set; }
        public TankWeapon SecondaryWeapon { get; private set; }
        public TankWeapon TertiaryWeapon { get; private set; }
    }
}
