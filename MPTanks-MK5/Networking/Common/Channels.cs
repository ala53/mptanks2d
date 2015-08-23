using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common
{
    public static class Channels
    {
        public static readonly int GameplayData = 0;
        public static readonly int Login = 1;
        public static readonly int Polling = 2;
    }
}
