using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Modding.Unpacker
{
    public class ModHeader
    {
        public string[] CodeFiles { get; set; }
        public string[] DLLFiles { get; set; }
        public string[] ImageFiles { get; set; }
        public string[] SoundFiles { get; set; }
        public string[] ComponentFiles { get; set; }
        public string[] MapFiles { get; set; }
        public ModDependency[] Dependencies { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public string Tag { get; set; } = "Release";
        public string DatabaseUrl { get; set; } = "FILLED_IN_BY_ZSB_SERVER";
    }

    public class ModDependency
    {
        public string ModName { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public string DatabaseUrl { get; set; } = "FILLED_IN_BY_ZSB_SERVER";
    }
}
