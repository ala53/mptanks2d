using Lidgren.Network;
using MPTanks.Engine;
using MPTanks.Networking.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class PlayerUpdateAction : ActionBase
    {
        public byte[] GamemodeState { get; private set; }
        public FullStatePlayer Player { get; private set; }
        public PlayerUpdateAction()
        {
        }

        public PlayerUpdateAction(NetworkPlayer player, GameCore game)
        {
            Player = new FullStatePlayer(player);
            GamemodeState = game.Gamemode.GetFullState();
            game.Gamemode.SetFullState(GamemodeState);
        }
        protected override void DeserializeInternal(NetIncomingMessage message)
        {
            Player = FullStatePlayer.Read(message);
            GamemodeState = message.ReadBytes(message.ReadUInt16());
        }
        public override void Serialize(NetOutgoingMessage message)
        {
            Player.Write(message);
            message.Write((ushort)GamemodeState.Length);
            message.Write(GamemodeState);
        }
    }
}
