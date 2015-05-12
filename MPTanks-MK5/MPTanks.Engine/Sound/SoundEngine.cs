using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Sound
{
    public class SoundEngine
    {
        #region Events
        private event EventHandler<SoundStartedEvent> OnSoundStarted = delegate { };
        private event EventHandler<SoundChangedEvent> OnSoundChanged = delegate { };
        private event EventHandler<SoundStoppedEvent> OnSoundStopped = delegate { };

        private SoundStartedEvent _pooledStartedEvent = new SoundStartedEvent();
        private SoundChangedEvent _pooledChangedEvent = new SoundChangedEvent();
        private SoundStoppedEvent _pooledStoppedEvent = new SoundStoppedEvent();

        /// <summary>
        /// DO NOT USE. THIS IS PRIVATE.
        /// </summary>
        /// <remarks>
        /// Called to raise changed events from the Sound engine without dealing with memory leaks from
        /// events (storing strong references)
        /// </remarks>
        /// <param name="sound"></param>
        internal void RaiseChangedEventFromSound(Sound sound)
        {
            _pooledChangedEvent.Sound = sound;
            OnSoundChanged(sound, _pooledChangedEvent);
        }
        #endregion


        private LinkedList<Sound> _sounds = new LinkedList<Sound>();
        public IEnumerable<Sound> Sounds { get { return _sounds; } }
        private GameCore _game;

        public SoundEngine(GameCore game)
        {
            _game = game;
        }

        /// <summary>
        /// Plays a sound
        /// </summary>
        /// <param name="soundName"></param>
        /// <param name="beginningOffset"></param>
        /// <param name="endOffset"></param>
        /// <param name="positioning"></param>
        /// <param name="position"></param>
        /// <param name="repeat"></param>
        /// <returns></returns>
        public Sound PlaySound(string soundName,
            SoundOffset beginningOffset, SoundOffset endOffset, Sound.SoundPositioning positioning,
            Vector2 position = default(Vector2), Sound.SoundRepeat repeat = Sound.SoundRepeat.NoRepeat)
        {
            return null;
        }
        public Sound PlaySound(string soundName, Sound.SoundPositioning positioning = Sound.SoundPositioning.Static,
            Vector2 position = default(Vector2), Sound.SoundRepeat repeat = Sound.SoundRepeat.NoRepeat)
        {
            return PlaySound(soundName, SoundOffset.Beginning, SoundOffset.End, positioning, position, repeat);
        }

        /// <summary>
        /// Allows the player to mark a sound as completed and have it removed from existence
        /// </summary>
        /// <param name="sound"></param>
        public void MarkSoundCompleted(Sound sound)
        {

        }
    }

    public struct SoundOffset
    {
        public static readonly SoundOffset Beginning =
            new SoundOffset(0);
        public static readonly SoundOffset End =
            new SoundOffset(float.MaxValue);

        public readonly float Offset;
        public SoundOffset(float offset)
        {
            Offset = offset;
        }

        public static implicit operator SoundOffset(float ms)
        {
            return new SoundOffset(ms);
        }
        public static implicit operator float(SoundOffset offset)
        {
            return offset.Offset;
        }
    }
}
