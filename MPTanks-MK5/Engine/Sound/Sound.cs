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

        public string AssetName { get; private set; }

        public int LoopCount { get; set; }
        private TimeSpan _time;
        private bool _timeDirty;
        public bool TimeDirty
        {
            get
            {
                var dirty = _timeDirty;
                _timeDirty = false;
                return dirty;
            }
        }
        public TimeSpan Time
        {
            get { return _time; }
            set { _time = value; _timeDirty = true; }
        }

        public bool Playing { get; set; }

        public float Pitch { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public float Volume { get; set; } = 1;

        public Action<Sound> CompletionCallback { get; set; }
        public bool Positional { get; set; }

        public SoundEngine Engine { get; private set; }

        internal Sound(SoundEngine engine, string _asset)
        {
            UniqueId = _currentId++;
            AssetName = _asset;
            Engine = engine;
            LoopCount = 0;
        }
        /// <summary>
        /// Do not use!
        /// </summary>
        /// <param name="time"></param>
        public void UnsafeSetTime(TimeSpan time)
        {
            _time = time;
        }
    }
}
