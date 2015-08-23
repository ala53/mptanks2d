using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server.Extensions
{
    public class ExtensionManager
    {
        private Server _server;
        private List<ServerExtensionBase> _extensions = new List<ServerExtensionBase>();
        public ExtensionManager(Server server)
        {
            _server = server;
            var baseExtensions = new ServerExtensionBase[]
            {
                new Included.BanHammer(server),
                new Included.ExtensionLoader(server)
            };

            foreach (var ext in baseExtensions)
            {
                _extensions.Add(ext);
                ext.Initialize();
            }
        }
        public void LoadExtensions(string searchDir)
        {

        }

        public void LoadExtension(string filename)
        {
        }

        public void LoadExtension(Type t)
        {
            if (!t.IsSubclassOf(typeof(ServerExtensionBase))) throw new Exception("Not a valid extension!");

            ServerExtensionBase ext = null;
            ext.Initialize();
            _extensions.Add(ext);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var e in _extensions) e.Update(gameTime);

        }
    }
}
