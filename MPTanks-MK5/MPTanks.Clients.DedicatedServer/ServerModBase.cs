using MPTanks.Networking.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.DedicatedServer
{
    public abstract class ServerModBase
    {
        public Server Server { get; set; }
        public ServerModBase(Server server) { }
        
    }
}
