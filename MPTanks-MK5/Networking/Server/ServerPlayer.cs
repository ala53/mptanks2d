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
using MPTanks.Networking.Common.Game;

namespace MPTanks.Networking.Server
{
    public class ServerPlayer
    {
        public ServerPlayer(Server server, NetworkPlayer playerObject)
        {
            Server = server;
            _player = playerObject;
            _player.Id = ushort.MaxValue;
        }
        public PseudoFullGameWorldState LastSentState { get; set; }
        public NetConnection Connection { get; internal set; }
        public TimeSpan LatencyTwoWay => TimeSpan.FromSeconds(Connection.AverageRoundtripTime);
        public TimeSpan LatencyOneWay => TimeSpan.FromSeconds(Connection.AverageRoundtripTime / 2);
        public Server Server { get; private set; }
        public bool IsReady => Player?.IsReady ?? false; 
        private NetworkPlayer _player;
        public NetworkPlayer Player
        {
            get
            {
                if (Server.Game.PlayersById.ContainsKey(_player.Id))
                {
                    var plr = Server.Game.PlayersById[_player.Id] as NetworkPlayer;
                    _player = plr;
                }
                return _player;
            }
        }

    }
}
