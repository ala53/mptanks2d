﻿using Lidgren.Network;
using MPTanks.Engine;
using MPTanks.Engine.Logging;
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
        public virtual ILogger Logger { get; set; }
        public NetworkProcessorDiagnostics Diagnostics { get; private set; } = new NetworkProcessorDiagnostics();

        #region Message Processing
        private static byte _currentMessageTypeId = 0;
        private static Dictionary<byte, Type> _toServerMessageTypes = new Dictionary<byte, Type>();
        private static Dictionary<byte, Type> _toClientMessageTypes = new Dictionary<byte, Type>();
        private static Dictionary<byte, Type> _toServerActionTypes = new Dictionary<byte, Type>();
        private static Dictionary<byte, Type> _toClientActionTypes = new Dictionary<byte, Type>();
        private static Dictionary<byte, Type> _allTypes = new Dictionary<byte, Type>();
        private static Dictionary<Type, byte> _allTypesReverse = new Dictionary<Type, byte>();

        public IReadOnlyDictionary<Type, byte> TypeIndexTable => _allTypesReverse;

        public static void RegisterToClientMessageType(Type messageType)
        {
            _toClientMessageTypes.Add(++_currentMessageTypeId, messageType);
            _allTypes.Add(_currentMessageTypeId, messageType);
            _allTypesReverse.Add(messageType, _currentMessageTypeId);
        }
        public static void RegisterToServerMessageType(Type messageType)
        {
            _toServerMessageTypes.Add(++_currentMessageTypeId, messageType);
            _allTypes.Add(_currentMessageTypeId, messageType);
            _allTypesReverse.Add(messageType, _currentMessageTypeId);
        }
        public static void RegisterToClientActionType(Type actionType)
        {
            _toClientActionTypes.Add(++_currentMessageTypeId, actionType);
            _allTypes.Add(_currentMessageTypeId, actionType);
            _allTypesReverse.Add(actionType, _currentMessageTypeId);
        }

        public static void RegisterToServerActionType(Type actionType)
        {
            _toServerActionTypes.Add(++_currentMessageTypeId, actionType);
            _allTypes.Add(_currentMessageTypeId, actionType);
            _allTypesReverse.Add(actionType, _currentMessageTypeId);
        }

        public void ProcessMessages(NetIncomingMessage messageBlock)
        {
            if (GlobalSettings.Debug)
                ProcessMessagesInternal(messageBlock);
            else
                try
                {
                    ProcessMessagesInternal(messageBlock);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error! Message Queue Processing failed.", ex);
                }
        }
        private void ProcessMessagesInternal(NetIncomingMessage messageBlock)
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
                if (_allTypes.ContainsKey(msgId))
                    if (GlobalSettings.Debug)
                        ProcessMessage(msgId, messageBlock);
                    else
                        try
                        {
                            ProcessMessage(msgId, messageBlock);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("Message parsing error!", ex);
                            Logger.Error($"Message type: {_allTypes[msgId].FullName}");
                            OnProcessingError(ex);
                        }
        }

        public void ProcessMessage(byte id, NetIncomingMessage message)
        {
            if (_allTypes.ContainsKey(id))
                Diagnostics.AddMsg(_allTypes[id]);
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

        public virtual void ProcessToServerMessage(MessageBase message)
        {

        }

        public virtual void ProcessToClientMessage(NetConnection client, MessageBase message)
        {

        }

        public virtual void ProcessToServerAction(ActionBase action)
        {

        }

        public virtual void ProcessToClientAction(NetConnection client, ActionBase action)
        {

        }

        public virtual void OnProcessingError(Exception error)
        {

        }
        #endregion  

        private List<MessageBase> _messages = new List<MessageBase>();
        public IReadOnlyList<MessageBase> MessageQueue => _messages;
        public void SendMessage(MessageBase message)
        {
            _messages.Add(message);
        }

        public void WriteMessages(NetOutgoingMessage message)
        {
            message.Write((ushort)_messages.Count);
            foreach (var msg in _messages)
                message.Write(_allTypesReverse[msg.GetType()]);
            foreach (var msg in _messages)
            {
                msg.Serialize(message);
            }
            _messages.Clear();
        }

        public void ClearQueue() => _messages.Clear();
    }

    public class NetworkProcessorDiagnostics
    {
        private Dictionary<Type, int> _usesByType = new Dictionary<Type, int>();

        public void Reset()
        {
            _usesByType.Clear();
        }
        public IReadOnlyDictionary<Type, int> GetMostUsed(int count)
        {
            var sorted = _usesByType.OrderByDescending(a => a.Value);
            return sorted.Take(count).ToDictionary(a => a.Key, a => a.Value);
        }

        internal void AddMsg(Type t)
        {
            if (!_usesByType.ContainsKey(t))
                _usesByType.Add(t, 0);
            _usesByType[t] = _usesByType[t] + 1;
        }
    }
}
