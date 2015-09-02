#define DBG_FULL_SERIALIZE
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
#if DBG_FULL_SERIALIZE
            iState.FirePressed = message.ReadBoolean();
            iState.WeaponNumber = message.ReadByte(7);
            iState.LookDirection = message.ReadFloat();
            iState.MovementSpeed = message.ReadFloat();
            iState.RotationSpeed = message.ReadFloat();
#else
            iState.FirePressed = message.ReadBoolean();
            iState.WeaponNumber = message.ReadByte(2);
            iState.LookDirection = message.ReadRangedSingle(-MathHelper.TwoPi, MathHelper.TwoPi, 13);
            iState.MovementSpeed = (message.ReadUnitSingle(12) - 0.5f) * 2;
            iState.RotationSpeed = (message.ReadUnitSingle(12) - 0.5f) * 2;
            //PlayerPosition = new Vector2(message.ReadFloat(), message.ReadFloat());
#endif
            InputState = iState;
        }
        public PlayerInputChangedAction(NetworkPlayer player, InputState state)
        {
            PlayerId = player.Id;
            InputState = state;
        }

        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(PlayerId, GameCore.PlayerIdNumberOfBits);
#if DBG_FULL_SERIALIZE
            message.Write(InputState.FirePressed);
            message.Write((byte)InputState.WeaponNumber, 7);
            message.Write(InputState.LookDirection);
            message.Write(InputState.MovementSpeed);
            message.Write(InputState.RotationSpeed);
#else
            message.Write(InputState.FirePressed);
            message.Write((byte)InputState.WeaponNumber, 2);
            message.WriteRangedSingle(InputState.LookDirection, -MathHelper.TwoPi, MathHelper.TwoPi, 13);
            message.WriteUnitSingle((InputState.MovementSpeed + 1f) / 2f, 12);
            message.WriteUnitSingle((InputState.RotationSpeed + 1f) / 2f, 12);
#endif
        }
    }
}
