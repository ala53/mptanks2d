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
        public const float Beginning = float.MaxValue;
        public const float PlayToEnd = float.MaxValue;

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

        public string AssetName { get; private set; }
        public object UserData { get; set; }

        public int TotalRepeatCount { get; private set; }
        public float PositionMs { get; private set; }
        /// <summary>
        /// The timescale that the sound is played at (1, 2, 1/2, 1/4, 1/8, etc.)
        /// </summary>
        public float Timescale { get; set; }
        /// <summary>
        /// Repeats the sound from the start, incrementing the repeat counter.
        /// </summary>
        public void Repeat()
        {
            TotalRepeatCount++;
            PositionMs = 0;
        }
    }
}
