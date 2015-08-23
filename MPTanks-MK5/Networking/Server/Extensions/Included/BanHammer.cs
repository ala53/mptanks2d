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
            ChatHandler.RegisterCommand((Action<ServerPlayer, string>)BanCommand, "ban", "Ban a player from the server");
            ChatHandler.RegisterCommand((Action<ServerPlayer[], string>)BanCommand, "ban", "Ban multiple players from the server");
            ChatHandler.RegisterCommand((Action<ServerPlayer, string>)KickCommand, "kick", "Kick a player from the server");
            ChatHandler.RegisterCommand((Action<ServerPlayer[], string>)KickCommand, "kick", "Kick multiple players from the server");
        }

        private void BanCommand(ServerPlayer player, string reason = null)
        {
        }
        private void BanCommand(ServerPlayer[] players, string reason = null)
        {
        }
        private void KickCommand(ServerPlayer player, string reason = null)
        {
        }
        private void KickCommand(ServerPlayer[] players, string reason = null)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
