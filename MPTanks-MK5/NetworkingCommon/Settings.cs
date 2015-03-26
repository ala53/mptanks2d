using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common
{
    public class Settings
    {
        public static readonly IReadOnlyCollection<string> MasterServers = 
            Array.AsReadOnly(new[] {
                "mserv1.zsbgames.tk",
                "mserv2.zsbgames.tk"
            });
    }
}
