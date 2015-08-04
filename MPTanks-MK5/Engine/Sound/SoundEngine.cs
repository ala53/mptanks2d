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
            float beginningOffset, bool positional,
            Vector2 position = default(Vector2), int loopCount = 0)
        {
            var _sound = new Sound(this, soundName)
            {
                Position = position,
                Positional = positional,
                Time = TimeSpan.FromMilliseconds(beginningOffset),
                LoopCount = loopCount,
            };
            _sounds.AddLast(_sound);
            return _sound;
        }
        public Sound PlaySound(string soundName, bool positional,
            Vector2 position = default(Vector2), int loopCount = 0)
        {
            return PlaySound(soundName, 0, positional, position, loopCount);
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
