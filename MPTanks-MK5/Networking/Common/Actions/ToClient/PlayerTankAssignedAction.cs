using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class PlayerTankAssignedAction : ActionBase
    {
        public Guid PlayerId { get; private set; }
        public ushort ObjectId { get; private set; }

        public PlayerTankAssignedAction(NetIncomingMessage message) : base(message)
        {
            PlayerId = new Guid(message.ReadBytes(16));
            ObjectId = message.ReadUInt16();
        }

        public PlayerTankAssignedAction(NetworkPlayer player, ushort id)
        {
            PlayerId = player.Id;
            ObjectId = id;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(PlayerId.ToByteArray());
            message.Write(ObjectId);
        }
    }
}
