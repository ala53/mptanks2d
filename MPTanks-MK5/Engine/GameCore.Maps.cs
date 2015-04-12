using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine
{
    public partial class GameCore
    {
        /// <summary>
        /// Forcibly loads the map, ignoring the authoritative status of the server
        /// </summary>
        public void ForceMapLoad()
        {
            CreateMapObjects();
        }

        private void CreateMapObjects()
        {
            Map.CreateObjects();
        }
    }
}
