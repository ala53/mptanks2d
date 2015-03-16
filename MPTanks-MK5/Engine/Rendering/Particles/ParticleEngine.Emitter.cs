using Engine.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Rendering.Particles
{
    public partial class ParticleEngine
    {
        public Emitter CreateEmitter(float fuzziness, Assets.SpriteInfo spriteInfo,
            Color colorMask, RectangleF emissionArea, Vector2 size,
            bool calculateVelocityAndAccelRelativeToRotation = false,
            float fadeInTime = 0, float fadeOutTime = 0, float lifeSpan = 500,
            Vector2 velocity = default(Vector2), Vector2 acceleration = default(Vector2),
            float rotation = 0,
            float rotationVelocity = 0, float particlesPerSecond = 100, float emitterLifespan = 10000,
            Action<Emitter> diedCallback = null)
        {
            //fuzziness / 2 is used because fuzziness should be both directions
            //e.g. fuzziness = 1 should be +- 50%, not +-100%
            var min = 1 - fuzziness / 2;
            var max = 1 + fuzziness / 2;

            return CreateEmitter(new[] { spriteInfo },
                       fadeInTime * min, fadeInTime * max, fadeOutTime * min, fadeOutTime * max,
                       lifeSpan * min, lifeSpan * max, emissionArea, velocity * min,
                       velocity * max, acceleration * min, acceleration * max,
                       calculateVelocityAndAccelRelativeToRotation, size * min, size * max,
                       new Color(new Color(colorMask.ToVector3() * min), colorMask.A),
                       new Color(new Color(colorMask.ToVector3() * max), colorMask.A),
                       rotation * min, rotation * max, rotationVelocity * min, rotationVelocity * max,
                       particlesPerSecond * min, particlesPerSecond * max, emitterLifespan, diedCallback
                  );
        }
        public Emitter CreateEmitter(Assets.SpriteInfo[] spriteInfos,
            float minFadeInTime, float maxFadeInTime,
            float minFadeOutTime, float maxFadeOutTime,
            float minLifeSpan, float maxLifeSpan,
            RectangleF emissionArea,
            Vector2 minVelocity, Vector2 maxVelocity,
            Vector2 minAcceleration, Vector2 maxAcceleration,
            bool calculateVelocityAndAccelRelativeToRotation,
            Vector2 minSize, Vector2 maxSize,
            Color minColorMask, Color maxColorMask,
            float minRotation, float maxRotation,
            float minRotationVelocity, float maxRotationVelocity,
            float minParticlesPerSecond, float maxParticlesPerSecond,
            float emitterLifespan, Action<Emitter> diedCallback = null)
        {
            var em = new Emitter(this);
            em.Sprites = spriteInfos;
            em.MinFadeInMs = minFadeInTime;
            em.MaxFadeInMs = maxFadeInTime;
            em.MinFadeOutMs = minFadeOutTime;
            em.MaxFadeOutMs = maxFadeOutTime;
            em.MinLifespanMs = minLifeSpan;
            em.MaxLifespanMs = maxLifeSpan;
            em.EmissionArea = emissionArea;
            em.MinVelocity = minVelocity;
            em.MaxVelocity = maxVelocity;
            em.MinAcceleration = minAcceleration;
            em.MaxAcceleration = maxAcceleration;
            em.CalculateVelocityAndAccelerationRelativeToRotation = calculateVelocityAndAccelRelativeToRotation;
            em.MinSize = minSize;
            em.MaxSize = maxSize;
            em.MinColorMask = minColorMask;
            em.MaxColorMask = maxColorMask;
            em.MinRotation = minRotation;
            em.MaxRotation = maxRotation;
            em.MinRotationVelocity = minRotationVelocity;
            em.MaxRotationVelocity = maxRotationVelocity;
            em.MinParticlesPerSecond = minParticlesPerSecond;
            em.MaxParticlesPerSecond = maxParticlesPerSecond;
            em.EmitterLifespanMs = emitterLifespan;
            em.RemovedCallback = diedCallback;

            AddEmitter(em);
            return em;
        }

        private List<Emitter> _removeList = new List<Emitter>();
        private List<Emitter> _addList = new List<Emitter>();
        private void RemoveEmitter(Emitter emitter)
        {
            if (_updating)
                _removeList.Add(emitter);
            else
            {
                if (emitter.RemovedCallback != null)
                    emitter.RemovedCallback(emitter);
                _emitters.Remove(emitter);
            }
        }
        private void AddEmitter(Emitter emitter)
        {
            if (emitter == null)
                return;
            if (_updating)
                _addList.Add(emitter);
            else
                _emitters.Add(emitter);
        }

        private void ProcessEmitterQueue()
        {
            foreach (var em in _addList)
                _emitters.Add(em);
            foreach (var em in _removeList)
            {
                if (em.RemovedCallback != null)
                    em.RemovedCallback(em);
                _emitters.Remove(em);
            }

            _addList.Clear();
            _removeList.Clear();
        }

        private volatile bool _updating = false;
        private void ProcessEmitters(GameTime gameTime)
        {
            _updating = true;
            for (var i = 0; i < _emitters.Count; i++)
            {
                _emitters[i].Update(gameTime);
            }
            _updating = false;
            //And process the remove/add queue 
            ProcessEmitterQueue();
        }

        /// <summary>
        /// An emitter that creates particles based
        /// </summary>
        public class Emitter
        {
            public bool Alive { get; private set; }
            public Assets.SpriteInfo[] Sprites { get; set; }
            public float MinFadeInMs { get; set; }
            public float MaxFadeInMs { get; set; }
            public float MinFadeOutMs { get; set; }
            public float MaxFadeOutMs { get; set; }
            public float MinLifespanMs { get; set; }
            public float MaxLifespanMs { get; set; }
            public float MinRotation { get; set; }
            public float MaxRotation { get; set; }
            public float MinRotationVelocity { get; set; }
            public float MaxRotationVelocity { get; set; }
            public float MinParticlesPerSecond { get; set; }
            public float MaxParticlesPerSecond { get; set; }
            public Color MinColorMask { get; set; }
            public Color MaxColorMask { get; set; }
            public Vector2 MinVelocity { get; set; }
            public Vector2 MaxVelocity { get; set; }
            public Vector2 MinAcceleration { get; set; }
            public Vector2 MaxAcceleration { get; set; }
            public bool CalculateVelocityAndAccelerationRelativeToRotation { get; set; }
            public Vector2 MinSize { get; set; }
            public Vector2 MaxSize { get; set; }
            public RectangleF EmissionArea { get; set; }
            public float EmitterLifespanMs { get; set; }
            public Action<Emitter> RemovedCallback { get; set; }
            public ParticleEngine Container { get; private set; }

            private float totalTimeAlive = 0;
            private int totalParticlesCreated = 0;

            /// <summary>
            /// PRIVATE DO NOT USE. Use <see cref="ParticleEngine.CreateEmitter">CreateEmitter</see> 
            /// from the particle class.
            /// </summary>
            /// <param name="engine">DO NOT USE.</param>
            internal Emitter(ParticleEngine engine)
            {
                Container = engine;
                Alive = true;
            }

            public void Kill()
            {
                Container.RemoveEmitter(this);
                Alive = false;
            }

            private Random _random = new Random();
            private float GetRandomBetween(float min, float max)
            {
                var rand = (float)_random.NextDouble();
                var dist = max - min;
                return rand * dist + min;
            }
            private Vector2 GetRandomBetween(Vector2 min, Vector2 max)
            {
                return new Vector2(
                    GetRandomBetween(min.X, max.X),
                    GetRandomBetween(min.Y, max.Y)
                    );
            }
            private Vector2 GetRandomPosition(RectangleF rect)
            {
                return new Vector2(
                    GetRandomBetween(rect.Left, rect.Right),
                    GetRandomBetween(rect.Top, rect.Bottom));
            }
            private Color GetRandomBetween(Color minColor, Color maxColor)
            {
                return Color.Lerp(minColor, maxColor, (float)_random.NextDouble());
            }

            public void Update(GameTime gameTime)
            {
                totalTimeAlive += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (totalTimeAlive > EmitterLifespanMs || !Alive)
                {
                    Kill();
                    return;
                }

                int numberOfParticlesToCreate = GetNumberOfParticlesToCreate();
                if (numberOfParticlesToCreate <= 0)
                    return; //No particles to make, nothing to do

                //Spawn the particles
                for (var i = 0; i < numberOfParticlesToCreate; i++)
                {
                    var asset = ChooseRandomAsset();
                    var fadeOutTime = GetRandomBetween(MinFadeOutMs, MaxFadeOutMs);
                    var fadeInTime = GetRandomBetween(MinFadeInMs, MaxFadeInMs);

                    Vector2 velocity = GetRandomBetween(MinVelocity, MaxVelocity);
                    Vector2 acceleration = GetRandomBetween(MinAcceleration, MaxAcceleration);
                    float rotation = GetRandomBetween(MinRotation, MaxRotation);

                    if (CalculateVelocityAndAccelerationRelativeToRotation)
                    {
                        var rotationRel = new Vector2(
                            (float)Math.Sin(rotation), (float)-Math.Cos(rotation)
                            );
                        velocity *= rotationRel;
                        acceleration *= rotationRel;
                    }

                    var particle = new Particle()
                    {
                        Acceleration = acceleration,
                        AssetName = asset.SpriteName,
                        SheetName = asset.SheetName,
                        ColorMask = GetRandomBetween(MinColorMask, MaxColorMask),
                        FadeInMs = fadeInTime,
                        FadeOutMs = fadeOutTime,
                        LifespanMs = GetRandomBetween(MinLifespanMs, MaxLifespanMs) + fadeInTime + fadeOutTime,
                        Position = GetRandomPosition(EmissionArea),
                        Rotation = rotation,
                        RotationVelocity = GetRandomBetween(MinRotationVelocity, MaxRotationVelocity),
                        Size = GetRandomBetween(MinSize, MaxSize),
                        Velocity = velocity,
                    };
                    Container.AddParticle(particle);
                    totalParticlesCreated++; //Note that we created another particle
                }
            }

            private Assets.SpriteInfo ChooseRandomAsset()
            {
                if (Sprites.Length == 1)
                    return Sprites[0];

                return Sprites[_random.Next(0, Sprites.Length - 1)];
            }

            private int GetNumberOfParticlesToCreate()
            {
                //So, we do a whole-lifetime analysis so that we don't have issues with short frames
                //E.g. if we are supposed to release 5 particles / sec and each frame is 1/10 of a sec,
                //a naive implementation would never spawn a particles
                float totalParticlesToSpawn = ((totalTimeAlive / 1000) *
                    GetRandomBetween(MinParticlesPerSecond, MaxParticlesPerSecond));

                return (int)(totalParticlesToSpawn - totalParticlesCreated);
            }
        }
    }
}
