using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class TimescaleChangedAction : ActionBase
    {
        public float Timescale { get; set; }
        public TimescaleChangedAction(NetIncomingMessage message)
            :base(message)
        {
            Timescale = message.ReadFloat();
        }

        public TimescaleChangedAction(float newTimescale)
        {
            Timescale = newTimescale;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(Timescale);
        }
    }
}
