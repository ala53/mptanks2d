using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Rendering.Particles
{
    /// <summary>
    /// A single in-engine particle, a 80 byte struct.
    /// </summary>
    public struct Particle
    {
        public float LifespanMs;
        public float FadeOutMs;
        public float FadeInMs;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public bool RenderBelowObjects;
        public Vector2 Position
        {
            get { return NonCenteredPosition - (Size / 2); }
            set { NonCenteredPosition = value + (Size / 2); }
        }
        public Vector2 NonCenteredPosition;
        public Vector2 Size;
        public string AssetName;
        public string SheetName;
        public Color ColorMask;
        public float Rotation;
        public float RotationVelocity;
        public bool ShinkInsteadOfFade;
        public Vector2 OriginalSize;
        /// <summary>
        /// Do not use. Private to particle engine.
        /// </summary>
        public float TotalTimeAlreadyAlive;
        /// <summary>
        /// Do not use. Private to particle engine.
        /// </summary>
        public float Alpha;
    }
}
