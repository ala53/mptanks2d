using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace MPTanks.Networking.Common.Actions.ToServer
{
    public class PlayerReadyChangedAction : ActionBase
    {
        public bool IsReady { get; private set; }
        public PlayerReadyChangedAction()
        {
        }

        public PlayerReadyChangedAction(bool ready)
        {
            IsReady = ready;
        }
        protected override void DeserializeInternal(NetIncomingMessage message)
        {
            IsReady = message.ReadBoolean();
        }
        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(IsReady);
        }
    }
}
