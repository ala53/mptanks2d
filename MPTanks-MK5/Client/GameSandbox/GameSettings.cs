using MPTanks.Client.GameSandbox.Input;
using MPTanks.Engine.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.GameSandbox
{
    [Serializable]
    public class GameSettings : SettingsBase
    {
        public static GameSettings Instance { get; private set; }

        static GameSettings()
        {
            Instance = new GameSettings("gamesettings.json");
        }

        public Setting<string> GameLogLocation { get; private set; }

        /// <summary>
        /// Whether to force a gen 0 GC every frame. This gets rid of
        /// most temporary objects, helping to trace actual memory leaks.
        /// Unfortunately, it does so to the possible detriment of 
        /// framerate.
        /// </summary>
        public Setting<bool> ForceFullGCEveryFrame { get; private set; }

        //Where to look for assets
        public Setting<string[]> AssetSearchPaths { get; private set; }

        //Stores mods in a runtime directory. That way, when we download mods from servers, we 
        //just leave them in the temp directory where they are removed next time the program opens
        public Setting<string> ModUnpackPath { get; private set; }
        public Setting<string> ModAssetPath { get; private set; }
        public Setting<string> ModMapPath { get; private set; }
        public Setting<string> ModDownloadPath { get; private set; }

        public Setting<string> UserTankImageDownloadCache { get; private set; }

        /// <summary>
        /// A list of mod files (relative or absolute) that should be loaded without 
        /// Code security verification. AKA trusted mods.
        /// </summary>
        public Setting<string[]> CoreMods { get; private set; }

        public static readonly string[] DefaultTrustedMods = new[] {
                    "core-assets.mod"
                };

        /// <summary>
        /// The amount of "blocks" to compensate for in rendering because of the 
        /// skin on physics objects
        /// </summary>
        public Setting<float> PhysicsCompensationForRendering { get; private set; }

        #region Screen Resolution
        public Setting<bool> Fullscreen { get; private set; }
        public Setting<bool> VSync { get; private set; }
        public Setting<float> Zoom { get; private set; }
        public Setting<float> SSAARate { get; private set; }
        #endregion

        #region Input settings
        public Setting<string> InputDriverName { get; private set; }
        public Setting<string> InputKeyBindings { get; private set; }
        #endregion
        private GameSettings(string file)
            : base(file)
        { }
        protected override void SetDefaults()
        {
            GameLogLocation = Setting.Path(this, "Log storage location")
            .SetDescription("Where to store runtime logs for the game.")
            .SetDefault(Path.Combine(ConfigDir, "gamelogs", "game.log"));

            ForceFullGCEveryFrame = Setting.Bool(this, "Force Full GC every frame")
            .SetDescription("Whether to force a full GC every frame. Useful for detecting memory leaks, terrible for performance." +
            " (If you don't know what this is: DON'T USE IT)")
            .SetDefault(false);

            ModUnpackPath = Setting.Hidden<string>(this, "Mod temp directory")
            .SetDescription("The place to store mods that are used at runtime. In other words, this is the directory" +
            " that *.mod files are unpacked into.")
            .SetDefault(Path.Combine(ConfigDir, "tempmodunpack"));

            ModAssetPath = Setting.Hidden<string>(this, "Mod temp directory for assets")
            .SetDescription("The place to store mods assets that are used at runtime. In other words, this is the directory" +
            " that *.mod files are unpacked into (but not the code).")
            .SetDefault(Path.Combine(ConfigDir, "tempmodunpack", "assets"));

            ModMapPath = Setting.Hidden<string>(this, "Mod temp directory for maps")
            .SetDescription("The place where maps from *.mod files are unpacked to.")
            .SetDefault(Path.Combine(ConfigDir, "tempmodunpack", "maps"));

            ModDownloadPath = Setting.Path(this, "Mod download directory")
            .SetDescription("The directory to store mods downloaded from servers in.")
            .SetDefault(Path.Combine(ConfigDir, "mods"));


            CoreMods = Setting.Hidden<string[]>(this, "Core Mods")
            .SetDescription("The core mods that will be autoinjected into every game without verification.")
            .SetDefault(DefaultTrustedMods);

            AssetSearchPaths = Setting.Hidden<string[]>(this, "Asset search paths")
            .SetDescription("The paths in which to look for assets assets.")
            .SetDefault(new[] {
                    Directory.GetCurrentDirectory(), //current directory
                    Path.Combine(Directory.GetCurrentDirectory(), "assets"),
                    Path.Combine(Directory.GetCurrentDirectory(), "assets", "animations"),
                    Path.Combine(Directory.GetCurrentDirectory(), "assets", "mapobjects"),
                    Path.Combine(Directory.GetCurrentDirectory(), "assets", "other"),
                    Path.Combine(Directory.GetCurrentDirectory(), "assets", "tanks"),
                    Path.Combine(ModUnpackPath, "assets"),
                    Path.Combine(ConfigDir, "assets"),
                    ConfigDir
                });

            PhysicsCompensationForRendering = Setting.Hidden<float>(this, "Physics Skin Compensation")
            .SetDescription("The amount in blocks to compensate for Farseer Physics's skin on bodies.")
            .SetDefault(0.085f);

            Fullscreen = Setting.Bool(this, "Fullscreen mode")
            .SetDescription("Whether to render the game in fullscreen mode")
            .SetDefault(false);

            VSync = Setting.Bool(this, "Enable vertical blank sync")
            .SetDescription("Whether v-blank-sync should be enabled.")
            .SetDefault(true);

            Zoom = Setting.Hidden<float>(this, "Game world zoom")
            .SetDescription("The zoom level of the game world, relative to default.")
            .SetDefault(1f);

            SSAARate = Setting.Number(this, "SSAA Rate")
            .SetDescription("The amount of supersampling to do for the game images; " +
            "higher values are (slightly) better, lower values are (much) faster.")
            .SetDefault(1.25f)
            .SetAllowedValues(1, 1.25f, 1.5f, 1.75f, 2f, 2.25f, 2.5f, 2.75f, 3f, 3.5f, 4f, 4.5f, 5f, 5.5f, 6f);

            UserTankImageDownloadCache = Setting.Path(this, "Custom Tank Image Download Path")
            .SetDescription("The path in which to download custom images that users make for their tanks")
            .SetDefault(Path.Combine(
                ConfigDir, "tankimages"));

            InputDriverName = Setting.String(this, "Input driver name")
            .SetDescription("The name of the driver to use for in game input.")
            .SetDefault(KeyboardMouseInputDriver.Name)
            .SetAllowedValues(() => InputDriverBase.Drivers.Keys);

            InputKeyBindings = Setting.Hidden<string>(this, "Input Key Bindings")
            .SetDescription("The stored key bindings for the current input driver");
        }
    }
}
