using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.UI
{
    public class OldPageObject
    {
        public UserInterfacePage OldPage { get; set; }
        public object OldState => OldPage?.State;
    }
}
