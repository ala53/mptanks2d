using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using MPTanks.Engine.Gamemodes;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    /// <summary>
    /// Used for synchronization: lets the client know that the game has ended and that
    /// it should go into the post game physics phase.
    /// </summary>
    public class GameEndedAction : ActionBase
    {
        public short WinningTeamId { get; private set; }
        public GameEndedAction()
        {
        }

        public GameEndedAction(Team winning) { WinningTeamId = winning.TeamId; }
        protected override void DeserializeInternal(NetIncomingMessage message)
        {
            WinningTeamId = message.ReadInt16();
        }
        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(WinningTeamId);
        }
    }
}
