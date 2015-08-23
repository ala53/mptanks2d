using Lidgren.Network;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    /// <summary>
    /// Marks a change to the <see cref="GameCore.Status"/> property.
    /// </summary>
    public class GameStatusChangedAction : ActionBase
    {
        public GameCore.CurrentGameStatus Status { get; private set; }
        public GameStatusChangedAction(NetIncomingMessage message) : base(message)
        {
            Status = (GameCore.CurrentGameStatus)message.ReadByte();
        }

        public GameStatusChangedAction(GameCore game)
        {
            Status = game.Status;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write((byte)Status);
        }
    }
}
