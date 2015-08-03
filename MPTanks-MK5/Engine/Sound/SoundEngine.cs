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
            float beginningOffset, Sound.SoundPositioning positioning,
            Vector2 position = default(Vector2), int loopCount = 0, object tag = null)
        {
            var _sound = new Sound(this, soundName)
            {
                PlayerData = tag,
                Position = position,
                PositioningMode = positioning,
                Time = TimeSpan.FromMilliseconds(beginningOffset),
                LoopCount = loopCount,
            };
            _sounds.AddLast(_sound);
            return _sound;
        }
        public Sound PlaySound(string soundName, Sound.SoundPositioning positioning = Sound.SoundPositioning.Static,
            Vector2 position = default(Vector2), int loopCount = 0, object tag = null)
        {
            return PlaySound(soundName, 0, positioning, position, loopCount);
        }

        /// <summary>
        /// Allows the client to mark a sound as completed and have it removed from existence
        /// </summary>
        /// <param name="sound"></param>
        public void MarkSoundCompleted(Sound sound)
        {
            if (sound.CompletionCallback != null)
                sound.CompletionCallback(sound);

            _sounds.Remove(sound);
        }
    }
}
