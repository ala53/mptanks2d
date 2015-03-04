using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.GameStates
{
    struct PartialObjectState
    {

        public ushort ObjectId;
        //Has the Location on the X axis changed
        public bool LocationXChanged;
        //Has the Location on the Y axis changed
        public bool LocationYChanged;
        //Has the rotation changed
        public bool RotationChanged;
        //Has the velocity on the X axis changed
        public bool VelocityXChanged;
        //Has the velocity on the Y axis changed
        public bool VelocityYChanged;
        //Has the object state changed.
        public bool HealthChanged;
        //Is there any extended data that should
        //be passed off to the object itself.
        public bool ExtendedDataChanged;

        public float LocationX;
        public float LocationY;
        public float Rotation;
        public float VelocityX;
        public float VelocityY;
        public ushort Health;
        public byte[] ExtendedData;

        /// <summary>
        /// Turns a full object state into a delta by culling identical or near identical values.
        /// </summary>
        /// <param name="lastState"></param>
        /// <returns></returns>
        public PartialObjectState ConvertFullStateToDeltaState(PartialObjectState lastState)
        {
            const float locationTolerance = 0.5f;
            const float rotationTolerance = 0.5f;
            const float velocityTolerance = 0.1f;

            var obj = this;

            if (Math.Abs(LocationX - lastState.LocationX) > locationTolerance) //out of reasonable tolerance
                obj.LocationXChanged = true;
            else
                obj.LocationXChanged = false;

            if (Math.Abs(LocationY - lastState.LocationY) > locationTolerance) //out of reasonable tolerance
                obj.LocationYChanged = true;
            else
                obj.LocationYChanged = false;

            if (Math.Abs(Rotation - lastState.Rotation) > rotationTolerance) //out of reasonable tolerance
                obj.RotationChanged = true;
            else
                obj.RotationChanged = false;

            if (Math.Abs(VelocityX - lastState.VelocityX) > velocityTolerance) //out of reasonable tolerance
                obj.VelocityXChanged = true;
            else
                obj.VelocityXChanged = false;

            if (Math.Abs(VelocityY - lastState.VelocityY) > velocityTolerance) //out of reasonable tolerance
                obj.VelocityYChanged = true;
            else
                obj.VelocityYChanged = false;

            if (Health != lastState.Health) //health has changed
                obj.HealthChanged = true;
            else
                obj.HealthChanged = false;

            if (!ExtendedData.SequenceEqual(lastState.ExtendedData)) //it has changed
                obj.ExtendedDataChanged = true;
            else
                obj.ExtendedDataChanged = false;

            return obj;
        }

        public void Write(Lidgren.Network.NetOutgoingMessage message)
        {
            //A 1 byte overhead because we encode which
            //data is stored in the packet.
            message.Write(LocationXChanged);
            message.Write(LocationYChanged);
            message.Write(Rotation);
            message.Write(VelocityXChanged);
            message.Write(VelocityYChanged);
            message.Write(HealthChanged);
            message.Write(ExtendedDataChanged);
            message.WritePadBits();

            //Then write the actual data where necessary.

            if (LocationXChanged)
                message.Write(LocationX);
            if (LocationYChanged)
                message.Write(LocationY);
            if (RotationChanged) //Compress rotation
                message.WriteSignedSingle(Rotation, 16);
            if (VelocityXChanged) //and velocity
                message.WriteSignedSingle(VelocityX, 16);
            if (VelocityYChanged)
                message.WriteSignedSingle(VelocityY, 16);
            if (HealthChanged)
                message.Write(Health);
            if (ExtendedDataChanged)
            {
                message.Write((byte)ExtendedData.Length);
                message.Write(ExtendedData);
            }

        }

        public static PartialObjectState ReadMessage(Lidgren.Network.NetIncomingMessage message)
        {
            var obj = new PartialObjectState();

            obj.LocationXChanged = message.ReadBoolean();
            obj.LocationYChanged = message.ReadBoolean();
            obj.RotationChanged = message.ReadBoolean();
            obj.VelocityXChanged = message.ReadBoolean();
            obj.VelocityYChanged = message.ReadBoolean();
            obj.HealthChanged = message.ReadBoolean();
            obj.ExtendedDataChanged = message.ReadBoolean();
            message.SkipPadBits();

            if (obj.LocationXChanged)
                obj.LocationX = message.ReadFloat();
            if (obj.LocationYChanged)
                obj.LocationY = message.ReadFloat();
            if (obj.RotationChanged)
                obj.Rotation = message.ReadSignedSingle(16);
            if (obj.VelocityXChanged)
                obj.VelocityX = message.ReadSignedSingle(16);
            if (obj.VelocityYChanged)
                obj.VelocityY = message.ReadSignedSingle(16);
            if (obj.HealthChanged)
                obj.Health = message.ReadUInt16();
            if (obj.ExtendedDataChanged)
            {
                var tmp = message.ReadByte();
                obj.ExtendedData = message.ReadBytes(tmp);
            }

            return obj;
        }
    }
}
