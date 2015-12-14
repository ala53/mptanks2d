using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Settings
{
    public class GlobalSettings : SettingsBase
    {
        public static bool Debug { get { return Instance.DebugMode; } }
        public static bool Trace { get { return Instance.TraceMode; } }
        public static GlobalSettings Instance { get; private set; } = new GlobalSettings("globalsettings.json");

        public Setting<bool> DebugMode { get; private set; }
        public Setting<bool> TraceMode { get; private set; }
        public Setting<string> LogLevel { get; private set; }
        public Setting<string> StoredAccountInfo { get; private set; }
        
        private GlobalSettings(string file) : base(file)
        {
        }

        protected override void SetDefaults()
        {
            TraceMode = Setting.Hidden<bool>(this, "Trace mode")
            .SetDescription("Whether to run with unchained exceptions.")
            .SetDefault(false);

            DebugMode = Setting.Bool(this, "Debug mode")
            .SetDescription("Whether to run the game in a debugging mode. The equivalent of #if DEBUG.")
#if DEBUG
            .SetDefault(true);
#else
            .SetDefault(false);
#endif
            LogLevel = Setting.String(this, "Log Level")
            .SetDescription("The NLog Log level to run the game at (Fatal, Error, Warn, Info, Debug, Trace).")
            .SetDefault(DebugMode ? "Trace" : "Info");

            StoredAccountInfo = Setting.Hidden<string>(this, "DRM Stored Data")
            .SetDescription("The stored data from the online authentication system.")
            .SetDefault(null);
        }

        private static string AssemblyProductVersion
        {
            get
            {
                object[] attributes = typeof(GameCore).Assembly
                    .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
                return attributes.Length == 0 ?
                    "" :
                    ((AssemblyInformationalVersionAttribute)attributes[0]).InformationalVersion;
            }
        }

    }
}
