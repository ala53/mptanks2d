using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.IO;

namespace MPTanks.Networking.Server.Extensions.Included
{
    public class ExtensionLoader : ServerExtensionBase
    {
        public ExtensionLoader(Server server) : base(server)
        {

        }

        public override void Initialize()
        {
            ChatHandler.RegisterCommand((Action<string>)LoadExtensionCommand,
                "loadextension", "Loads an extension from the disk.");
            ChatHandler.RegisterCommand((Action<string, bool>)LoadExtensionsCommand,
                "loadextensions", "Loads extensions in a directory from the disk.");
            ChatHandler.RegisterCommand((Action<string[]>)LoadExtensionsCommand,
                "loadextensions", "Loads an array of extension files.");
            ChatHandler.RegisterCommand((Action<string>)LoadExtensionsCommand,
                "loadextensions", "Loads extensions in a directory from the disk.");
        }

        private void LoadExtensionCommand(string filename)
        {
            Server.ExtensionManager.LoadExtension(filename);
            ChatHandler.SendMessage("[Server] Loaded extension: " + new FileInfo(filename).Name,
                Server.Administrators.ToArray());
        }
        private void LoadExtensionsCommand(string dirName) =>
            LoadExtensionsCommand(dirName, false);
        private void LoadExtensionsCommand(string dirName, bool recurse)
        {
            foreach (var ext in Server.ExtensionManager.LoadExtensions(dirName, recurse))
                ChatHandler.SendMessage("[Server] Loaded extension: " + new FileInfo(ext).Name,
                    Server.Administrators.ToArray());
        }
        private void LoadExtensionsCommand(string[] files)
        {
            foreach (var ext in files)
            {
                Server.ExtensionManager.LoadExtension(ext);
                ChatHandler.SendMessage("[Server] Loaded extension: " + new FileInfo(ext).Name,
                    Server.Administrators.ToArray());
            }
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
