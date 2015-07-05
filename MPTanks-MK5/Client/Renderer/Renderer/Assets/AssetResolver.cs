using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Renderer.Renderer.Assets
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
            foreach (var directory in SearchPaths)
            {
                foreach (var file in Directory.GetFiles(directory))
                {
                    var fi = new FileInfo(file);
                    if (fi.Name.Equals(sheetName, StringComparison.OrdinalIgnoreCase))
                    {
                        return file;
                    }
                }
            }
            return null;
        }
    }
}
