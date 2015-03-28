using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.GameClient
{
    class ClientSettings
    {
        public static string LogLocation = "${basedir}/clientlogs/client.log";
        /// <summary>
        /// The maximum number of on screen particles to allow
        /// </summary>
        public static int MaxParticlesToRender = 10000;

        /// <summary>
        /// Whether to force a gen 0 GC every frame. This gets rid of
        /// most temporary objects, helping to trace actual memory leaks.
        /// Unfortunately, it does so to the possible detriment of 
        /// framerate.
        /// </summary>
        public static bool ForceFullGCEveryFrame = false;
        public static bool ForceGen0GCEveryFrame = false;
    }
}
