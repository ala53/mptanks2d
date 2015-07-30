using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Rendering.Particles
{
    public partial class ParticleEngine
    {
        private List<Emitter> _emitters = new List<Emitter>();
        public IList<Emitter> Emitters { get { return _emitters.AsReadOnly(); } }
        private Particle[] _particles;
        public Particle[] Particles { get { return _particles; } }
        public GameCore Game { get; private set; }
        private int _addPosition = 0;
        public int LivingParticlesCount { get; private set; }
        public ParticleEngine(GameCore game)
        {
            Game = game;
            _particles = new Particle[Game.Settings.ParticleLimit];
        }

        public void AddParticle(Particle particle)
        {
            //Compute the position with wraparound - if we're over the limit,
            //we remove the oldest particles first
            _addPosition = _addPosition % Game.Settings.ParticleLimit;

            //Sanity check
            if (particle.LifespanMs <= 0)
            {
                Game.Logger.Error("Particles cannot have a negative lifespan!");
                return;
            }
            //Ignore unset color masks
            if (particle.ColorMask == default(Color)) particle.ColorMask = Color.White;

            particle.TotalTimeAlreadyAlive = 0;
            particle.Alpha = particle.ColorMask.A / 255f;
            particle.OriginalSize = particle.Size;
            particle.Alive = true;

            _particles[_addPosition] = particle;
            _addPosition++;
        }

        public void Update(GameTime gameTime)
        {
            //Update emitters
            ProcessEmitters(gameTime);
            //And particles
            ProcessParticles((float)gameTime.ElapsedGameTime.TotalMilliseconds);
        }
        private void ProcessParticles(float deltaMs)
        {
            var deltaScale = deltaMs / 1000;
            int particlesCount = 0;
            for (int i = 0; i < _particles.Length; i++)
            {
                if (!_particles[i].Alive)
                    continue;

                _particles[i].TotalTimeAlreadyAlive += deltaMs;
                if (_particles[i].LifespanMs <= _particles[i].TotalTimeAlreadyAlive) //Kill dead particles
                {
                    _particles[i].Alive = false;
                    continue;
                }
                //But if they're alive:

                //Statistical tracking
                particlesCount++;
                //If the particle has started it's fade out...
                if (_particles[i].LifespanMs - _particles[i].FadeOutMs <= _particles[i].TotalTimeAlreadyAlive)
                {
                    var percentFadedOut =
                        ((_particles[i].LifespanMs - _particles[i].TotalTimeAlreadyAlive)) / _particles[i].FadeOutMs;
                    if (_particles[i].ShinkInsteadOfFadeOut)
                    {
                        _particles[i].Size = _particles[i].OriginalSize * percentFadedOut;
                    }
                    else
                    {
                        //Fade out
                        _particles[i].ColorMask = new Color(_particles[i].ColorMask, _particles[i].Alpha * percentFadedOut);
                    }
                }
                //If the particle is still fading in
                if (_particles[i].FadeInMs > _particles[i].TotalTimeAlreadyAlive && _particles[i].FadeInMs > 0)
                {
                    if (_particles[i].GrowInsteadOfFadeIn)
                    {
                        var percentFadedIn = _particles[i].TotalTimeAlreadyAlive / _particles[i].FadeInMs;
                        _particles[i].Size = _particles[i].OriginalSize * percentFadedIn;
                    }
                    else
                    {
                        var percentFadedIn = _particles[i].TotalTimeAlreadyAlive / _particles[i].FadeInMs;
                        _particles[i].ColorMask = new Color(_particles[i].ColorMask, _particles[i].Alpha * percentFadedIn);
                    }
                }

                _particles[i].NonCenteredPosition += (_particles[i].Velocity * deltaScale); //And move it in world space
                _particles[i].Rotation += (_particles[i].RotationVelocity * deltaScale); //And rotate it
                //And update it's velocity so the next iteration can have a different velocity
                _particles[i].Velocity += (_particles[i].Acceleration * deltaScale);

            }
            LivingParticlesCount = particlesCount;
        }
    }
}
