using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class CountdownStartedAction : ActionBase
    {
        public TimeSpan CountdownTime { get; set; }
        protected override void DeserializeInternal(NetIncomingMessage msg)
        {
            CountdownTime = TimeSpan.FromMilliseconds(msg.ReadFloat());
        }
        public CountdownStartedAction() { }
        public CountdownStartedAction(TimeSpan countdown)
        {
            CountdownTime = countdown;
        }

       

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write((float)CountdownTime.TotalMilliseconds);
        }
    }
}
