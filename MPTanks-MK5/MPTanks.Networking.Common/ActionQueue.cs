using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common
{
    /// <summary>
    /// A queue of actions
    /// </summary>
    public class ActionQueue
    {
        private List<QueueItem> _items = new List<QueueItem>();
        public IReadOnlyList<QueueItem> Items { get { return _items; } }

        public class QueueItem
        {
            public Actions.Action Action { get; set; }
            public int FrameNumber { get; set; }
            public static QueueItem Get()
            {
                return Pool.Get<QueueItem>();
            }
        }
        public class QueueFrame
        {
            public int FrameNumber { get; set; }
            public List<Actions.Action> Actions { get; set; }

            public static QueueFrame Get()
            {
                return Pool.Get<QueueFrame>();
            }
        }
    }
}
