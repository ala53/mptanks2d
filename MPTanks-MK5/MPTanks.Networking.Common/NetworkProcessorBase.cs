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

        private List<__MessageRaw> _messagesParsed = new List<__MessageRaw>();
        public void ProcessMessages(byte[] messagesData)
        {
            _messagesParsed.Clear();
            for (var i = 0; i < messagesData.Length;)
            {
                try
                {
                    var pkt = new __MessageRaw();
                    pkt.PacketId = messagesData[i++];
                    pkt.ContentsLength = BitConverter.ToUInt16(messagesData, i);
                    i += 2;
                    pkt.Contents = messagesData.Slice(i, pkt.ContentsLength);
                    i += pkt.ContentsLength;

                    _messagesParsed.Add(pkt);
                }
                catch (Exception ex)
                {
                    if (GlobalSettings.Debug) throw ex;
                }
            }

            foreach (var message in _messagesParsed)
            {
                try
                {
                    ProcessMessage(message.PacketId, message.Contents);
                }
                catch (Exception ex)
                {
                    if (GlobalSettings.Debug) throw ex;
                }
            }
        }

        private struct __MessageRaw
        {
            public byte PacketId;
            public ushort ContentsLength;
            public byte[] Contents;
        }

        public void ProcessMessage(byte id, byte[] data)
        {
            if (_toServerMessageTypes.ContainsKey(id))
            {
                var obj = (MessageBase)Activator.CreateInstance(_toServerMessageTypes[id], data);
                ProcessToServerMessage(obj);
            }
            else if (_toClientMessageTypes.ContainsKey(id))
            {
                var obj = (MessageBase)Activator.CreateInstance(_toClientMessageTypes[id], data);
                ProcessToClientMessage(obj);
            }
            else if (_toServerActionTypes.ContainsKey(id))
            {
                var obj = (ActionBase)Activator.CreateInstance(_toServerActionTypes[id], data);
                ProcessToServerAction(obj);
            }
            else if (_toClientActionTypes.ContainsKey(id))
            {
                var obj = (ActionBase)Activator.CreateInstance(_toClientActionTypes[id], data);
                ProcessToClientAction(obj);
            }
        }

        public abstract void ProcessToServerMessage(MessageBase message);
        public abstract void ProcessToClientMessage(MessageBase message);

        public abstract void ProcessToServerAction(ActionBase action);

        public abstract void ProcessToClientAction(ActionBase action);
    }
}
