using MPTanks.Engine.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common
{
    public class Settings : SettingsBase
    {
        public static Settings Instance { get; private set; } = new Settings("Common Networking.json");

        public Setting<string> MasterServer { get; private set; }

        public Setting<string> LoginServer { get; private set; }

        public Setting<string> ModServer { get; private set; }

        public Setting<string> AccountServer { get; private set; }

        public Setting<string> PayServer { get; private set; }

        public Setting<int> MaxActionFrameCount { get; private set; }

        public Setting<float> MaxNetworkDelayMs { get; private set; }

        private Settings(string file) : base(file)
        {
        }

        protected override void SetDefaults()
        {
            MasterServer = new Setting<string>(this, "Master Server Address",
                "The master server to list hosted servers on.",
                "master.mptanks.zsbgames.me");

            LoginServer = new Setting<string>(this, "Login Server Address",
                "The url for the login server (the one used for client logins).",
                "login.mptanks.zsbgames.me");

            ModServer = new Setting<string>(this, "Mod Server Address",
                "The server that stores the mod database and search utilities.",
                "mods.mptanks.zsbgames.me");

            AccountServer = new Setting<string>(this, "Account Server Address",
                "The address of the account server, which stores customization information for tanks (colors and textures)," +
                " account information (both public and private), and lets one authenticate an auth token.",
                "accounts.mptanks.zsbgames.me");

            PayServer = new Setting<string>(this, "Payments Server Address",
                "The address of the server that ZSB uses for payment processing (buying stuff).",
                "payments.mptanks.zsbgames.me");

            MaxActionFrameCount = new Setting<int>(this, "Maximum Action Frames Count",
                "The maximum number of action frames to store at any point", 600);

            MaxNetworkDelayMs = new Setting<float>(this, "Maximum network delay for batching",
                "The maximum number of milliseconds that the action queue can be delayed (in milliseconds) " +
                "to batch messages together. This improves bandwidth usage a fair bit but also increases latency.", 30);
        }
    }
}
