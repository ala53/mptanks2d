using Lidgren.Network;
using MPTanks.Networking.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class PlayerJoinedAction : ActionBase
    {
        public FullStatePlayer Player { get; private set; }
        public PlayerJoinedAction(NetIncomingMessage message) : base(message)
        {
            Player = FullStatePlayer.Read(message);
        }

        public PlayerJoinedAction(NetworkPlayer player)
        {
            Player = new FullStatePlayer(player);
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            Player.Write(message);
        }
    }
}
