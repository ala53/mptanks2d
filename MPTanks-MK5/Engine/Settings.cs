using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine
{
    public class Settings
    {
        /// <summary>
        /// The physics engine runs at 1/10 scale for some idiotic reason.
        /// Basically, that means that a 3x5 tank is 0.3x0.5 blocks because
        /// Box2D a.k.a. Farseer works best with objects between 0.1 and 10 units
        /// in size.
        /// </summary>
        public float PhysicsScale = 0.1f;

        /// <summary>
        /// A list of mod files (relative or absolute) that should be loaded without 
        /// Code security verification. AKA trusted mods.
        /// </summary>
        public static readonly string[] TrustedMods = {
            "MPTanks.Modding.Mods.Core.dll"
            };

        /// <summary>
        /// The scale the rendering runs at relative to the blocks.
        /// This way, we can pass integers around safely.
        /// </summary>
        public float RenderScale = 100f;

        public float RenderLineThickness = 5;

        /// <summary>
        /// The amount of "blocks" to compensate for in rendering because of the 
        /// skin on physics objects
        /// </summary>
        public float PhysicsCompensationForRendering = 0.085f;

        public float TankDensity = 15;

        /// <summary>
        /// Number of milliseconds after GameCore initialization to wait before starting
        /// the game because we want all of the setup to be done and for people to be connected.
        /// </summary>
        public float TimeToWaitBeforeStartingGame = 5000;

        /// <summary>
        /// The number of milliseconds after game to continue updating
        /// </summary>
        public float TimePostGameToContinueRunning = 5000;

        /// <summary>
        /// The maximum number of particles to allow in game
        /// </summary>
        public int ParticleLimit = 100000;
    }
}
