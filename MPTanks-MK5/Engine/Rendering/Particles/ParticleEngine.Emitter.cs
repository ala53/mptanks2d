using MPTanks.Engine.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Engine.Helpers;

namespace MPTanks.Engine.Rendering.Particles
{
    public partial class ParticleEngine
    {
        /// <summary>
        /// Creates a particle emitter instance.
        /// </summary>
        /// <param name="fuzziness">The amount of randomness to apply, between 0 and 1.</param>
        /// <param name="spriteInfo">The info for the sprite to render on the particles.</param>
        /// <param name="colorMask">The color mask to apply to the texture. E.g. a white texture with a blue mask will be blue.</param>
        /// <param name="emissionArea">The rectangle of the emission area.</param>
        /// <param name="size"></param>
        /// <param name="calculateVelocityAndAccelRelativeToRotation"></param>
        /// <param name="fadeInTime"></param>
        /// <param name="fadeOutTime"></param>
        /// <param name="lifeSpan"></param>
        /// <param name="velocity"></param>
        /// <param name="acceleration"></param>
        /// <param name="rotation"></param>
        /// <param name="rotationVelocity"></param>
        /// <param name="particlesPerSecond"></param>
        /// <param name="emitterLifespan"></param>
        /// <param name="shrinkInsteadOfFadeOut"></param>
        /// <param name="scaleUniform"></param>
        /// <param name="renderBelowObjects"></param>
        /// <param name="diedCallback"></param>
        /// <returns></returns>
        public Emitter CreateEmitter(float fuzziness, Assets.SpriteInfo spriteInfo,
            Color colorMask, RectangleF emissionArea, Vector2 size,
            bool calculateVelocityAndAccelRelativeToRotation = false,
            float fadeInTime = 0, float fadeOutTime = 0, float lifeSpan = 500,
            Vector2 velocity = default(Vector2), Vector2 acceleration = default(Vector2),
            Vector2 emitterVelocity = default(Vector2), float rotation = 0,
            float rotationVelocity = 0, float particlesPerSecond = 100, float emitterLifespan = 10000,
            bool growInsteadOfFadeIn = false, bool shrinkInsteadOfFadeOut = false,
            bool scaleUniform = false, bool renderBelowObjects = false,
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
                       particlesPerSecond * min, particlesPerSecond * max, emitterLifespan, 
                       growInsteadOfFadeIn, shrinkInsteadOfFadeOut,
                       scaleUniform, renderBelowObjects, emitterVelocity, diedCallback
                  );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fuzziness"></param>
        /// <param name="spriteInfos"></param>
        /// <param name="colorMask"></param>
        /// <param name="emissionArea"></param>
        /// <param name="size"></param>
        /// <param name="calculateVelocityAndAccelRelativeToRotation"></param>
        /// <param name="fadeInTime"></param>
        /// <param name="fadeOutTime"></param>
        /// <param name="lifeSpan"></param>
        /// <param name="velocity"></param>
        /// <param name="acceleration"></param>
        /// <param name="rotation"></param>
        /// <param name="rotationVelocity"></param>
        /// <param name="particlesPerSecond"></param>
        /// <param name="emitterLifespan"></param>
        /// <param name="shrinkInsteadOfFadeOut"></param>
        /// <param name="scaleUniform"></param>
        /// <param name="renderBelowObjects"></param>
        /// <param name="diedCallback"></param>
        /// <returns></returns>
        public Emitter CreateEmitter(float fuzziness, Assets.SpriteInfo[] spriteInfos,
            Color colorMask, RectangleF emissionArea, Vector2 size,
            bool calculateVelocityAndAccelRelativeToRotation = false,
            float fadeInTime = 0, float fadeOutTime = 0, float lifeSpan = 500,
            Vector2 velocity = default(Vector2), Vector2 acceleration = default(Vector2),
            Vector2 emitterVelocity = default(Vector2), float rotation = 0,
            float rotationVelocity = 0, float particlesPerSecond = 100, float emitterLifespan = 10000,
            bool growInsteadOfFadeIn = false, bool shrinkInsteadOfFadeOut = false, 
            bool scaleUniform = false, bool renderBelowObjects = false,
            Action<Emitter> diedCallback = null)
        {
            //fuzziness / 2 is used because fuzziness should be both directions
            //e.g. fuzziness = 1 should be +- 50%, not +-100%
            var min = 1 - fuzziness / 2;
            var max = 1 + fuzziness / 2;

            return CreateEmitter(spriteInfos,
                       fadeInTime * min, fadeInTime * max, fadeOutTime * min, fadeOutTime * max,
                       lifeSpan * min, lifeSpan * max, emissionArea, velocity * min,
                       velocity * max, acceleration * min, acceleration * max,
                       calculateVelocityAndAccelRelativeToRotation, size * min, size * max,
                       new Color(new Color(colorMask.ToVector3() * min), colorMask.A),
                       new Color(new Color(colorMask.ToVector3() * max), colorMask.A),
                       rotation * min, rotation * max, rotationVelocity * min, rotationVelocity * max,
                       particlesPerSecond * min, particlesPerSecond * max, emitterLifespan, 
                       growInsteadOfFadeIn, shrinkInsteadOfFadeOut,
                       scaleUniform, renderBelowObjects, emitterVelocity, diedCallback
                  );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteInfos"></param>
        /// <param name="minFadeInTime"></param>
        /// <param name="maxFadeInTime"></param>
        /// <param name="minFadeOutTime"></param>
        /// <param name="maxFadeOutTime"></param>
        /// <param name="minLifeSpan"></param>
        /// <param name="maxLifeSpan"></param>
        /// <param name="emissionArea"></param>
        /// <param name="minVelocity"></param>
        /// <param name="maxVelocity"></param>
        /// <param name="minAcceleration"></param>
        /// <param name="maxAcceleration"></param>
        /// <param name="calculateVelocityAndAccelRelativeToRotation"></param>
        /// <param name="minSize"></param>
        /// <param name="maxSize"></param>
        /// <param name="minColorMask"></param>
        /// <param name="maxColorMask"></param>
        /// <param name="minRotation"></param>
        /// <param name="maxRotation"></param>
        /// <param name="minRotationVelocity"></param>
        /// <param name="maxRotationVelocity"></param>
        /// <param name="minParticlesPerSecond"></param>
        /// <param name="maxParticlesPerSecond"></param>
        /// <param name="emitterLifespan"></param>
        /// <param name="shrinkInsteadOfFadeOut"></param>
        /// <param name="scaleUniform"></param>
        /// <param name="renderBelowObjects"></param>
        /// <param name="diedCallback"></param>
        /// <returns></returns>
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
            float emitterLifespan, 
            bool growInsteadOfFadeIn, bool shrinkInsteadOfFadeOut,
            bool scaleUniform, bool renderBelowObjects,
            Vector2 emitterVelocity,
            Action<Emitter> diedCallback = null)
        {
            var em = new Emitter(this);
            em.Sprites = spriteInfos;
            em.MinFadeInMs = minFadeInTime;
            em.MaxFadeInMs = maxFadeInTime;
            em.MinFadeOutMs = minFadeOutTime;
            em.MaxFadeOutMs = maxFadeOutTime;
            em.GrowInsteadOfFadeIn = growInsteadOfFadeIn;
            em.ShrinkInsteadOfFadeOut = shrinkInsteadOfFadeOut;
            em.ScaleUniform = scaleUniform;
            em.RenderBelowObjects = renderBelowObjects;
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
            em.EmitterVelocity = emitterVelocity;
            em.RemovedCallback = diedCallback;

            AddEmitter(em);
            return em;
        }

        private List<Emitter> _removeList = new List<Emitter>();
        private List<Emitter> _addList = new List<Emitter>();
        private void RemoveEmitter(Emitter emitter)
        {
            if (_updating)
                lock (_removeList)
                    _removeList.Add(emitter);
            else
            {
                if (emitter.RemovedCallback != null)
                    emitter.RemovedCallback(emitter);
                lock (_emitters)
                    _emitters.Remove(emitter);
            }
        }
        private void AddEmitter(Emitter emitter)
        {
            if (_updating)
                lock (_addList)
                    _addList.Add(emitter);
            else
                lock (_emitters)
                    _emitters.Add(emitter);
        }

        private void ProcessEmitterQueue()
        {
            lock (_addList)
                foreach (var em in _addList)
                    lock (_emitters)
                        _emitters.Add(em);
            lock (_removeList)
                foreach (var em in _removeList)
                {
                    if (em.RemovedCallback != null)
                        em.RemovedCallback(em);
                    lock (_emitters)
                        _emitters.Remove(em);
                }

            lock (_addList)
                _addList.Clear();
            lock (_removeList)
                _removeList.Clear();
        }

        private volatile bool _updating = false;
        private double _deficitMs = 0;
        private GameTime _internalGameTime = new GameTime();

        private void ProcessEmitters(GameTime gameTime)
        {
            //Track the deficit in milliseconds so we can do multiple steps for 1 frame if necessary
            _deficitMs += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_deficitMs <= 0) return; //if we are over on frame time, just do nothing
            //And then do the update loop
            _updating = true;

            //Keep track of the "totalGameTime"
            var totalTime = gameTime.TotalGameTime - TimeSpan.FromMilliseconds(_deficitMs);

            while (_deficitMs > 0) //otherwise, loop and do all the ticks
            {
                double tickMs = _deficitMs;
                if (tickMs >= Game.Settings.ParticleEmitterMaxDeltaTime) //handle a disproportionately large tick time
                    tickMs = Game.Settings.ParticleEmitterMaxDeltaTime; //and do multiple steps if necessary
                //Set the internal gametime instance so we can pass the correct tick time
                _internalGameTime.ElapsedGameTime = TimeSpan.FromMilliseconds(tickMs);
                _internalGameTime.TotalGameTime = totalTime;
                //And do the tick 
                ProcessEmittersTicked(_internalGameTime);
                //And subtract the tick time so we have an accurate clock
                _deficitMs -= tickMs;
                //And the total time
                totalTime += TimeSpan.FromMilliseconds(tickMs);
            }

            _updating = false;
            //And process the remove/add queue 
            ProcessEmitterQueue();
        }

        private void ProcessEmittersTicked(GameTime gameTime)
        {
            for (var i = 0; i < _emitters.Count; i++)
            {
                lock (_emitters)
                    _emitters[i].Update(gameTime);
            }
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
            private float _minPPS;
            public float MinParticlesPerSecond
            {
                get { return _minPPS; }
                set
                {
                    _minPPS = value;
                    ActualParticlesPerSecond = GetRandomBetween(MinParticlesPerSecond, MaxParticlesPerSecond);
                }
            }
            private float _maxPPS;
            public float MaxParticlesPerSecond
            {
                get { return _maxPPS; }
                set
                {
                    _maxPPS = value;
                    ActualParticlesPerSecond = GetRandomBetween(MinParticlesPerSecond, MaxParticlesPerSecond);
                }
            }
            public float ActualParticlesPerSecond { get; set; }
            public Color MinColorMask { get; set; }
            public Color MaxColorMask { get; set; }
            public Vector2 MinVelocity { get; set; }
            public Vector2 MaxVelocity { get; set; }
            public Vector2 MinAcceleration { get; set; }
            public Vector2 MaxAcceleration { get; set; }
            public bool CalculateVelocityAndAccelerationRelativeToRotation { get; set; }
            public bool GrowInsteadOfFadeIn { get; set; }
            public bool ShrinkInsteadOfFadeOut { get; set; }
            public bool ScaleUniform { get; set; }
            public bool RenderBelowObjects { get; set; }
            public Vector2 MinSize { get; set; }
            public Vector2 MaxSize { get; set; }
            public RectangleF EmissionArea { get; set; }
            public float EmitterLifespanMs { get; set; }
            public Action<Emitter> RemovedCallback { get; set; }
            public ParticleEngine Container { get; private set; }

            public Vector2 EmitterVelocity { get; set; }


            public bool Paused { get; set; }

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

                ActualParticlesPerSecond = GetRandomBetween(MinParticlesPerSecond, MaxParticlesPerSecond);
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
                if (Paused) return;

                //Update the total time
                totalTimeAlive += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                //Handle being dead
                if (totalTimeAlive > EmitterLifespanMs || !Alive)
                {
                    Kill();
                    return;
                }

                //Cache the initial value so we can do computations and get the exact position
                var currentEmissionArea = EmissionArea;

                //And move the emitter to its starting position
                var emitterDelta = EmitterVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //Move the emitter based on the velocity
                EmissionArea = new RectangleF(EmissionArea.X + emitterDelta.X, EmissionArea.Y + emitterDelta.Y,
                    EmissionArea.Width, EmissionArea.Height);

                //Keep track of how many particles we can create
                int numberOfParticlesToCreate = GetNumberOfParticlesToCreate();
                if (numberOfParticlesToCreate <= 0)
                    return; //No particles to make, nothing to do

                //Spawn the particles
                for (var i = 0; i < numberOfParticlesToCreate; i++)
                {
                    //Get one of the allowed assets (randomly)
                    var asset = BasicHelpers.ChooseRandom(Sprites);
                    var fadeOutTime = GetRandomBetween(MinFadeOutMs, MaxFadeOutMs);
                    var fadeInTime = GetRandomBetween(MinFadeInMs, MaxFadeInMs);

                    Vector2 velocity = GetRandomBetween(MinVelocity, MaxVelocity);
                    Vector2 acceleration = GetRandomBetween(MinAcceleration, MaxAcceleration);
                    float rotation = GetRandomBetween(MinRotation, MaxRotation);
                    //Get the size and scale either uniformly or randomly
                    Vector2 size;
                    if (ScaleUniform)
                    {
                        var factor = GetRandomBetween(0, 1);
                        var distX = MaxSize.X - MinSize.X;
                        var distY = MaxSize.Y - MinSize.Y;
                        size = new Vector2(distX * factor + MinSize.X, distY * factor + MinSize.Y);
                    }
                    else
                    {
                        size = GetRandomBetween(MinSize, MaxSize);
                    }

                    //Do you really have to ask with that variable name?
                    if (CalculateVelocityAndAccelerationRelativeToRotation)
                    {
                        var rotationRel = new Vector2(
                            (float)Math.Sin(rotation), (float)-Math.Cos(rotation)
                            );
                        velocity *= rotationRel;
                        acceleration *= rotationRel;
                    }

                    //And create the actual particle
                    var particle = new Particle()
                    {
                        Acceleration = acceleration,
                        SpriteInfo = asset,
                        ColorMask = GetRandomBetween(MinColorMask, MaxColorMask),
                        FadeInMs = fadeInTime,
                        FadeOutMs = fadeOutTime,
                        LifespanMs = GetRandomBetween(MinLifespanMs, MaxLifespanMs) + fadeInTime + fadeOutTime,
                        Position = GetRandomPosition(currentEmissionArea),
                        Rotation = rotation,
                        RotationVelocity = GetRandomBetween(MinRotationVelocity, MaxRotationVelocity),
                        Size = size,
                        Velocity = velocity,
                        RenderBelowObjects = RenderBelowObjects,
                        GrowInsteadOfFadeIn = ShrinkInsteadOfFadeOut,
                        ShinkInsteadOfFadeOut = ShrinkInsteadOfFadeOut
                    };
                    //Move the emitter to the exact location it should be for this particle
                    var exactEmitterDelta = EmitterVelocity *
                        (((float)i / numberOfParticlesToCreate) * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    //Move the emitter based on the velocity
                    currentEmissionArea =
                        new RectangleF(EmissionArea.X + exactEmitterDelta.X, EmissionArea.Y + exactEmitterDelta.Y,
                        EmissionArea.Width, EmissionArea.Height);
                    //And add it to the list
                    Container.AddParticle(particle);
                    totalParticlesCreated++; //Note that we created another particle
                }

            }

            private int GetNumberOfParticlesToCreate()
            {
                //So, we do a whole-lifetime analysis so that we don't have issues with short frames
                //E.g. if we are supposed to release 5 particles / sec and each frame is 1/10 of a sec,
                //a naive implementation would never spawn a particles
                float totalParticlesToSpawn = ((totalTimeAlive / 1000) *
                    ActualParticlesPerSecond);

                return (int)(totalParticlesToSpawn - totalParticlesCreated);
            }
        }
    }
}
