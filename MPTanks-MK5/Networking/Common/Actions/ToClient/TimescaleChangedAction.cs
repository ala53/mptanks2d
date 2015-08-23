using Lidgren.Network;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class TimescaleChangedAction : ActionBase
    {
        public GameCore.TimescaleValue Timescale { get; set; }
        public TimescaleChangedAction(NetIncomingMessage message)
            :base(message)
        {
            Timescale = new GameCore.TimescaleValue(message.ReadDouble(), message.ReadString());
        }
        public TimescaleChangedAction(GameCore game)
        {
            Timescale = game.Timescale;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(Timescale.Fractional);
            message.Write(Timescale.DisplayString);
        }
    }
}
