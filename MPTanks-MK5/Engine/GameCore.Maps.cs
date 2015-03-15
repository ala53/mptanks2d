using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public partial class GameCore
    {
        /// <summary>
        /// Forcibly loads the map, ignoring the authoritative status of the server
        /// </summary>
        public void ForceMapLoad()
        {
            CreateMapObjects(true);
        }

        private void CreateMapObjects(bool authorized = false)
        {

        }
    }
}
