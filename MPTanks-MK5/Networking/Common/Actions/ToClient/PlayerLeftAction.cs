using Lidgren.Network;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class PlayerLeftAction : ActionBase
    {
        public ushort PlayerId { get; private set; }
        public byte[] GamemodeState { get; private set; }
        public PlayerLeftAction(NetIncomingMessage message) : base(message)
        {
            PlayerId = (ushort)message.ReadUInt32(GameCore.PlayerIdNumberOfBits);
            message.ReadBytes(message.PeekUInt16());
        }

        public PlayerLeftAction(NetworkPlayer player, GameCore game)
        {
            GamemodeState = game.Gamemode.FullState;
            PlayerId = player.Id;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(PlayerId, GameCore.PlayerIdNumberOfBits);
            message.Write((ushort)GamemodeState.Length);
            message.Write(GamemodeState);
        }
    }
}
