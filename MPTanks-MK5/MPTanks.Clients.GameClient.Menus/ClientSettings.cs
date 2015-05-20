using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.GameClient.Menus
{
    class ClientSettings : GameSettings
    {
        private static ClientSettings _instance;
        public static ClientSettings Instance
        {
            get
            {
                if (_instance == null) LoadSettings();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        #region Load Helper
        private static void LoadSettings()
        {
            _instance = new ClientSettings();
        }

        #endregion
        #region Save Helper
        public static void Save()
        {
            Instance.SaveChanges();
        }
        public void SaveChanges()
        {

        }
        #endregion

        public override void OnSettingChanged(Engine.Settings.Setting setting)
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
