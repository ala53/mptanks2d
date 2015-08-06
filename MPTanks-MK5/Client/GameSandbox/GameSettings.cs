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
        public Setting<bool> ForceGen0GCEveryFrame { get; private set; }

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
            GameLogLocation = Setting.Create(this, "Log storage location",
                   "Where to store runtime logs for the game. This uses NLog storage conventions." +
                   " So, ${basedir} is the program's installation directory.",
                   Path.Combine(ConfigDir, "gamelogs", "game.log"));

            ForceFullGCEveryFrame = Setting.Create(this, "Force Full GC every frame",
            "Whether to force a full GC every frame. Useful for detecting memory leaks, terrible for performance.", false);

            ForceGen0GCEveryFrame = Setting.Create(this, "Force Gen 0 GC every frame",
            "Whether to force a fast GC every frame. This is rarely a significant performance problem so" +
            " it's useful for debugging purposes. Recommended to be off but it's ok to have it on.", false);

            ModUnpackPath = Setting.Create(this, "Mod temp directory",
            "The place to store mods that are used at runtime. In other words, this is the directory" +
            " that *.mod files are unpacked into.",
            Path.Combine(ConfigDir, "tempmodunpack"));

            ModAssetPath = Setting.Create(this, "Mod temp directory for assets",
                "The place to store mods assets that are used at runtime. In other words, this is the directory" +
                " that *.mod files are unpacked into (but not the code).",
                Path.Combine(ConfigDir, "tempmodunpack", "assets"));

            ModMapPath = Setting.Create(this, "Mod temp directory for maps",
                "The place where maps from *.mod files are unpacked to.", Path.Combine(ConfigDir, "tempmodunpack", "maps"));

            ModDownloadPath = Setting.Create(this, "Mod download directory",
                "The directory to store mods downloaded from servers in.",
                Path.Combine(ConfigDir, "mods"));


            CoreMods = Setting.Create(this, "Core Mods",
                "The core mods that will be autoinjected into every game without verification." +
                "They must be DLL files.", DefaultTrustedMods);

            AssetSearchPaths = Setting.Create(this, "Asset search paths", "The paths in which to look for assets assets.",
                new[] {
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

            PhysicsCompensationForRendering = Setting.Create(this, "Physics Skin Compensation",
                "The amount in blocks to compensate for Farseer Physics's skin on bodies.", 0.085f);

            Fullscreen = Setting.Create(this, "Fullscreen mode",
                "Whether to render the game in fullscreen mode", false);

            VSync = Setting.Create(this, "Enable vertical blank sync",
                "Whether v-blank-sync should be enabled.", true);

            Zoom = Setting.Create(this, "Game world zoom",
                "The zoom level of the game world, relative to default.",
                1f);

            UserTankImageDownloadCache = Setting.Create(this, "Custom Tank Image Download Path",
                "The path in which to download custom images that users make for their tanks",
                Path.Combine(ConfigDir, "tankimages"));

            InputDriverName = Setting.Create(this, "Input driver name",
                "The name of the driver to use for in game input.", GamePadInputDriver.Name);

            InputKeyBindings = Setting.Create<string>(this, "Input Key Bindings",
                "The stored key bindings for the current input driver", null);
        }
    }
}
