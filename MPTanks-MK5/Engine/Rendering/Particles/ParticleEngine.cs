using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Rendering.Particles
{
    public class ParticleEngine
    {
        public Particle[] Particles { get; private set; }
        public GameCore Game { get; private set; }
        private int _addPosition = 0; //We track add position for wraparound when we go over
        public int LivingParticlesCount { get; private set; }
        public ParticleEngine(GameCore game)
        {
            Game = game;
            Particles = new Particle[Settings.ParticleLimit];
        }

        public void AddParticle(Particle particle)
        {
            //Compute the position with wraparound - if we're over the limit,
            //we remove the oldest particles first
            _addPosition = _addPosition % (Settings.ParticleLimit);

            //Sanity checks
            if (particle.LifespanMs <= 0)
            {
                Game.Logger.Error("Particles cannot have a negative lifespan!");
                return;
            }
            //Ignore unset color masks
            if (particle.ColorMask == default(Color)) particle.ColorMask = Color.White;

            particle.Alive = true;
            particle.Alpha = particle.ColorMask.A / 255f;
            //And overwrite it in the array
            Particles[_addPosition] = particle;

            //And increment the add counter
            _addPosition++;
        }

        public void Update(GameTime gameTime)
        {
            ProcessParticles((float)gameTime.ElapsedGameTime.TotalMilliseconds);
        }
        private void ProcessParticles(float deltaMs)
        {
            const float fadeOutMs = 75;
            LivingParticlesCount = 0;
            var deltaScale = deltaMs / 1000; //Calculate the relative amount of a second this is
            for (int i = 0; i < Particles.Length; i++)
            {
                var part = Particles[i]; //Get the particle from the array 
                if (!part.Alive) //Skip dead particles for performance
                    continue;

                //Statistical tracking
                LivingParticlesCount++;
                //Update the lifespan's time
                part.TotalTimeAlreadyAlive += deltaMs; //Increase the alive time for the particle
                //If the particle has outlived it's lifespan, we remove it
                if (part.LifespanMs + fadeOutMs <= part.TotalTimeAlreadyAlive)
                {
                    part.Alive = false; //Mark the particle as dead
                    Particles[i] = part; //And write the updates back to the engine
                    continue;
                }
                if (part.LifespanMs <= part.TotalTimeAlreadyAlive)
                {
                    var percentToFadeOut = (part.TotalTimeAlreadyAlive - part.LifespanMs) / fadeOutMs;
                    //Fade out
                    part.ColorMask = new Color(part.ColorMask, part.Alpha * percentToFadeOut);
                }

                part.Position += (part.Velocity * deltaScale); //And move it in world space
                part.Rotation += (part.RotationVelocity * deltaScale); //And rotate it
                //And update it's velocity so the next iteration can have a different velocity
                part.Velocity += (part.Acceleration * deltaScale);

                Particles[i] = part; //And write the updates back to the engine
            }
        }
    }
}
