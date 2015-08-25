using Lidgren.Network;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
   public class GameCanRunChangedAction : ActionBase
    {
        public bool CanRun { get; private set; }
        public GameCanRunChangedAction(NetIncomingMessage message) : base(message)
        {
            CanRun = message.ReadBoolean();
        }

        public GameCanRunChangedAction(GameCore game)
        {
            CanRun = game.CanRun;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(CanRun);
        }
    }
}
