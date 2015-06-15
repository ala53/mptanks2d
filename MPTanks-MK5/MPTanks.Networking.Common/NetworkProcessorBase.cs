using Lidgren.Network;
using MPTanks.Engine;
using MPTanks.Engine.Settings;
using MPTanks.Networking.Common.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common
{
    public abstract class NetworkProcessorBase
    {
        public virtual NetPeer Peer { get; protected set; }
        
        #region Message Processing
        private static byte _currentMessageTypeId = 0;
        private static Dictionary<byte, Type> _toServerMessageTypes = new Dictionary<byte, Type>();
        private static Dictionary<byte, Type> _toClientMessageTypes = new Dictionary<byte, Type>();
        private static Dictionary<byte, Type> _toServerActionTypes = new Dictionary<byte, Type>();
        private static Dictionary<byte, Type> _toClientActionTypes = new Dictionary<byte, Type>();

        public static void RegisterToClientMessageType(Type messageType)
        {
            _toClientMessageTypes.Add(_currentMessageTypeId++, messageType);
        }
        public static void RegisterToServerMessageType(Type messageType)
        {
            _toServerMessageTypes.Add(_currentMessageTypeId++, messageType);
        }
        public static void RegisterToClientActionType(Type actionType)
        {
            _toClientActionTypes.Add(_currentMessageTypeId++, actionType);
        }

        public static void RegisterToServerActionType(Type actionType)
        {
            _toServerActionTypes.Add(_currentMessageTypeId++, actionType);
        }

        public void ProcessMessages(NetIncomingMessage messageBlock)
        {
            //Layout:
            //Message count: 2 bytes
            //message 1 type id
            // message 2 type id
            //...
            //message one contents
            //message 2 contents

            var msgCount = messageBlock.ReadUInt16();
            var msgIds = messageBlock.ReadBytes(msgCount);

            foreach (var msgId in msgIds)
                ProcessMessage(msgId, messageBlock);
        }

        public void ProcessMessage(byte id, NetIncomingMessage message)
        {
            if (_toServerMessageTypes.ContainsKey(id))
            {
                var obj = (MessageBase)Activator.CreateInstance(_toServerMessageTypes[id], message);
                ProcessToServerMessage(obj);
            }
            else if (_toClientMessageTypes.ContainsKey(id))
            {
                var obj = (MessageBase)Activator.CreateInstance(_toClientMessageTypes[id], message);
                ProcessToClientMessage(message.SenderConnection, obj);
            }
            else if (_toServerActionTypes.ContainsKey(id))
            {
                var obj = (ActionBase)Activator.CreateInstance(_toServerActionTypes[id], message);
                ProcessToServerAction(obj);
            }
            else if (_toClientActionTypes.ContainsKey(id))
            {
                var obj = (ActionBase)Activator.CreateInstance(_toClientActionTypes[id], message);
                ProcessToClientAction(message.SenderConnection, obj);
            }
        }

        public abstract void ProcessToServerMessage(MessageBase message);
        public abstract void ProcessToClientMessage(NetConnection client, MessageBase message);

        public abstract void ProcessToServerAction(ActionBase action);

        public abstract void ProcessToClientAction(NetConnection client, ActionBase action);

        #endregion  
    }
}
