using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class PlayerReadyToStartChangedAction : ActionBase
    {
        public Guid PlayerId { get; private set; }
        public bool IsReadyToStart { get; private set; }
        public PlayerReadyToStartChangedAction(NetIncomingMessage message) : base(message)
        {
            PlayerId = new Guid(message.ReadBytes(16));
            IsReadyToStart = message.ReadBoolean();
        }

        public PlayerReadyToStartChangedAction(NetworkPlayer player, bool ready)
        {
            PlayerId = player.Id;
            IsReadyToStart = ready;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(PlayerId.ToByteArray());
            message.Write(IsReadyToStart);
        }
    }
}
