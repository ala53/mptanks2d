using Lidgren.Network;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class PlayerReadyToStartChangedAction : ActionBase
    {
        public ushort PlayerId { get; private set; }
        public bool IsReadyToStart { get; private set; }
        public PlayerReadyToStartChangedAction(NetIncomingMessage message) : base(message)
        {
            PlayerId = (ushort)message.ReadUInt32(GameCore.PlayerIdNumberOfBits);
            IsReadyToStart = message.ReadBoolean();
        }

        public PlayerReadyToStartChangedAction(NetworkPlayer player, bool ready)
        {
            PlayerId = player.Id;
            IsReadyToStart = ready;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(PlayerId, GameCore.PlayerIdNumberOfBits);
            message.Write(IsReadyToStart);
        }
    }
}
