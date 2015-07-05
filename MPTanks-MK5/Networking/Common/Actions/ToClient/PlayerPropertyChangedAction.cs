using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class PlayerPropertyChangedAction : ActionBase
    {
        public object NewValue { get; set; }
        public PlayerPropertyChangedAction(NetIncomingMessage message)
            : base(message)
        {

        }

        public PlayerPropertyChangedAction(NetworkPlayer.NetworkPlayerPropertyChanged property, object value)
        {

        }

        public override void Serialize(NetOutgoingMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
