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
            ModSearchPaths = Setting.Hidden<string[]>(this, "Mod search paths")
            .SetDescription("The paths in which to search for packed *.mod files.")
            .SetDefault(new[] {
                    Path.Combine(Directory.GetCurrentDirectory(), "mods"),
                    Path.Combine(ConfigDir, "mods")
                });

            ClientLogLocation = Setting.Path(this, "Client log directory")
            .SetDescription("The directory where the client (menus and watchdog) logs are stored.")
            .SetDefault(Path.Combine(ConfigDir, "clientlogs", "client.log"));

            SandboxGames = Setting.Bool(this, "Sandbox servers and game instances")
            .SetDescription("WARNING! SIGNIFICANT USABILITY IMPLICATIONS! Disabling this will crash the game when you leave a server. ")
            .SetDefault(true);

            StoredServerAddress = Setting.Hidden<string>(this, "\"Join server\" stored user address").SetDefault("");

            WindowRectangle = Setting.Hidden<Rectangle>(this, "Window Rectangle")
            .SetDefault(new Rectangle(100, 100, 800, 480));
        }

    }
}
