using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Events
{
    public class FatalError
    {
        public string StackTrace { get; set; }
        public Exception Exception { get; set; }
        public string FriendlyMessage { get; set; }
    }
}
