using MPTanks.Engine.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server
{
    public class ServerSettings : SettingsBase
    {
        public static ServerSettings Instance { get; private set; } =
            new ServerSettings("serversettings.json");

        public Setting<float> TimeToWaitForPlayersReady { get; private set; }

        private ServerSettings(string file) : base(file) { }
        
        protected override void SetDefaults()
        {
            TimeToWaitForPlayersReady = Setting.Create(this, "Player ready wait time",
                "Amount of time to wait for players to be ready before " +
                "starting the game regardless of their status (give default tank).", 60000f);
        }
    }
}
