using MPTanks.Engine.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server.WebInterface
{
    class WebPlayerInfoResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string ClanName { get; set; }
        public bool Premium { get; set; }
    }
}
