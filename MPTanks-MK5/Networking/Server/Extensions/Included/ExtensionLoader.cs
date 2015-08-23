using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MPTanks.Networking.Server.Extensions.Included
{
    public class ExtensionLoader : ServerExtensionBase
    {
        public ExtensionLoader(Server server) : base(server)
        {

        }

        public override void Initialize()
        {
            ChatHandler.RegisterCommand((Delegate)LoadExtensionCommand, "loadextension", Chat.ChatServer.ChatCommandParameter.String);
        }

        private void LoadExtensionCommand(string filename)
        {
            Server.ExtensionManager.LoadExtension(filename);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
