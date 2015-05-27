using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Rendering.Renderer.Assets
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
            return "";
        }
    }
}
