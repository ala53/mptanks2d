using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4
{
    namespace Settings
    {
        public static class Static
        {
            public const string SettingsPath = "settings.json";
            /// <summary>
            /// Whitelists the modules you can use as a server
            /// for non-premium players (custom builds for f2p)
            /// </summary>
            public const bool WhitelistServerModules = false;
            public static readonly string[] ServerModulesWhitelist =
            {   
                "basictank_zsb", 
                "mortartank_zsb", 
                "defaultmap_zsb" 
            };
        }

        public static class Modifiable
        {

            private static bool loaded = false;

            public static bool IsServerOnly = false;
            internal static Window GameInstance;

            public static float MaxPhysicsFramerate = 30;

            public static ScriptEngineType Engine = ScriptEngineType.Jint;
            public enum ScriptEngineType
            {
                Jint,
                ClearScriptV8,
                VroomJs
            }

            public static class CoreAssets
            {
                public static string UI = "assets/ui";
                public static string JS = "assets/js";
                public static string ConsoleJS = "assets/js/console";
                public static string Libraries = "dependencies/lib";
            }

            public static class ModulePaths
            {
                public static string Tanks = "assets/modules/tanks";
                public static string Projectiles = "assets/modules/projectiles";
                public static string Maps = "assets/modules/maps";
                public static string MapObjects = "assets/modules/objects";
                public static string[] OtherPaths = { };

                public static string[] All
                {
                    get
                    {
                        return new[] { Tanks, Projectiles, Maps, MapObjects }
                            .Union(OtherPaths).ToArray();
                    }
                }
            }

            static Modifiable()
            {
                Load();
            }

            public static void Load()
            {
                if (loaded)
                    return;

                if (!System.IO.File.Exists(Static.SettingsPath))
                    return;

                dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(
                    System.IO.File.ReadAllText(Static.SettingsPath));

                IsServerOnly = obj.serverOnly;
                MaxPhysicsFramerate = obj.maxPhysicsFramerate;
                Engine = obj.scriptEngine;

                ModulePaths.Tanks = obj.modulePaths.tanks;
                ModulePaths.Projectiles = obj.modulePaths.projectiles;
                ModulePaths.Maps = obj.modulePaths.maps;
                ModulePaths.MapObjects = obj.modulePaths.mapObjects;
                ModulePaths.OtherPaths = obj.modulePaths.other;

                CoreAssets.UI = obj.coreAssets.ui;
                CoreAssets.JS = obj.coreAssets.js;
                CoreAssets.ConsoleJS = obj.coreAssets.console;
                CoreAssets.Libraries = obj.coreAssets.lib;

                loaded = true;
            }

            public static void Save()
            {
                var saved = Newtonsoft.Json.JsonConvert.SerializeObject(
                    new
                    {
                        serverOnly = IsServerOnly,
                        maxPhysicsFramerate = MaxPhysicsFramerate,
                        scriptEngine = Engine,
                        modulePaths = new
                        {
                            tanks = ModulePaths.Tanks,
                            projectiles = ModulePaths.Projectiles,
                            maps = ModulePaths.Maps,
                            mapObjects = ModulePaths.MapObjects,
                            other = ModulePaths.OtherPaths
                        },
                        coreAssets = new
                        {
                            ui = CoreAssets.UI,
                            js = CoreAssets.JS,
                            console = CoreAssets.ConsoleJS,
                            lib = CoreAssets.Libraries
                        }
                    });


                System.IO.File.WriteAllText("settings.json", saved);
            }
        }
    }
}
