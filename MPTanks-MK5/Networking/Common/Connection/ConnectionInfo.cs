using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Connection
{
    public class ConnectionInfo
    {
        public string FriendlyServerName { get; set; } = "";
        public bool IsHost { get; set; }
        public string ServerAddress { get; set; } = "";
        public ushort ServerPort { get; set; }
        public string Password { get; set; } = "";
    }
}
