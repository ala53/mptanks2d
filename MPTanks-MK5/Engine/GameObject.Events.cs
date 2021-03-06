﻿using Microsoft.Xna.Framework;
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
        protected bool RaiseStateChangeEvent(Action<ByteArrayWriter> eventWriter)
        {
            if (eventWriter == null)
                return false;
            var writer = ByteArrayWriter.Get();

            eventWriter(writer);

            if (!Game.Authoritative || writer == null || writer.Size == 0
                || writer.Size > Game.Settings.MaxStateChangeSize ||
                (TimeAlive - _lastStateChange) < Game.Settings.MaxStateChangeFrequency ||
                !_eventsEnabled)
                return false;

            _stateArgs.Object = this;
            _stateArgs.State = writer.Data;
            Game.EventEngine.RaiseGameObjectStateChanged(_stateArgs);

            writer.Release();

            return true;
        }

        public void ReceiveStateData(byte[] stateData)
        {
            var rdr = ByteArrayReader.Get(stateData);
            if (GlobalSettings.Debug)
                ReceiveStateDataInternal(rdr);
            else
                try
                {
                    ReceiveStateDataInternal(rdr);
                }
                catch (Exception ex)
                {
                    Game.Logger.Error($"GameObject partial state parsing failed! {ReflectionName}[ID {ObjectId}]", ex);
                }

            rdr.Release();
        }

        protected virtual void ReceiveStateDataInternal(ByteArrayReader reader) { }

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
