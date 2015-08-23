using Lidgren.Network;
using MPTanks.Networking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Engine;
using Microsoft.Xna.Framework;
using MPTanks.Engine.Tanks;

namespace MPTanks.Networking.Server
{
    public class ServerPlayer : NetworkPlayer
    {
        public ServerPlayer(Server server) { Server = server; }
        public NetConnection Connection { get; internal set; }
        public TimeSpan LatencyTwoWay => TimeSpan.FromSeconds(Connection.AverageRoundtripTime);
        public TimeSpan LatencyOneWay => TimeSpan.FromSeconds(Connection.AverageRoundtripTime / 2);
        public Server Server { get; private set; }
        public NetworkPlayer Player
        {
            get
            {
                if (Server.GameInstance.Game.PlayersById.ContainsKey(Id))
                    return (NetworkPlayer)Server.GameInstance.Game.PlayersById[Id];
                return this;
            }
        }

    }
}
