using Microsoft.Xna.Framework;
using MPTanks.Engine.Helpers;
using MPTanks.Engine.Settings;
using Newtonsoft.Json;
using System;
using System.Text;

namespace MPTanks.Engine
{
    public partial class GameObject
    {
        private bool _eventsEnabled = true;
        
        private Core.Events.Types.GameObjects.StateChangedEventArgs _stateArgs =
            new Core.Events.Types.GameObjects.StateChangedEventArgs();

        private TimeSpan _lastStateChange = TimeSpan.Zero;
        protected bool RaiseStateChangeEvent(byte[] newStateData)
        {
            if (!Game.Authoritative || newStateData == null || newStateData.Length == 0
                || newStateData.Length > Game.Settings.MaxStateChangeSize ||
                (TimeAlive - _lastStateChange) < TimeSpan.FromMilliseconds(Game.Settings.MaxStateChangeFrequency) ||
                !_eventsEnabled)
                return false;

            _stateArgs.Object = this;
            _stateArgs.State = newStateData;
            Game.EventEngine.RaiseGameObjectStateChanged(_stateArgs);

            return true;
        }

        protected bool RaiseStateChangeEvent(string state)
        {
            return RaiseStateChangeEvent(SerializationHelpers.Serialize(state));
        }
        /// <summary>
        /// Serializes the object to JSON before sending it.
        /// </summary>
        /// <param name="obj"></param>
        protected bool RaiseStateChangeEvent(object obj)
        {
            return RaiseStateChangeEvent(SerializationHelpers.Serialize(obj));
        }

        public void ReceiveStateData(byte[] stateData)
        {
            if (GlobalSettings.Debug)
                SerializationHelpers.ResolveDeserialize(stateData,
                    ReceiveStateDataInternal, ReceiveStateDataInternal, ReceiveStateDataInternal);
            else
                try
                {
                    SerializationHelpers.ResolveDeserialize(stateData,
                        ReceiveStateDataInternal, ReceiveStateDataInternal, ReceiveStateDataInternal);
                }
                catch (Exception ex)
                {
                    Game.Logger.Error($"GameObject partial state parsing failed! {ReflectionName}[ID {ObjectId}]", ex);
                    ReceiveStateDataInternal(stateData);
                }
        }

        protected virtual void ReceiveStateDataInternal(byte[] stateData) { }

        protected virtual void ReceiveStateDataInternal(dynamic obj) { }

        protected virtual void ReceiveStateDataInternal(string state) { }

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
        public struct BasicPropertyChangeArgs
        {
            public GameObject Owner { get; set; }
            public BasicPropertyChangeEventType Type { get; set; }
            public bool BoolValue { get; set; }
            public Vector2 VectorValue { get; set; }
            public float FloatValue { get; set; }
            public bool OldBoolValue { get; set; }
            public Vector2 OldVectorValue { get; set; }
            public float OldFloatValue { get; set; }
        }
        public enum BasicPropertyChangeEventType : byte
        {
            Position,
            Rotation,
            LinearVelocity,
            AngularVelocity,
            Size,
            Health,
            IsSensor,
            IsStatic,
            Restitution,
        }

        private void RaiseBasicPropertyChange(BasicPropertyChangeEventType type, Vector2 oldValue, Vector2 newValue)
        {
            if (_eventsEnabled)
            {
                Game.EventEngine.RaiseGameObjectBasicPropertyChanged(new BasicPropertyChangeArgs
                {
                    Type = type,
                    VectorValue = newValue,
                    OldVectorValue = oldValue,
                    Owner = this
                });
            }
        }
        private void RaiseBasicPropertyChange(BasicPropertyChangeEventType type, bool oldValue, bool newValue)
        {
            if (_eventsEnabled)
            {
                Game.EventEngine.RaiseGameObjectBasicPropertyChanged(new BasicPropertyChangeArgs
                {
                    Type = type,
                    BoolValue = newValue,
                    OldBoolValue = oldValue,
                    Owner = this
                });
            }
        }
        private void RaiseBasicPropertyChange(BasicPropertyChangeEventType type, float oldValue, float newValue)
        {
            if (_eventsEnabled)
            {
                Game.EventEngine.RaiseGameObjectBasicPropertyChanged(new BasicPropertyChangeArgs
                {
                    Type = type,
                    FloatValue = newValue,
                    OldFloatValue = oldValue,
                    Owner = this
                });
            }
        }

        public void ReceiveBasicPropertyChange(BasicPropertyChangeEventType type, float value)
        {
            if (type == BasicPropertyChangeEventType.AngularVelocity)
                AngularVelocity = value;
            if (type == BasicPropertyChangeEventType.Rotation)
                Rotation = value;
            if (type == BasicPropertyChangeEventType.Restitution)
                Restitution = value;
            if (type == BasicPropertyChangeEventType.Health)
                Health = value;
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

        public void ReceiveBasicPropertyChange(BasicPropertyChangeEventType type, bool value)
        {
            if (type == BasicPropertyChangeEventType.IsSensor)
                IsSensor = value;
            if (type == BasicPropertyChangeEventType.IsStatic)
                IsStatic = value;
        }
        #endregion

        private void RaiseOnDestroyed(GameObject destroyer = null)
        {
            if (_eventsEnabled)
                Game.EventEngine.RaiseGameObjectDestroyed(this, destroyer);
        }

        private void RaiseOnDestructionEnded()
        {
            if (_eventsEnabled)
                Game.EventEngine.RaiseGameObjectDestructionEnded(this);
        }

        private void RaiseOnCreated()
        {
            if (_eventsEnabled)
                Game.EventEngine.RaiseGameObjectCreated(this);
        }
    }
}
