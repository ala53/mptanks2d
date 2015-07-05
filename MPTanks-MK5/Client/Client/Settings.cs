using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client
{
    static class Settings
    {
        public static string FilePathForInGameAssembly = typeof(MPTanks.Client.GameSandbox.GameClient).Assembly.Location;
    }
}
