using MPTanks.Engine.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Chat
{
    public class ChatQueue
    {
        private ILogger _logger;
        public ChatQueue(ILogger logger)
        {
            _logger = logger;
        }
    }
}
