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

        public Setting<TimeSpan> TimeToWaitForPlayersReady { get; private set; }

        private ServerSettings(string file) : base(file)
        {
        }

        protected override void SetDefaults()
        {
            TimeToWaitForPlayersReady = Setting.Time(this, "Player ready wait time")
            .SetDescription("Amount of time to wait for players to be ready before " +
                "starting the game regardless of their status (give default tank).")
            .SetDefault(TimeSpan.FromMilliseconds(15000));
        }
    }
}
