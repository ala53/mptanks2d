using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Rendering.Particles
{
    /// <summary>
    /// A single in-engine particle, a 68 byte struct.
    /// </summary>
    public struct Particle
    {
        public float TotalTimeAlreadyAlive;
        public float LifespanMs;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Vector2 Position;
        public Vector2 Size;
        public string AssetName;
        public string SheetName;
        public Color ColorMask;
        public float Rotation;
        public float RotationVelocity;
        /// <summary>
        /// Do not use. Private to particle engine.
        /// </summary>
        public bool Alive;
        /// <summary>
        /// Do not use. Private to particle engine.
        /// </summary>
        public float Alpha;
    }
}
