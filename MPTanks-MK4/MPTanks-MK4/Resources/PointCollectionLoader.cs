using MPTanks_MK4.Resources.Resource;
using Newtonsoft.Json;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MPTanks_MK4.Resources
{
    class PointCollectionLoader
    {
        public PointCollection LoadPoints(string data)
        {
            var obj = new PointCollection();
            Newtonsoft.Json.JsonConvert.PopulateObject(data, obj);
            return obj;
        }
    }
}
