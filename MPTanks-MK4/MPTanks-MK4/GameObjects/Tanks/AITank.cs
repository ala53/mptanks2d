using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4.GameObjects.Tanks
{
    /// <summary>
    /// A server only class to have tanks that are AI controlled.
    /// </summary>
    class AITank : Tank
    {
        public Tank CreateUserTank()
        {
            throw new NotImplementedException();
        }
    }
}
