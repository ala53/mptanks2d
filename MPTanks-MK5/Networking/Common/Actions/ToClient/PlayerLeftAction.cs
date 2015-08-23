using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class PlayerLeftAction : ActionBase
    {
        public Guid PlayerId { get; private set; }
        public PlayerLeftAction(NetIncomingMessage message):base(message)
        {
            PlayerId = new Guid(message.ReadBytes(16));
        }

        public PlayerLeftAction(NetworkPlayer player)
        {
            PlayerId = player.Id;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(PlayerId.ToByteArray());
        }
    }
}
