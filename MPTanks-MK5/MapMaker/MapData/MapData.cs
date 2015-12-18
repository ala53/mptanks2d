using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.MapMaker.MapData
{
    class MapData
    {
        public List<Modding.ModInfo> Mods { get; set; } = new List<Modding.ModInfo>();

        public string GenerateMap()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new Engine.Maps.Serialization.MapJSON());
        }
    }
}
