using MPTanks.Engine.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.GameClient.Menus
{
    class ClientSettings : SettingsBase
    {
        private static ClientSettings _instance;
        public static ClientSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ClientSettings();
                    _instance.LoadFromFile(Path.Combine(ConfigDir, "Client Settings.json"));
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        
        #region Save Helper
        public new static void Save()
        {
            Instance.Save(Path.Combine(ConfigDir, "Client Settings.json"));
        }
        #endregion
        
        public override void OnSettingChanged(Setting setting)
        {
            Save();
        }

        public Setting<string[]> ModSearchPaths { get; private set; }

        public Setting<string> ClientLogLocation { get; private set; }

        public ClientSettings()
        {
            ModSearchPaths = new Setting<string[]>(this, "Mod search paths", "The paths in which to search for packed *.mod files.",
                new[] {
                    Path.Combine(Directory.GetCurrentDirectory(), "mods"),
                    Path.Combine(ConfigDir, "mods")
                });

            ClientLogLocation = new Setting<string>(this, "Client log directory",
                "The directory where the client (menus and watchdog) logs are stored.",
                Path.Combine(ConfigDir, "clientlogs", "client.log"));
        }

    }
}
