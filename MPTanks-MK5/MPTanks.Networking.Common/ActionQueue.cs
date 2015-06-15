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
        private List<QueueFrame> _items = new List<QueueFrame>();
        public IReadOnlyList<QueueFrame> Items { get { return _items; } }

        public void NextFrame()
        {
            _items.Add(QueueFrame.Get());
            if (_items.Count > Settings.Instance.MaxActionFrameCount)
                _items.RemoveAt(_items.Count - 1);
        }

        public void AddAction(Actions.Action action)
        {
            _items[_items.Count - 1].Actions.Add(action);
        }
        public class QueueFrame
        {
            public int FrameNumber { get; set; }
            public List<Actions.Action> Actions { get; set; }

            public static QueueFrame Get()
            {
                var frame = Pool.Get<QueueFrame>();
                frame.FrameNumber = 0;
                frame.Actions.Clear();
                return frame;
            }
        }
    }
}
