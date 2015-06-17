using Microsoft.Xna.Framework;
using MPTanks.Engine.Helpers;
using Newtonsoft.Json;
using System;
using System.Text;

namespace MPTanks.Engine
{
    public partial class GameObject
    {
        private bool _eventsEnabled = true;

        public event EventHandler<GameObject> OnCreated = delegate { };

        public event EventHandler<Core.Events.Types.GameObjects.DestroyedEventArgs> OnDestroyed = delegate { };

        public event EventHandler<GameObject> OnDestructionEnded = delegate { };

        private Core.Events.Types.GameObjects.StateChangedEventArgs _stateArgs =
            new Core.Events.Types.GameObjects.StateChangedEventArgs();

        public event EventHandler<Core.Events.Types.GameObjects.StateChangedEventArgs> OnStateChanged = delegate { };

        private float _lastStateChange = -10000000;
        protected bool RaiseStateChangeEvent(byte[] newStateData)
        {
            if (!Game.Authoritative || newStateData == null || newStateData.Length == 0
                || newStateData.Length > Game.Settings.MaxStateChangeSize ||
                (TimeAliveMs - _lastStateChange) < Game.Settings.MaxStateChangeFrequency ||
                !_eventsEnabled)
                return false;

            _stateArgs.Object = this;
            _stateArgs.State = newStateData;
            OnStateChanged(this, _stateArgs);

            return true;
        }

        protected bool RaiseStateChangeEvent(string state)
        {
            var count = Encoding.UTF8.GetByteCount(state);
            var array = new byte[count + stringSerializedMagicBytes.Length];
            array.SetContents(stringSerializedMagicBytes, 0);
            array.SetContents(state, stringSerializedMagicBytes.Length);
            return RaiseStateChangeEvent(array);
        }

        const long JSONSerializedMagicNumber = unchecked(0x1337FCEDBCCB3010L);
        byte[] JSONSerializedMagicBytes = BitConverter.GetBytes(JSONSerializedMagicNumber);

        const long stringSerializedMagicNumber = unchecked(0x1337E3EECACB3010L);
        byte[] stringSerializedMagicBytes = BitConverter.GetBytes(stringSerializedMagicNumber);

        /// <summary>
        /// Serializes the object to JSON before sending it.
        /// </summary>
        /// <param name="obj"></param>
        protected bool RaiseStateChangeEvent(object obj)
        {
            var serialized = SerializeStateChangeObject(obj);
            var count = Encoding.UTF8.GetByteCount(serialized);
            var array = new byte[count + JSONSerializedMagicBytes.Length];
            array.SetContents(JSONSerializedMagicBytes, 0);
            array.SetContents(serialized, JSONSerializedMagicBytes.Length);
            return RaiseStateChangeEvent(array);
        }

        private JsonSerializerSettings _serializerSettingsForStateChange = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full,
            TypeNameHandling = TypeNameHandling.All
        };
        protected string SerializeStateChangeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, _serializerSettingsForStateChange);
        }

        public void ReceiveStateData(byte[] stateData)
        {
            if (stateData.SequenceBegins(JSONSerializedMagicBytes))
            {
                //Try to deserialize
                var obj = DeserializeStateChangeObject(stateData.GetString(stringSerializedMagicBytes.Length));
                ReceiveStateDataInternal(obj);
            }
            else if (stateData.SequenceBegins(stringSerializedMagicBytes))
            {
                //Try to deserialize
                var obj = stateData.GetString(stringSerializedMagicBytes.Length);
                ReceiveStateDataInternal(obj);
            }
            else
            {
                ReceiveStateDataInternal(stateData);
            }
        }

        protected object DeserializeStateChangeObject(string obj)
        {
            return JsonConvert.DeserializeObject(obj);
        }
        protected T DeserializeStateChangeObject<T>(string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }

        protected virtual void ReceiveStateDataInternal(byte[] stateData)
        {

        }

        protected virtual void ReceiveStateDataInternal(dynamic obj)
        {

        }

        protected virtual void ReceiveStateDataInternal(string state)
        {

        }
        
        protected void UnsafeDisableEvents()
        {
            _eventsEnabled = false;
        }

        protected void UnsafeEnableEvents()
        {
            _eventsEnabled = true;
        }

        #region RPCs
        protected void RPC(Delegate function, params object[] args)
        {
            Game.RPCHelper.Call(this, function, args);
        }
        /// <summary>
        /// Called when an RPC is received
        /// </summary>
        /// <param name="call"></param>
        public virtual void OnReceiveRPC(RPC.RPC call)
        {

        }
        #endregion

        #region Base Property Changes 
        public event EventHandler<BasicPropertyChangeEventType> OnBasicPropertyChanged = delegate { };
        public enum BasicPropertyChangeEventType : byte
        {
            Position,
            Rotation,
            LinearVelocity,
            AngularVelocity,
            Size,
            IsSensor,
            IsStatic,
            Restitution,
        }

        private void RaiseBasicPropertyChange(BasicPropertyChangeEventType type)
        {
            if (_eventsEnabled)
                OnBasicPropertyChanged(this, type);
        }

        public void ReceiveBasicPropertyChange(BasicPropertyChangeEventType type, float value)
        {
            if (type == BasicPropertyChangeEventType.AngularVelocity)
                AngularVelocity = value;
            if (type == BasicPropertyChangeEventType.Rotation)
                Rotation = value;
        }

        public void ReceiveBasicPropertyChange(BasicPropertyChangeEventType type, Vector2 value)
        {
            if (type == BasicPropertyChangeEventType.LinearVelocity)
                LinearVelocity = value;
            if (type == BasicPropertyChangeEventType.Size)
                Size = value;
            if (type == BasicPropertyChangeEventType.Position)
                Position = value;
        }
        #endregion

        private void RaiseOnDestroyed(GameObject destroyer = null)
        {
            if (_eventsEnabled)
                OnDestroyed(this, new Core.Events.Types.GameObjects.DestroyedEventArgs
                {
                    Destroyed = this,
                    Destroyer = destroyer,
                    Time = DateTime.Now
                });
        }

        private void RaiseOnDestructionEnded()
        {
            if (_eventsEnabled)
                OnDestructionEnded(this, this);
        }

        private void RaiseOnCreated()
        {
            if (_eventsEnabled)
                OnCreated(this, this);
        }
    }
}
