using Lidgren.Network;
using Microsoft.Xna.Framework;
using MPTanks.Engine;
using MPTanks.Engine.Tanks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class PlayerInputChangedAction : ActionBase
    {
        public ushort PlayerId { get; set; }
        public InputState InputState { get; set; }
        public PlayerInputChangedAction(NetIncomingMessage message) : base(message)
        {
            PlayerId = (ushort)message.ReadUInt32(GameCore.PlayerIdNumberOfBits);
            var iState = new InputState();
            iState.FirePressed = message.ReadBoolean();
            iState.WeaponNumber = message.ReadByte(7);
            iState.LookDirection = message.ReadFloat();
            iState.MovementSpeed = message.ReadFloat();
            iState.RotationSpeed = message.ReadFloat();
            InputState = iState;
        }
        public PlayerInputChangedAction(NetworkPlayer player, InputState state)
        {
            PlayerId = player.Id;
            InputState = state;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write((uint)PlayerId, GameCore.PlayerIdNumberOfBits);
            message.Write(InputState.FirePressed);
            message.Write((byte)InputState.WeaponNumber, 7);
            message.Write(InputState.LookDirection);
            message.Write(InputState.MovementSpeed);
            message.Write(InputState.RotationSpeed);
        }
    }
}
