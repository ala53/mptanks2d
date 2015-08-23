using Microsoft.Xna.Framework;
using MPTanks.Networking.Server.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server.Extensions
{
    public abstract class ServerExtensionBase
    {
        public Server Server { get; private set; }
        public ChatServer ChatHandler => Server.ChatHandler;
        public ServerExtensionBase(Server server)
        {
            Server = server;
        }

        public abstract void Initialize();

        public abstract void Update(GameTime gameTime);
    }
}
