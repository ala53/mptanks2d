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
    public struct PseudoFullObjectState
    {
        const float _velocityThreshold = 0.05f;
        const float _positionThreshold = 1;
        const float _rotationThreshold = (float)Math.PI / 180f;
        const float _restitutionThreshold = 2f / 255;
        const float _sizeThreshold = 0.01f;

        public ushort ObjectId { get; set; }
        public bool VelocityChanged { get; set; }
        public bool PositionChanged { get; set; }
        public bool RotationChanged { get; set; }
        public bool RotationVelocityChanged { get; set; }
        public bool RestitutionChanged { get; set; }
        public bool SizeChanged { get; set; }
        public bool IsSensorObject { get; set; }
        public bool WasDestroyed { get; set; }

        public HalfVector2 Velocity { get; set; }
        public Half Rotation { get; set; }
        public Half RotationVelocity { get; set; }
        public Vector2 Position { get; set; }
        public Half Restitution { get; set; }
        public HalfVector2 Size { get; set; }

        public PseudoFullObjectState(PseudoFullObjectState lastState, PseudoFullObjectState newState, bool destroyed = false)
        {
            ObjectId = newState.ObjectId;
            VelocityChanged = false;
            PositionChanged = false;
            RotationChanged = false;
            RotationVelocityChanged = false;
            RestitutionChanged = false;
            SizeChanged = false;
            IsSensorObject = newState.IsSensorObject;
            WasDestroyed = destroyed;

            Velocity = newState.Velocity;
            Rotation = newState.Rotation;
            RotationVelocity = newState.RotationVelocity;
            Position = newState.Position;
            Restitution = newState.Restitution;
            Size = newState.Size;

            //Check if velocity changed
            if (MathHelper.Distance(lastState.Velocity.ToVector2().X,
                newState.Velocity.ToVector2().X) > _velocityThreshold ||
                MathHelper.Distance(lastState.Velocity.ToVector2().Y,
                newState.Velocity.ToVector2().Y) > _velocityThreshold)
                VelocityChanged = true;


            //Check if rotation changed
            if (MathHelper.Distance(lastState.Rotation, newState.Rotation) > _rotationThreshold)
                RotationChanged = true;

            //Check if position changed
            if (MathHelper.Distance(lastState.RotationVelocity, newState.RotationVelocity) > _rotationThreshold)
                RotationVelocityChanged = true;

            //Check if position changed
            if (MathHelper.Distance(lastState.Position.X, newState.Position.X) > _positionThreshold ||
                MathHelper.Distance(lastState.Position.Y, newState.Position.Y) > _positionThreshold)
                PositionChanged = true;

            //Check if restitution changed
            if (MathHelper.Distance(lastState.Restitution, newState.Restitution) > _restitutionThreshold)
                RestitutionChanged = true;

            //Check if size changed
            if (MathHelper.Distance(lastState.Size.ToVector2().X, newState.Size.ToVector2().X) > _sizeThreshold ||
                MathHelper.Distance(lastState.Size.ToVector2().Y, newState.Size.ToVector2().Y) > _sizeThreshold)
                SizeChanged = true;
        }

        public PseudoFullObjectState(GameObject obj, bool destroyed = false)
        {

            ObjectId = obj.ObjectId;
            VelocityChanged = true;
            PositionChanged = true;
            RotationChanged = true;
            RotationVelocityChanged = true;
            RestitutionChanged = true;
            SizeChanged = true;
            IsSensorObject = obj.IsSensor;
            WasDestroyed = destroyed;

            Velocity = new HalfVector2(obj.LinearVelocity);
            Rotation = (Half)obj.Rotation;
            RotationVelocity = (Half)obj.AngularVelocity;
            Position = obj.Position;
            Restitution = (Half)obj.Restitution;
            Size = new HalfVector2(obj.Size);
        }

        public PseudoFullObjectState(PseudoFullObjectState lastState, GameObject obj, bool destroyed = false)
            : this(lastState, new PseudoFullObjectState(obj, destroyed))
        { }

        public PseudoFullObjectState(PseudoFullObjectState state, bool destroyed)
        {
            if (destroyed)
            {
                ObjectId = state.ObjectId;
                VelocityChanged = false;
                PositionChanged = false;
                RotationChanged = false;
                RotationVelocityChanged = false;
                RestitutionChanged = false;
                SizeChanged = false;
                IsSensorObject = false;
                WasDestroyed = true;
                Velocity = new HalfVector2();
                Rotation = 0;
                RotationVelocity = 0;
                Position = new Vector2();
                Restitution = 0;
                Size = new HalfVector2();
            }
            else
            {
                ObjectId = state.ObjectId;
                VelocityChanged = state.VelocityChanged;
                PositionChanged = state.PositionChanged;
                RotationChanged = state.RotationChanged;
                RotationVelocityChanged = state.RotationVelocityChanged;
                RestitutionChanged = state.RestitutionChanged;
                SizeChanged = state.SizeChanged;
                IsSensorObject = state.IsSensorObject;
                WasDestroyed = state.WasDestroyed;
                Velocity = state.Velocity;
                Rotation = state.Rotation;
                RotationVelocity = state.RotationVelocity;
                Position = state.Position;
                Restitution = state.Restitution;
                Size = state.Size;
            }
        }
    }
}
