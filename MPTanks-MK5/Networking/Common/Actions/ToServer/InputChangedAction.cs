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
        public InputChangedAction(NetIncomingMessage message):base(message)
        {
            var iState = new InputState();
            iState.FirePressed = message.ReadBoolean();
            iState.WeaponNumber = message.ReadByte(7);
            iState.LookDirection = message.ReadFloat();
            iState.MovementSpeed = message.ReadFloat();
            iState.RotationSpeed = message.ReadFloat();
            InputState = iState;
        }

        public InputChangedAction(InputState state)
        {
            InputState = state;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(InputState.FirePressed);
            message.Write((byte)InputState.WeaponNumber, 7);
            message.Write(InputState.LookDirection);
            message.Write(InputState.MovementSpeed);
            message.Write(InputState.RotationSpeed);
        }
    }
}
