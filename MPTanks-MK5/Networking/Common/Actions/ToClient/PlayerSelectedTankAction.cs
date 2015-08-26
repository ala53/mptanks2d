using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class PlayerSelectedTankAction : ActionBase
    {
        public Guid PlayerId { get; private set; }
        public string TankType { get; private set; }
        public PlayerSelectedTankAction(NetIncomingMessage message) : base(message)
        {
            PlayerId = new Guid(message.ReadBytes(16));
            TankType = message.ReadString();
        }

        public PlayerSelectedTankAction(NetworkPlayer player, string tankType)
        {
            PlayerId = player.Id;
            TankType = tankType;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(PlayerId.ToByteArray());
            message.Write(TankType);
        }
    }
}
