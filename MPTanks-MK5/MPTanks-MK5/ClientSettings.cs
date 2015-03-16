using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK5
{
    class ClientSettings
    {
        public static string LogLocation = "${basedir}/client.log";
        /// <summary>
        /// The maximum number of on screen particles to allow
        /// </summary>
        public const int MaxParticlesToRender = 10000;
    }
}
