using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Rendering
{
    /// <summary>
    /// Null renderer, it just removes all animations, sounds, and particle emitters
    /// for performance reasons. Use for dedicated servers to avoid the overhead of animation.
    /// </summary>
    public class NullRenderer
    {
        public NullRenderer(GameCore game)
        {
            _game = game;
        }
        private GameCore _game;
        public void Update()
        {
            foreach (var anim in _game.AnimationEngine.Animations)
                _game.AnimationEngine.MarkAnimationCompleted(anim);

            foreach (var emitter in _game.ParticleEngine.Emitters)
                emitter.EmitterLifespanMs = 0;

            foreach (var sound in _game.SoundEngine.Sounds)
                _game.SoundEngine.MarkSoundCompleted(sound);
        }
    }
}
