using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Settings
{
    public class GlobalSettings : SettingsBase
    {
        public static bool Debug { get { return Instance.DebugMode; } }
        public static GlobalSettings Instance { get; private set; } = new GlobalSettings("Global Settings.json");

        public Setting<bool> DebugMode { get; private set; }

        public Setting<string> LogLevel { get; private set; }

        private GlobalSettings(string file) : base(file)
        {
        }

        protected override void SetDefaults()
        {
#if DEBUG
            DebugMode = new Setting<bool>(this, "Debug mode", "Whether to run the game in a debugging mode. The equivalent of #if DEBUG.", true);
#else
            DebugMode = new Setting<bool>(this, "Debug mode", "Whether to run the game in a debugging mode. The equivalent of #if DEBUG.", false);
#endif
            LogLevel = new Setting<string>(this, "Log Level", "The NLog Log level to run the game at (Fatal, Error, Warn, Info, Debug, Trace).",
                DebugMode ? "Trace" : "Info");
        }
    }
}
