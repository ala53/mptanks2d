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
        public static GlobalSettings Instance { get; private set; } = new GlobalSettings("Global Settings.json");

        public Setting<bool> DebugMode { get; private set; }

        public Setting<string> LogLevel { get; private set; }

        public Setting<string> CurrentGameVersion { get; private set; }

        private GlobalSettings(string file) : base(file)
        {
        }

        protected override void SetDefaults()
        {
#if DEBUG
            DebugMode = Setting.Create(this, "Debug mode", "Whether to run the game in a debugging mode. The equivalent of #if DEBUG.", true);
#else
            DebugMode = Setting.Create(this, "Debug mode", "Whether to run the game in a debugging mode. The equivalent of #if DEBUG.", false);
#endif
            LogLevel = Setting.Create(this, "Log Level", "The NLog Log level to run the game at (Fatal, Error, Warn, Info, Debug, Trace).",
                DebugMode ? "Trace" : "Info");

            CurrentGameVersion = Setting.Create(this, "Game version string", "The version number of MP Tanks. DO NOT CHANGE THIS.", 
                "MPTanks " + AssemblyProductVersion);
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
