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

        /// <summary>
        /// Whether to force a gen 0 GC every frame. This gets rid of
        /// most temporary objects, helping to trace actual memory leaks.
        /// Unfortunately, it does so to the possible detriment of 
        /// framerate.
        /// </summary>
        public const bool ForceGCEveryFrame = true;
    }
}
