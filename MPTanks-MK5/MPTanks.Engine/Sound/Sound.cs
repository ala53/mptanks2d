using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Sound
{
    public class Sound
    {
        #region Constants
        public enum SoundPositioning
        {
            /// <summary>
            /// The sound is non-moving
            /// </summary>
            Static,
            /// <summary>
            /// The sound moves on along a path defined by the velocity vector
            /// </summary>
            Moving,
            /// <summary>
            /// Sound tracks the player
            /// </summary>
            NonPositional
        }

        public enum SoundRepeat
        {
            /// <summary>
            /// Loops indefinitely
            /// </summary>
            Loop,
            /// <summary>
            /// Repeats the sound once
            /// </summary>
            RepeatOnce,
            /// <summary>
            /// Repeats the sound twice
            /// </summary>
            RepeatTwice,
            /// <summary>
            /// Only play the sound once
            /// </summary>
            NoRepeat

        }
        #endregion

        public virtual string AssetName { get; private set; }
        /// <summary>
        /// The data stored by the actual sound player on the server side.
        /// </summary>
        public virtual object PlayerData { get; set; }

        public virtual int TotalRepeatCount { get; set; }
        public virtual float PositionMs { get; set; }
        /// <summary>
        /// The timescale that the sound is played at (1, 2, 1/2, 1/4, 1/8, etc.)
        /// </summary>
        public virtual float Timescale { get; set; }

        public virtual Vector2 Position { get; set; }
        public virtual Vector2 Velocity { get; set; }

        public virtual Action<Sound> CompletionCallback { get; set; }
        public virtual SoundPositioning PositioningMode { get; set; }

        public virtual SoundEngine Engine { get; private set; }

        internal Sound(SoundEngine engine, string _asset)
        {
            AssetName = _asset;
            Engine = engine;
            Timescale = 1;
            TotalRepeatCount = 1;
        }

    }
}
