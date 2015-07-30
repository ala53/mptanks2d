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
        public AssetResolver(string[] searchPaths)
        {
            SearchPaths = searchPaths;
        }

        public string ResolveAssetFile(string sheetName)
        {
            if (Path.IsPathRooted(sheetName) && 
                File.Exists(sheetName)) return sheetName;

            string addedPath = null;
            var fName = sheetName;
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
            foreach (var dirBase in SearchPaths)
            {
                var directory = dirBase;
                if (addedPath != null)
                    directory += "\\" + addedPath;
                if (Directory.Exists(directory))
                    foreach (var file in Directory.GetFiles(directory))
                    {
                        var fi = new FileInfo(file);
                        if (fi.Name.Equals(fName, StringComparison.OrdinalIgnoreCase))
                        {
                            return file;
                        }
                    }
            }
            return null;
        }
    }
}
