using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server.Extensions.Included
{
    class BanHammer : ServerExtensionBase
    {
        public BanHammer(Server server) : base(server)
        {

        }

        public override void Initialize()
        {
            ChatHandler.RegisterCommand((Action<ServerPlayer>)BanCommand, "ban", Chat.ChatServer.ChatCommandParameter.Player);
            ChatHandler.RegisterCommand((Action<ServerPlayer[]>)BanCommand, "ban", Chat.ChatServer.ChatCommandParameter.ArrayOfPlayer);
            ChatHandler.RegisterCommand((Action<ServerPlayer>)KickCommand, "kick", Chat.ChatServer.ChatCommandParameter.Player);
            ChatHandler.RegisterCommand((Action<ServerPlayer[]>)KickCommand, "kick", Chat.ChatServer.ChatCommandParameter.ArrayOfPlayer);
        }

        private void BanCommand(ServerPlayer player)
        {
        }
        private void BanCommand(ServerPlayer[] players)
        {
        }
        private void KickCommand(ServerPlayer player)
        {
        }
        private void KickCommand(ServerPlayer[] players)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
