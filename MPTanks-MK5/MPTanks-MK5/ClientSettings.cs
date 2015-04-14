using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.GameClient
{
    class ClientSettings
    {
        public static string LogLocation = "${basedir}/clientlogs/client.log";
        /// <summary>
        /// The maximum number of on screen particles to allow
        /// </summary>
        public static int MaxParticlesToRender = 10000;

        /// <summary>
        /// Whether to force a gen 0 GC every frame. This gets rid of
        /// most temporary objects, helping to trace actual memory leaks.
        /// Unfortunately, it does so to the possible detriment of 
        /// framerate.
        /// </summary>
        public static bool ForceFullGCEveryFrame = false;
        public static bool ForceGen0GCEveryFrame = false;

        /// <summary>
        /// The number of instances of a sound that can be playing at the same time.
        /// </summary>
        public static int MaxInstancesOfOneSoundAllowed = 4;

        public static string[] AssetAllowedFileExtensions = {
            ".dds",
            ".png",
            ".jpg",
            ".jpeg",
            ".bmp",
            ".gif"
            };

        //Where config information is stored
        public static string ConfigDir = 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Saved Games", "MP Tanks 2D");

        //Where to look for assets
        public static string[] AssetSearchPaths = {
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
            Path.Combine(ConfigDir, "assets")
            };

        //Looks for *.mod files to unpack
        public static string[] ModSearchPaths = {
            Path.Combine(Directory.GetCurrentDirectory(), "mods"),
            Path.Combine(ConfigDir, "mods")
            };

        //Intentionally left blank here. This is for the user to load mods without whitelisting on their local computer
        public static string[] TrustedModPaths = {};

        //Stores mods in a runtime directory. That way, when we download mods from servers, we 
        //just leave them in the runtime directory where they are removed next time the program opens
        public static string ModUnpackPath =
            Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "mptanks", "runtimemods");
        }
}
