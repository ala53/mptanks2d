using Microsoft.Xna.Framework;
using MPTanks.Engine.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client
{
    class ClientSettings : SettingsBase
    {
        public static ClientSettings Instance { get; private set; } = new ClientSettings("clientsettings.json");

        public Setting<string[]> ModSearchPaths { get; private set; }

        public Setting<string> ClientLogLocation { get; private set; }

        public Setting<bool> SandboxGames { get; private set; }
        public Setting<string> StoredServerAddress { get; private set; }

        public Setting<Rectangle> WindowRectangle { get; private set; }

        public ClientSettings(string file) : base(file) { }

        protected override void SetDefaults()
        {
            ModSearchPaths = Setting.Create(this, "Mod search paths", "The paths in which to search for packed *.mod files.",
                new[] {
                    Path.Combine(Directory.GetCurrentDirectory(), "mods"),
                    Path.Combine(ConfigDir, "mods")
                });

            ClientLogLocation = Setting.Create(this, "Client log directory",
                "The directory where the client (menus and watchdog) logs are stored.",
                Path.Combine(ConfigDir, "clientlogs", "client.log"));

            SandboxGames = Setting.Create(this, "Sandbox servers and game instances",
                "WARNING! SIGNIFICANT USABILITY IMPLICATIONS! Disabling this will crash the game when you leave a server. " +
                "This says whether to run game instances in an isolated appdomain.", true);

            StoredServerAddress = Setting.Create(this, "\"Join server\" stored user address", "", "localhost:33132");

            WindowRectangle = Setting.Create(this, "Window Rectangle", "The location and size of the MPTanks window on screen",
                new Rectangle(0, 0, 800, 600));
        }

    }
}
