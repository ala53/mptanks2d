using MPTanks.Networking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server
{
    static class MessagePool
    {
        private static int _maxCount = 100;
        private static Dictionary<Type, Stack<MessageBase>> _messages = new Dictionary<Type, Stack<MessageBase>>();
        public static T Retrieve<T>() where T : MessageBase, new()
        {
            if (!_messages.ContainsKey(typeof(T)))
            {
                _messages[typeof(T)] = new Stack<MessageBase>();
            }
            var list = _messages[typeof(T)];
            if (list.Count > 0)
                return (T)list.Pop();

            return new T();
        }

        public static void Release(MessageBase msg)
        {
            var type = msg.GetType();
            if (!_messages.ContainsKey(type))
            {
                _messages[type] = new Stack<MessageBase>();
            }
            var list = _messages[type];
            if (list.Count >= _maxCount)
                return;
            list.Push(msg);
        }
    }
}
