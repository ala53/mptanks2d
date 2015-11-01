using MPTanks.Engine.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.GameSandbox.Mods
{
    class AsyncModLoader
    {
        public int TotalCount { get; private set; }
        public int CompletedCount { get; private set; }
        public bool Finished { get; private set; }
        public bool Errored { get; private set; }
        private string _status = "";
        private object _syncLock = new object();
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                lock (_syncLock)
                    _status = value;
            }
        }

        public Task AsyncLoaderTask { get; private set; }
        public bool Running => !Finished;

        private AsyncModLoader() { }
        public static AsyncModLoader Create(GameSettings settings, params string[] otherModNames)
        {
            var ml = new AsyncModLoader();

            ml.Status = "Starting...";
            ml.TotalCount = settings.CoreMods.Value.Length + otherModNames.Length;

            ml.AsyncLoaderTask = Task.Run(() =>
            {
                //Load core mods
                string errors = "";
                bool hasError = false;
                foreach (var modFile in settings.CoreMods.Value)
                {
                    ml.Status = $"Loading ({ml.CompletedCount} / {ml.TotalCount}): (Core) {new FileInfo(modFile).Name}";

                    if (GlobalSettings.Debug)
                        LoadFullTrustModInternal(modFile, settings, ref errors, ref hasError);
                    else
                        try { LoadFullTrustModInternal(modFile, settings, ref errors, ref hasError); }
                        catch (Exception ex) { Logger.Error("Mod loader (Core mods)", ex); }

                    ml.CompletedCount++;
                }

                foreach (var modInfo in otherModNames)
                {
                    ml.Status = $"Loading ({ml.CompletedCount} / {ml.TotalCount}): {modInfo}";

                    if (GlobalSettings.Debug)
                        LoadFullTrustModInternal(modInfo, settings, ref errors, ref hasError);
                    else
                        try { LoadFullTrustModInternal(modInfo, settings, ref errors, ref hasError); }
                        catch (Exception ex) { Logger.Error("Mod loader (Core mods)", ex); }

                    ml.CompletedCount++;
                }

                if (hasError)
                {
                    ml.Status = "Errored";
                    Logger.Error("Mod loader errors:");
                    Logger.Error(errors);
                    ml.Errored = true;
                }
                else ml.Status = "Complete";

                ml.Finished = true;
            });

            return ml;
        }


        /// <summary>
        /// Loads a mod from a file. E.g. C:\files\modname.mod (in a full trust context)
        /// </summary>
        /// <param name="modFile"></param>
        /// <param name="settings"></param>
        /// <param name="errors"></param>
        private static void LoadFullTrustModInternal(string modFile, GameSettings settings, ref string errors, ref bool hasError)
        {
            string err = "";
            Logger.Info($"Loading core (trusted) mod {modFile}");

            var mod = Modding.ModLoader.LoadMod(modFile, settings.ModUnpackPath, settings.ModMapPath,
                settings.ModAssetPath, out err, false, GlobalSettings.Debug);

            if (mod == null)
                hasError = true;
            errors += "\n\n\n" + err;
        }

        /// <summary>
        /// Loads a mod from its name and version tag
        /// E.g. ModName 1.02-TAG`
        /// </summary>
        /// <param name="modNameWithVersion"></param>
        /// <param name="settings"></param>
        /// <param name="errors"></param>
        private static void LoadUntrustedModInternal(string modNameWithVersion, GameSettings settings, ref string errors, ref bool hasError)
        {
            string err = "";
            Logger.Info($"Loading secondary mod {modNameWithVersion}");

            var name = modNameWithVersion.Split(' ')[0];
            int major;
            try { major = int.Parse(modNameWithVersion.Split(' ')[1].Split('.')[0]); }
            catch
            {
                hasError = true;
                errors += "\n\n\n";
                errors += "PARSE ERROR!\n";
                errors += $"Cannot parse version of {modNameWithVersion}\n";
                return;
            }
            //Find the mod
            var modInfo = Modding.ModDatabase.Get(name, major);

            var mod = Modding.ModLoader.LoadMod(modInfo.File, settings.ModUnpackPath, settings.ModMapPath,
                settings.ModAssetPath, out err, modInfo.UsesWhitelist, GlobalSettings.Debug);

            if (mod == null)
                hasError = true;

            errors += "\n\n\n" + err;
        }
    }
}
