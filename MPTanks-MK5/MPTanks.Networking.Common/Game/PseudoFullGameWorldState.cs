using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Game
{
    /// <summary>
    /// A pseudo-full state. It contains the physical information for all the game objects but not their 
    /// actual private state data, which is computationally expensive
    /// </summary>
    public class PseudoFullGameWorldState
    {
        public IReadOnlyDictionary<ushort, PseudoFullObjectState> ObjectStates { get { return _objectStates; } }
        private Dictionary<ushort, PseudoFullObjectState> _objectStates
            = new Dictionary<ushort, PseudoFullObjectState>();

        public GameCore.CurrentGameStatus CurrentGameStatus { get; private set; }
        public float CurrentGameTimeMilliseconds { get; private set; }
        public bool FriendlyFireEnabled { get; private set; }

        public static PseudoFullGameWorldState Create(GameCore game)
        {
            var state = new PseudoFullGameWorldState();
            foreach (var obj in game.GameObjects)
            {
                state._objectStates.Add(obj.ObjectId, new PseudoFullObjectState(obj));
            }

            state.CurrentGameStatus = game.GameStatus;
            state.CurrentGameTimeMilliseconds = game.TimeMilliseconds;
            state.FriendlyFireEnabled = game.FriendlyFireEnabled;

            return state;
        }

        public PseudoFullGameWorldState MakeDelta(PseudoFullGameWorldState lastState)
        {
            var state = new PseudoFullGameWorldState();

            foreach (var obj in lastState.ObjectStates.Values)
            {
                //It was destroyed, flag it
                if (!ObjectStates.ContainsKey(obj.ObjectId))
                    state._objectStates.Add(obj.ObjectId, new PseudoFullObjectState(obj, true));

                //Otherwise, compute state differences
                state._objectStates.Add(obj.ObjectId, new PseudoFullObjectState(obj, ObjectStates[obj.ObjectId]));
            }

            //Then do a reverse search to find the new ones
            foreach (var obj in ObjectStates.Values)
            {
                if (!lastState.ObjectStates.ContainsKey(obj.ObjectId) &&
                    !state.ObjectStates.ContainsKey(obj.ObjectId))
                {
                    state._objectStates.Add(obj.ObjectId, obj);
                }
            }

            state.CurrentGameStatus = CurrentGameStatus;
            state.CurrentGameTimeMilliseconds = CurrentGameTimeMilliseconds;
            state.FriendlyFireEnabled = FriendlyFireEnabled;

            return state;
        }

        public void Apply(GameCore game)
        {
            //Do it via reflection to keep api private
            var statusProp = typeof(GameCore).GetProperty(nameof(GameCore.GameStatus));
            statusProp.SetValue(game, CurrentGameStatus);

            //Do this with reflection because we want to keep the api private (set game time)
            var timeProp = typeof(GameCore).GetProperty(nameof(GameCore.TimeMilliseconds));
            timeProp.SetValue(game, CurrentGameTimeMilliseconds);

            game.FriendlyFireEnabled = FriendlyFireEnabled;

            foreach (var objState in ObjectStates.Values)
            {
                if (objState.WasDestroyed && game.GameObjectsById.ContainsKey(objState.ObjectId))
                {
                    game.RemoveGameObject(game.GameObjectsById[objState.ObjectId], null, true);
                    continue;
                }

                var obj = game.GameObjectsById[objState.ObjectId];

                obj.IsSensor = objState.IsSensorObject;
                obj.IsStatic = objState.IsStaticObject;

                if (objState.PositionChanged)
                    obj.Position = objState.Position;
                if (objState.RestitutionChanged)
                    obj.Restitution = objState.Restitution;
                if (objState.RotationChanged)
                    obj.Rotation = objState.Rotation;
                if (objState.RotationVelocityChanged)
                    obj.AngularVelocity = objState.RotationVelocity;
                if (objState.SizeChanged)
                    obj.Size = objState.Size.ToVector2();
                if (objState.VelocityChanged)
                    obj.LinearVelocity = objState.Velocity.ToVector2();
            }
        }

        public static PseudoFullGameWorldState Read(NetIncomingMessage message)
        {
            var state = new PseudoFullGameWorldState();

            state.CurrentGameStatus = (GameCore.CurrentGameStatus)message.ReadByte();
            state.CurrentGameTimeMilliseconds = message.ReadFloat();
            state.FriendlyFireEnabled = message.ReadBoolean();

            message.ReadPadBits();

            var ct = message.ReadUInt16();

            for (var i = 0; i < ct; i++)
            {
                var objState = new PseudoFullObjectState();
                objState.ObjectId = message.ReadUInt16();
                objState.VelocityChanged = message.ReadBoolean();
                objState.PositionChanged = message.ReadBoolean();
                objState.RotationChanged = message.ReadBoolean();
                objState.RotationVelocityChanged = message.ReadBoolean();
                objState.RestitutionChanged = message.ReadBoolean();
                objState.SizeChanged = message.ReadBoolean();
                objState.IsSensorObject = message.ReadBoolean();
                objState.IsStaticObject = message.ReadBoolean();
                objState.WasDestroyed = message.ReadBoolean();

                message.ReadPadBits();

                if (objState.VelocityChanged)
                    objState.Velocity = new HalfVector2 { PackedValue = message.ReadUInt32() };

                if (objState.PositionChanged)
                    objState.Position = new Vector2(message.ReadFloat(), message.ReadFloat());

                if (objState.RotationChanged)
                    objState.Rotation = new Half() { InternalValue = message.ReadUInt16() };

                if (objState.RotationVelocityChanged)
                    objState.RotationVelocity = new Half() { InternalValue = message.ReadUInt16() };

                if (objState.RestitutionChanged)
                    objState.Restitution = new Half() { InternalValue = message.ReadUInt16() };

                if (objState.SizeChanged)
                    objState.Size = new HalfVector2() { PackedValue = message.ReadUInt32() };
            }

            return state;
        }

        public void Write(NetOutgoingMessage message)
        {
            //header
            message.Write((byte)CurrentGameStatus);
            message.Write(CurrentGameTimeMilliseconds);
            message.Write(FriendlyFireEnabled);
            message.WritePadBits();

            //number of objects
            message.Write((ushort)ObjectStates.Count);

            //and loop for objects
            foreach (var obj in ObjectStates.Values)
            {
                message.Write(obj.ObjectId);

                message.Write(obj.VelocityChanged);
                message.Write(obj.PositionChanged);
                message.Write(obj.RotationChanged);
                message.Write(obj.RotationVelocityChanged);
                message.Write(obj.RestitutionChanged);
                message.Write(obj.SizeChanged);
                message.Write(obj.IsSensorObject);
                message.Write(obj.IsStaticObject);
                message.Write(obj.WasDestroyed);

                message.WritePadBits();

                if (obj.VelocityChanged)
                    message.Write(obj.Velocity.PackedValue);

                if (obj.PositionChanged)
                {
                    message.Write(obj.Position.X);
                    message.Write(obj.Position.Y);
                }

                if (obj.RotationChanged)
                    message.Write(obj.Rotation);

                if (obj.RotationVelocityChanged)
                    message.Write(obj.RotationVelocity);

                if (obj.RestitutionChanged)
                    message.Write(obj.Restitution.InternalValue);

                if (obj.SizeChanged)
                    message.Write(obj.Size.PackedValue);
            }
        }
    }
}
