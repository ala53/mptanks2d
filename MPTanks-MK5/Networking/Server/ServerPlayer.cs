using Lidgren.Network;
using MPTanks.Networking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server
{
    public class ServerPlayer : NetworkPlayer
    {
        public NetConnection Connection { get; internal set; }
        public TimeSpan LatencyTwoWay => TimeSpan.FromSeconds(Connection.AverageRoundtripTime);
        public TimeSpan LatencyOneWay => TimeSpan.FromSeconds(Connection.AverageRoundtripTime / 2);
    }
}
