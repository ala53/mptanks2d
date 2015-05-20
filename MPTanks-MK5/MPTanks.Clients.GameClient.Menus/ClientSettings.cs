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

        //Where config information is stored
        public Setting<string> ConfigDir { get; private set; }

        public Setting<string[]> ModSearchPaths { get; private set; }

        public ClientSettings()
        {
            ModSearchPaths = new Setting<string[]>(this, "Mod search paths", "The paths in which to search for packed *.mod files.",
                new[] {
                    Path.Combine(Directory.GetCurrentDirectory(), "mods"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Saved Games", "MP Tanks 2D", "mods")
                });

            ConfigDir = new Setting<string>(this, "Save directory",
                "The directory where configuration information and mods are stored in.",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Saved Games", "MP Tanks 2D"));

        }

    }
}
