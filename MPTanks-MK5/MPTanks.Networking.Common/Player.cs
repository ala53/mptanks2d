using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Engine.Tanks;

namespace MPTanks.Networking.Common
{
    public class NetworkPlayer : GamePlayer
    {
        public bool HasCustomTankStyle { get; set; }
        public override Tank Tank
        {
            get
            {
                return base.Tank;
            }

            set
            {
                base.Tank = value;
                    ApplyTankStyles();
            }
        }

        public void ApplyTankStyles()
        {

        }
    }
}
