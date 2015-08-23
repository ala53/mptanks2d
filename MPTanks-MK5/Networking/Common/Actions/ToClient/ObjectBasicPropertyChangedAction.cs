using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    /// <summary>
    /// Marks a change to one of the base properties that all game objects have.
    /// </summary>
    public class ObjectBasicPropertyChangedAction : ActionBase
    {
        public ushort ObjectId { get; set; }
        public GameObject.BasicPropertyChangeEventType EventType { get; set; }
        public Vector2 VectorValue { get; set; }
        public float FloatValue { get; set; }
        public bool BoolValue { get; set; }
        public ObjectBasicPropertyChangedAction(NetIncomingMessage message) : base(message)
        {
            ObjectId = message.ReadUInt16();
            EventType = (GameObject.BasicPropertyChangeEventType)message.ReadByte();
            if (EventType == GameObject.BasicPropertyChangeEventType.AngularVelocity ||
                EventType == GameObject.BasicPropertyChangeEventType.LinearVelocity ||
                EventType == GameObject.BasicPropertyChangeEventType.Size)
            {
                VectorValue = new HalfVector2 { PackedValue = message.ReadUInt32() }.ToVector2();
            }
            else if (EventType == GameObject.BasicPropertyChangeEventType.Position)
            {
                VectorValue = new Vector2(message.ReadFloat(), message.ReadFloat());
            }
            else if (EventType == GameObject.BasicPropertyChangeEventType.Health ||
                EventType == GameObject.BasicPropertyChangeEventType.Restitution ||
                EventType == GameObject.BasicPropertyChangeEventType.Rotation)
                FloatValue = new Half() { InternalValue = message.ReadUInt16() };
            else if (EventType == GameObject.BasicPropertyChangeEventType.IsSensor ||
                EventType == GameObject.BasicPropertyChangeEventType.IsStatic)
                BoolValue = message.ReadBoolean();
        }
        public ObjectBasicPropertyChangedAction(
            GameObject.BasicPropertyChangeEventType type, GameObject.BasicPropertyChangeArgs args)
        {
            ObjectId = args.Owner.ObjectId;
            EventType = type;
            if (EventType == GameObject.BasicPropertyChangeEventType.AngularVelocity ||
                EventType == GameObject.BasicPropertyChangeEventType.LinearVelocity ||
                EventType == GameObject.BasicPropertyChangeEventType.Position ||
                EventType == GameObject.BasicPropertyChangeEventType.Size)
                VectorValue = args.VectorValue;
            else if (EventType == GameObject.BasicPropertyChangeEventType.Health ||
                EventType == GameObject.BasicPropertyChangeEventType.Restitution ||
                EventType == GameObject.BasicPropertyChangeEventType.Rotation)
                FloatValue = args.FloatValue;
            else if (EventType == GameObject.BasicPropertyChangeEventType.IsSensor ||
                EventType == GameObject.BasicPropertyChangeEventType.IsStatic)
                BoolValue = args.BoolValue;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(ObjectId);
            message.Write((byte)EventType);
            if (EventType == GameObject.BasicPropertyChangeEventType.AngularVelocity ||
                EventType == GameObject.BasicPropertyChangeEventType.LinearVelocity ||
                EventType == GameObject.BasicPropertyChangeEventType.Size)
            {
                message.Write(new HalfVector2(VectorValue).PackedValue);//Packing optimization to halve size
            }
            else if (EventType == GameObject.BasicPropertyChangeEventType.Position)
            {
                message.Write(VectorValue.X);
                message.Write(VectorValue.Y);
            }
            else if (EventType == GameObject.BasicPropertyChangeEventType.Health ||
                EventType == GameObject.BasicPropertyChangeEventType.Restitution ||
                EventType == GameObject.BasicPropertyChangeEventType.Rotation)
                message.Write(new Half(FloatValue).InternalValue);
            else if (EventType == GameObject.BasicPropertyChangeEventType.IsSensor ||
                EventType == GameObject.BasicPropertyChangeEventType.IsStatic)
                message.Write(BoolValue);
        }
    }
}
