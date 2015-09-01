using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server
{
    public class Configuration
    {
        public int MaxPlayers { get; set; } = 32;
        public ushort Port { get; set; } = 33132;
        public string Password { get; set; }
        public TimeSpan StateSyncRate { get; set; } = TimeSpan.FromSeconds(1);
    }

    public class InitializedConfiguration
    {
        public int MaxPlayers { get; private set; }
        public ushort Port { get; private set; }
        public string Password { get; set; }
        public  TimeSpan StateSyncRate { get; private set; }
        internal InitializedConfiguration(Configuration configuration)
        {
            MaxPlayers = configuration.MaxPlayers;
            Port = configuration.Port;
            Password = configuration.Password;
            StateSyncRate = configuration.StateSyncRate;
        }
    }
}
