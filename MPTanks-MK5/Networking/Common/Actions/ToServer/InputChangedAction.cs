//#define DBG_FULL_SERIALIZE

using Lidgren.Network;
using Microsoft.Xna.Framework;
using MPTanks.Engine.Tanks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToServer
{
    /// <summary>
    /// Notifies that the input state for a tank was changed by the client.
    /// 5 bytes including message header
    /// </summary>
    public class InputChangedAction : ActionBase
    {
        public InputState InputState { get; private set; }
        //public Vector2 PlayerPosition { get; private set; }
        public InputChangedAction()
        {        }

        public InputChangedAction(Vector2 myPos, InputState state)
        {
            InputState = state;
            //PlayerPosition = myPos;
        }
        protected override void DeserializeInternal(NetIncomingMessage message)
        {
            var iState = new InputState();
#if DBG_FULL_SERIALIZE
            iState.FirePressed = message.ReadBoolean();
            iState.WeaponNumber = message.ReadByte(7);
            iState.LookDirection = message.ReadFloat();
            iState.MovementSpeed = message.ReadFloat();
            iState.RotationSpeed = message.ReadFloat();
#else
            iState.FirePressed = message.ReadBoolean();
            iState.WeaponNumber = message.ReadByte(2);
            iState.LookDirection = message.ReadRangedSingle(-MathHelper.TwoPi, MathHelper.TwoPi, 10);
            iState.MovementSpeed = (message.ReadUnitSingle(9) - 0.5f) * 2;
            iState.RotationSpeed = (message.ReadUnitSingle(10) - 0.5f) * 2;
            //PlayerPosition = new Vector2(message.ReadFloat(), message.ReadFloat());
#endif
            InputState = iState;
        }
        public override void Serialize(NetOutgoingMessage message)
        {
#if DBG_FULL_SERIALIZE
            message.Write(InputState.FirePressed);
            message.Write((byte)InputState.WeaponNumber, 7);
            message.Write(InputState.LookDirection);
            message.Write(InputState.MovementSpeed);
            message.Write(InputState.RotationSpeed);
#else
            message.Write(InputState.FirePressed);
            message.Write((byte)InputState.WeaponNumber, 2);
            message.WriteRangedSingle(InputState.LookDirection, -MathHelper.TwoPi, MathHelper.TwoPi, 10);
            message.WriteUnitSingle((InputState.MovementSpeed + 1f) / 2f, 9);
            message.WriteUnitSingle((InputState.RotationSpeed + 1f) / 2f, 10);
#endif
            //message.Write(PlayerPosition.X);
            //message.Write(PlayerPosition.Y);
        }
    }
}
