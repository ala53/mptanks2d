using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server
{
    public class LoginManager
    {
        public Server Server { get; private set; }
        public LoginManager(Server server)
        {
            Server = server;
        }
        public void HandleConnection(NetIncomingMessage incoming)
        {
            if (Server.Players.Count < Server.Configuration.MaxPlayers)
                ApproveConnection(incoming);
            else DenyConnection(incoming, "Too many players");
        }
        private void ApproveConnection(NetIncomingMessage msg)
        {
            msg.SenderConnection.Approve();
            //To be removed: we should defer until we check the login info
            Server.Connections.Accept(msg.SenderConnection, new WebInterface.WebPlayerInfoResponse()
            {
                ClanName = "T3ST",
                Id = Guid.NewGuid(),
                Premium = true,
                Username = "T3ST_SUBJ3CT"
            });
        }

        private void DenyConnection(NetIncomingMessage msg, string reason = "Server terminated connection")
        {
            msg.SenderConnection.Deny(reason);
        }

        public void ProcessMessage(NetIncomingMessage message)
        {

        }
    }
}
