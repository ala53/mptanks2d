using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Infrastructure.ServerHoster
{
    class ActiveGame
    {
        public long Id { get; set; } = -1;
        public AppDomain Domain { get; set; }

        public void Unload()
        {

        }
    }
}
