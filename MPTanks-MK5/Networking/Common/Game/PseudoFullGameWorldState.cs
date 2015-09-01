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
        public double CurrentGameTimeMilliseconds { get; private set; }
        public bool FriendlyFireEnabled { get; private set; }

        public static PseudoFullGameWorldState Create(GameCore game)
        {
            var state = new PseudoFullGameWorldState();
            foreach (var obj in game.GameObjects)
            {
                state._objectStates.Add(obj.ObjectId, new PseudoFullObjectState(obj));
            }

            state.CurrentGameStatus = game.Status;
            state.CurrentGameTimeMilliseconds = game.Time.TotalMilliseconds;
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
                else
                {
                    var objState = new PseudoFullObjectState(obj, ObjectStates[obj.ObjectId]);
                    if (objState.HasChanges(obj))
                        state._objectStates.Add(obj.ObjectId, objState);
                }
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

        public void Apply(GameCore game, float latency = 0, ushort? tankId = null)
        {
            //Do it via reflection to keep api private
            var statusProp = typeof(GameCore).GetProperty(nameof(GameCore.Status));
            statusProp.SetValue(game, CurrentGameStatus);

            //Do this with reflection because we want to keep the api private (set game time)
            var timeProp = typeof(GameCore).GetProperty(nameof(GameCore.Time));
            timeProp.SetValue(game, TimeSpan.FromMilliseconds(CurrentGameTimeMilliseconds));

            game.FriendlyFireEnabled = FriendlyFireEnabled;

            foreach (var objState in ObjectStates.Values)
            {
                if (game.GameObjectsById.ContainsKey(objState.ObjectId))
                {
                    if (objState.WasDestroyed && game.GameObjectsById.ContainsKey(objState.ObjectId))
                    {
                        game.ImmediatelyForceObjectDestruction(game.GameObjectsById[objState.ObjectId]);
                        continue;
                    }
                    var obj = game.GameObjectsById[objState.ObjectId];

                    obj.IsSensor = objState.IsSensorObject;
                    obj.IsStatic = objState.IsStaticObject;

                    if (objState.VelocityChanged)
                        obj.LinearVelocity = objState.Velocity.ToVector2();
                    if (objState.PositionChanged)
                    {
                        //if (tankId != null && objState.ObjectId == tankId)
                        //{
                        //    if (Vector2.Distance(objState.Position, obj.Position) > 3f)
                        //        obj.Position = objState.Position + (obj.LinearVelocity * latency);
                        //}
                        //else
                            obj.Position = objState.Position + (obj.LinearVelocity * latency);
                    }
                    if (objState.RestitutionChanged)
                        obj.Restitution = objState.Restitution;
                    if (objState.RotationVelocityChanged)
                        obj.AngularVelocity = objState.RotationVelocity;
                    if (objState.RotationChanged)
                        obj.Rotation = objState.Rotation + (obj.AngularVelocity * latency);
                    if (objState.SizeChanged)
                        obj.Size = objState.Size.ToVector2();
                }
            }
        }

        public static PseudoFullGameWorldState Read(NetIncomingMessage message)
        {
            var state = new PseudoFullGameWorldState();

            state.CurrentGameStatus = (GameCore.CurrentGameStatus)message.ReadByte();
            state.CurrentGameTimeMilliseconds = message.ReadDouble();
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

                state._objectStates.Add(objState.ObjectId, objState);
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
                    message.Write(obj.Rotation.InternalValue);

                if (obj.RotationVelocityChanged)
                    message.Write(obj.RotationVelocity.InternalValue);

                if (obj.RestitutionChanged)
                    message.Write(obj.Restitution.InternalValue);

                if (obj.SizeChanged)
                    message.Write(obj.Size.PackedValue);
            }
        }
    }
}
