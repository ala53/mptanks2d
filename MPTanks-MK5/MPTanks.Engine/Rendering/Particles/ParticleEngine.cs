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
            //And particles
            ProcessParticles((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            //And clean the cache
            CleanNodeCache();
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

        private List<LRULinkedListNode> _nodePool = new List<LRULinkedListNode>();
        private struct LRULinkedListNode
        {
            public DateTime AddTime;
            public LinkedListNode<Particle> ParticleNode;
        }
        private void RemoveNode(LinkedListNode<Particle> node)
        {
            _nodePool.Add(new LRULinkedListNode()
            {
                ParticleNode = node,
                AddTime = DateTime.UtcNow
            });
            _particles.Remove(node);
        }
        private LinkedListNode<Particle> GetNode(Particle particle)
        {
            if (_nodePool.Count > 0)
            {
                var node = _nodePool.Last();
                _nodePool.RemoveAt(_nodePool.Count - 1);
                node.ParticleNode.Value = particle;
                return node.ParticleNode;
            }
            else
            {
                //Create a new node
                var node = new LinkedListNode<Particle>(particle);
                return node;
            }
        }

        private DateTime _lastNodeCleanup =
            DateTime.UtcNow;
        /// <summary>
        /// Cleans up the node cache removing
        /// the least recently used nodes.
        /// </summary>
        private void CleanNodeCache()
        {
            //The minimum interval in
            //milliseconds between 
            //linked list node gc's
            var minGCIntervalMs = 1000;
            //The minimum age of a LLN
            //to remove it at
            var minAgeToRemoveMs = 10000;
            //Check if we're above the min time to gc
            if (!((DateTime.UtcNow - _lastNodeCleanup).TotalMilliseconds > minGCIntervalMs))
                return;

            var countToRemove = 0;
            foreach (var node in _nodePool)
            {
                var msSinceAdded = (DateTime.UtcNow - node.AddTime).TotalMilliseconds;
                if (!(msSinceAdded < minAgeToRemoveMs))
                    break; //Because of the way items are added, we can
                //           we can guarantee that no later items will be removed
                countToRemove++;
            }
            //If there are nodes to remove:
            if (countToRemove > 0)
            {
                //remove them
                _nodePool.RemoveRange(0, countToRemove);
                //resize the thing for performance
                if (_nodePool.Capacity > (_nodePool.Count * 2))
                    _nodePool.Capacity = _nodePool.Count * 2;
                //and the emitter pool
                if (_emitters.Capacity > (_emitters.Count * 2))
                    _emitters.Capacity = _emitters.Count * 2;
            }

            _lastNodeCleanup = DateTime.UtcNow;
        }
    }
}
