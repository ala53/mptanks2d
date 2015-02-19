using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4.StateManagement.States
{
    class GameState
    {
        public ObjectState[] ChangedObjects;
        public CreatedTank[] NewTanks;
        public CreatedMapObject[] NewMapObjects;
        public ushort[] RemovedObjects;
    }
}
