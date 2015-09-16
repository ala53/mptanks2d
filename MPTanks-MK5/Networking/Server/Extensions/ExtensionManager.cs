using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
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
        public string[] LoadExtensions(string searchDir, bool recurse = false)
        {
            var files = new List<string>(Directory.GetFiles(searchDir, "*",
                recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                .Where(a => new FileInfo(a).Extension.ToLower() == ".dll"));
            foreach (var file in files)
                LoadExtension(file);

            return files.ToArray();
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
