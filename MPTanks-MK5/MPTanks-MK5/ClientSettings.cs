using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.GameClient
{
    public class ClientSettings : MPTanks.Engine.Settings
    {
        private static ClientSettings _instance;
        public static ClientSettings Instance
        {
            get
            {
                if (_instance == null) LoadSettings();
                return _instance;
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

        public Setting<string> LogLocation { get; private set; }

        /// <summary>
        /// The maximum number of on screen particles to allow
        /// </summary>
        public Setting<int> MaxParticlesToRender { get; private set; }

        /// <summary>
        /// Whether to force a gen 0 GC every frame. This gets rid of
        /// most temporary objects, helping to trace actual memory leaks.
        /// Unfortunately, it does so to the possible detriment of 
        /// framerate.
        /// </summary>
        public Setting<bool> ForceFullGCEveryFrame { get; private set; }
        public Setting<bool> ForceGen0GCEveryFrame { get; private set; }

        /// <summary>
        /// The number of instances of a sound that can be playing at the same time.
        /// </summary>
        public Setting<int> MaxInstancesOfOneSoundAllowed { get; private set; }

        public Setting<string[]> AssetAllowedFileExtensions { get; private set; }

        //Where config information is stored
        public Setting<string> ConfigDir { get; private set; }

        //Where to look for assets
        public Setting<string[]> AssetSearchPaths { get; private set; } //Looks for *.mod files to unpack
        public Setting<string[]> ModSearchPaths { get; private set; }

        //Stores mods in a runtime directory. That way, when we download mods from servers, we 
        //just leave them in the temp directory where they are removed next time the program opens
        public Setting<string> ModUnpackPath { get; private set; }

        /// <summary>
        /// The scale the rendering runs at relative to the blocks.
        /// This way, we can pass integers around safely.
        /// </summary>
        public Setting<float> RenderScale { get; private set; }

        /// <summary>
        /// The amount of "blocks" to compensate for in rendering because of the 
        /// skin on physics objects
        /// </summary>
        public Setting<float> PhysicsCompensationForRendering { get; private set; }

        public ClientSettings()
        {
            LogLocation = new Setting<string>(this, "Log storage location",
               "Where to store runtime logs for the game. This uses NLog storage conventions." +
               " So, ${basedir} is the program's installation directory.",
               "${basedir}/clientlogs/client.log");

            MaxParticlesToRender = new Setting<int>(this, "Max particles allowed on screen",
            "The maximum number of particles that can be displayed on screen at any particular time. Higher values" +
            " can increase visual fidelity (some particles may not be rendered at lower settings) while lower ones" +
            " substantially increase performance. See the related Particle Limit settings.",
            5000);

            ForceFullGCEveryFrame = new Setting<bool>(this, "Force Full GC every frame",
            "Whether to force a full GC every frame. Useful for detecting memory leaks, terrible for performance.", false);

            ForceGen0GCEveryFrame = new Setting<bool>(this, "Force Gen 0 GC every frame",
            "Whether to force a fast GC every frame. This is rarely a significant performance problem so" +
            " it's useful for debugging purposes. Recommended to be off but it's ok to have it on.", false);

            MaxInstancesOfOneSoundAllowed = new Setting<int>(this, "Max instances of 1 sound",
            "The maximum number of instaces of a single sound that can be playing simultaneously." +
            " If more sounds than that try to play simultaneously, the oldest one will be cut off. Increase" +
            " this if you are hearing audible cutoffs, at the cost of memory usage and performace.", 4);

            AssetAllowedFileExtensions = new Setting<string[]>(this, "Image asset file extensions",
                "The extensions to search for when trying to load an image, in the correct search order.",
                new[] { ".dds", ".png", ".jpg", ".jpeg", ".bmp", ".gif" });

            ConfigDir = new Setting<string>(this, "Save directory",
                "The directory where configuration information and mods are stored in.",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Saved Games", "MP Tanks 2D"));

            AssetSearchPaths = new Setting<string[]>(this, "Asset search paths", "The paths in which to look for assets.",
                new[] {
                    Directory.GetCurrentDirectory(), //current directory
                    Path.Combine(Directory.GetCurrentDirectory(), "assets"),
                    Path.Combine(Directory.GetCurrentDirectory(), "assets", "animations"),
                    Path.Combine(Directory.GetCurrentDirectory(), "assets", "mapobjects"),
                    Path.Combine(Directory.GetCurrentDirectory(), "assets", "other"),
                    Path.Combine(Directory.GetCurrentDirectory(), "assets", "tanks"),
                    Path.Combine(Directory.GetCurrentDirectory(), "mods"),
                    Path.Combine(Directory.GetCurrentDirectory(), "mods"),
                    Path.Combine(Directory.GetCurrentDirectory(), "mods", "modassets"),
                    Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "mptanks", "runtimemods", "modassets"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Saved Games", "MP Tanks 2D", "assets")
                });

            ModSearchPaths = new Setting<string[]>(this, "Mod search paths", "The paths in which to search for packed *.mod files.",
                new[] {
                    Path.Combine(Directory.GetCurrentDirectory(), "mods"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Saved Games", "MP Tanks 2D", "mods")
                });

            ModUnpackPath = new Setting<string>(this, "Mod temp directory",
                "The place to store mods that are used at runtime. In other words, this is the directory" +
                " that *.mod files are unpacked into.",
                Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "mptanks", "runtimemods"));

            RenderScale = new Setting<float>(this, "Render Scale",
            "The scale of rendering relative to game space so integer conversions work", 100f);

            PhysicsCompensationForRendering = new Setting<float>(this, "Physics Skin Compensation",
                "The amount in blocks to compensate for Farseer Physics's skin on bodies.", 0.085f);
        }
    }
}
