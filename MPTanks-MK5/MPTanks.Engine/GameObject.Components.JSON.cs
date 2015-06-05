using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Serialization
{
    class GameObjectComponentsJSON
    {
        public string Name { get; set; }
        public string ReflectionName { get; set; }
        public GameObjectSheetSpecifierJSON Sheet { get; set; }
    }
    class GameObjectComponentJSON
    {

    }
    class GameObjectSheetSpecifierJSON
    {
        public bool FromThisMod { get; set; }
        public string AssetName { get; set; }
        public string ModName { get; set; }
    }

}
