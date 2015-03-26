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
        private List<LinkedListNode<Particle>> _nodePool = new List<LinkedListNode<Particle>>();
        private List<Emitter> _emitters = new List<Emitter>();
        public IList<Emitter> Emitters { get { return _emitters.AsReadOnly(); } }
        private LinkedList<Particle> _particles;
        public IEnumerable<Particle> Particles { get { return _particles; } }
        public GameCore Game { get; private set; }
        private int _addPosition = 0; //We track add position for wraparound when we go over
        public int LivingParticlesCount { get; private set; }
        public ParticleEngine(GameCore game)
        {
            Game = game;
            _particles = new LinkedList<Particle>();
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

            //Guard clause: remove the first if we're over the limit
            if (Game.Settings.ParticleLimit <= _particles.Count)
                RemoveNode(_particles.First);
            //And add the new node
            _particles.AddLast(GetNode(particle));
        }

        public void Update(GameTime gameTime)
        {
            //Update emitters
            ProcessEmitters(gameTime);

            ProcessParticles((float)gameTime.ElapsedGameTime.TotalMilliseconds);
        }
        private void ProcessParticles(float deltaMs)
        {
            int particlesCount = 0;
            var deltaScale = deltaMs / 1000; //Calculate the relative amount of a second this is
            var node = _particles.First;
            while (node != null)
            {
                var part = node.Value;
                part.TotalTimeAlreadyAlive += deltaMs;
                if (part.LifespanMs <= part.TotalTimeAlreadyAlive) //Kill dead particles
                {
                    var _node = node;
                    node = node.Next;
                    RemoveNode(_node); //Remove and release to cache
                    continue;
                }
                //But if they're alive:

                //Statistical tracking
                particlesCount++;
                //If the particle has started it's fade out...
                if (part.LifespanMs - part.FadeOutMs <= part.TotalTimeAlreadyAlive)
                {
                    var percentFadedOut =
                        ((part.LifespanMs - part.TotalTimeAlreadyAlive)) / part.FadeOutMs;
                    if (part.ShinkInsteadOfFade)
                    {
                        part.Size = part.OriginalSize * percentFadedOut;
                    }
                    else
                    {
                        //Fade out
                        part.ColorMask = new Color(part.ColorMask, part.Alpha * percentFadedOut);
                    }
                }
                //If the particle is still fading in
                if (part.FadeInMs > part.TotalTimeAlreadyAlive && part.FadeInMs > 0)
                {
                    var percentFadedIn = part.TotalTimeAlreadyAlive / part.FadeInMs;
                    part.ColorMask = new Color(part.ColorMask, part.Alpha * percentFadedIn);
                }

                part.NonCenteredPosition += (part.Velocity * deltaScale); //And move it in world space
                part.Rotation += (part.RotationVelocity * deltaScale); //And rotate it
                //And update it's velocity so the next iteration can have a different velocity
                part.Velocity += (part.Acceleration * deltaScale);

                node.Value = part;
                node = node.Next;
            }
            LivingParticlesCount = particlesCount;
        }

        private void RemoveNode(LinkedListNode<Particle> node)
        {
            _nodePool.Add(node);
            _particles.Remove(node);
        }
        private LinkedListNode<Particle> GetNode(Particle particle)
        {
            if (_nodePool.Count > 0)
            {
                var node = _nodePool.Last();
                _nodePool.RemoveAt(_nodePool.Count - 1);
                node.Value = particle;
                return node;
            }
            else
            {
                //Create a new node
                var node = new LinkedListNode<Particle>(particle);
                return node;
            }
        }
    }
}
