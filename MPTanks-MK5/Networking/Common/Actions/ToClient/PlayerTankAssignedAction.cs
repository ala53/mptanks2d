using Lidgren.Network;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class PlayerTankAssignedAction : ActionBase
    {
        public ushort PlayerId { get; private set; }
        public ushort ObjectId { get; private set; }

        public PlayerTankAssignedAction(NetIncomingMessage message) : base(message)
        {
            PlayerId = (ushort)message.ReadUInt32(GameCore.PlayerIdNumberOfBits);
            ObjectId = message.ReadUInt16();
        }

        public PlayerTankAssignedAction(NetworkPlayer player, ushort id)
        {
            PlayerId = player.Id;
            ObjectId = id;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(PlayerId, GameCore.PlayerIdNumberOfBits);
            message.Write(ObjectId);
        }
    }
}
