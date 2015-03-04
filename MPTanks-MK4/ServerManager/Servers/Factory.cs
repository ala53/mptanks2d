using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManager.Servers
{
    static class Factory
    {
        private static List<Server.Server> Servers = new List<Server.Server>();

        public static bool CanCreateServer
        {
            get
            {
                return GetRunningServersCount() < Settings.MaxServers;
            }
        }

        private static int GetRunningServersCount()
        {
            int running = 0;
            foreach (var server in Servers)
                if (server.Running)
                    running++;

            return running;
        }

        public static Server.Server CreateServer(Engine.Gamemodes.Gamemode mode) 
        {

        }
    }
}
