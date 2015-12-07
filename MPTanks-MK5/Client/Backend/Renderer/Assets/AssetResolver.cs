using MPTanks.Engine.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Renderer.Assets
{
    class AssetResolver
    {
        public string[] SearchPaths { get; private set; }
        private GameCoreRenderer _renderer;
        public ILogger Logger => _renderer.Logger;
        public AssetResolver(string[] searchPaths, GameCoreRenderer renderer)
        {
            SearchPaths = searchPaths;
            _renderer = renderer;
        }

        public string ResolveAssetFile(string sheetName)
        {
            Logger.Trace($"Resolving file for {sheetName}");
            if (Path.IsPathRooted(sheetName) &&
                File.Exists(sheetName))
            {
                Logger.Trace($"{sheetName} is rooted and exists.");
                return sheetName;
            }

            string addedPath = null;
            var fName = sheetName;
            //Try to get the file name from the sheet name
            if (sheetName.Contains("\\"))
            {
                addedPath = sheetName.Substring(0, sheetName.LastIndexOf("\\"));
                fName = sheetName.Substring(sheetName.LastIndexOf("\\") + 1);
            }
            if (sheetName.Contains("/"))
            {
                addedPath = sheetName.Substring(0, sheetName.LastIndexOf("//"));
                fName = sheetName.Substring(sheetName.LastIndexOf("/"));
            }

            Logger.Trace($"{sheetName}: path = {addedPath}, filename = {fName}");

            foreach (var dirBase in SearchPaths)
            {
                var directory = dirBase;
                if (addedPath != null)
                    directory += "\\" + addedPath;
                if (Directory.Exists(directory))
                    foreach (var file in Directory.GetFiles(directory))
                    {
                        Logger.Trace($"Searching directory {directory} for {sheetName}");
                        var fi = new FileInfo(file);
                        if (fi.Name.Equals(fName, StringComparison.OrdinalIgnoreCase))
                        {
                            Logger.Trace($"{sheetName} found in {directory}");
                            return file;
                        }
                    }
            }

            Logger.Warning($"{sheetName} not found.");
            return null;
        }
    }
}
