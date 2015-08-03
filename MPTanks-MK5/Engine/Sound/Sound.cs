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
        private static int _currentId = int.MinValue;
        public int UniqueId { get; private set; }
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
        #endregion

        public virtual string AssetName { get; private set; }
        /// <summary>
        /// The data stored by the actual sound player on the client side.
        /// </summary>
        public virtual object PlayerData { get; set; }

        public virtual int LoopCount { get; set; }
        public virtual TimeSpan Time { get; set; }
        /// <summary>
        /// The timescale that the sound is played at (1, 2, 1/2, 1/4, 1/8, etc.)
        /// </summary>
        public virtual float Timescale { get; set; }

        public virtual Vector2 Position { get; set; }
        public virtual Vector2 Velocity { get; set; }

        public virtual float Volume { get; set; } = 1;

        public virtual Action<Sound> CompletionCallback { get; set; }
        public virtual SoundPositioning PositioningMode { get; set; }

        public virtual SoundEngine Engine { get; private set; }

        internal Sound(SoundEngine engine, string _asset)
        {
            UniqueId = _currentId++;
            AssetName = _asset;
            Engine = engine;
            Timescale = 1;
            LoopCount = 1;
        }

    }
}
