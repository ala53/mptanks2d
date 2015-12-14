using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Settings
{
    public class EngineSettings : SettingsBase
    {
        public static EngineSettings GetInstance() => new EngineSettings("enginesettings.json");

        /// <summary>
        /// The physics engine runs best at 1/10 scale for some idiotic reason.
        /// Basically, that means that a 3x5 tank is 0.3x0.5 blocks because
        /// Box2D a.k.a. Farseer works best with objects between 0.1 and 10 units
        /// in size.
        /// </summary>
        public Setting<float> PhysicsScale { get; private set; }

        public Setting<float> TankDensity { get; private set; }

        /// <summary>
        /// The number of milliseconds after game to continue updating
        /// </summary>
        public Setting<TimeSpan> TimePostGameToContinueRunning { get; private set; }

        /// <summary>
        /// The maximum number of particles to allow in game
        /// </summary>
        public Setting<int> ParticleLimit { get; private set; }


        /// <summary>
        /// The minimum delta time in milliseconds for a game tick. If DT is less than this,
        /// we do one tick, then skip the next one, etc. until we have a deficit of game time,
        /// then do another and repeat.
        /// </summary>
        public Setting<TimeSpan> MinDeltaTimeGameTick { get; private set; }

        /// <summary>
        /// The maximum delta time for a game tick. If the time is greater, multiple steps will be taken.
        /// </summary>
        public Setting<TimeSpan> MaxDeltaTimeGameTick { get; private set; }

        /// <summary>
        /// The maximum delta time in milliseconds that a tick of the particle emitter can be.
        /// If DT is greater than this, multiple steps will be taken rather than a single one.
        /// </summary>
        public Setting<TimeSpan> ParticleEmitterMaxDeltaTime { get; private set; }

        public Setting<int> MaxStateChangeSize { get; private set; }

        public Setting<TimeSpan> MaxStateChangeFrequency { get; private set; }

        public Setting<TimeSpan> HotJoinTankSelectionTime { get; private set; }

        public EngineSettings(string file) : base(file)
        {
        }

        public EngineSettings()
        {
        }

        protected override void SetDefaults()
        {
            PhysicsScale = Setting.Hidden<float>(this, "Physics Scale")
            .SetDescription("The scale of the physics engine relative to game world space")
            .SetDefault(0.1f);

            TankDensity = Setting.Hidden<float>(this, "Tank density")
            .SetDescription("The density of a tank in the physics engine.")
            .SetDefault(15f);

            TimePostGameToContinueRunning = Setting.Time(this, "Post game time")
            .SetDescription("The amount of time to keep the game running after the winner has been determined." +
            " Sort of a post round deathmatch phase and a phase that helps people read the outcome" +
            " of the game.")
            .SetDefault(TimeSpan.FromMilliseconds(5000));

            ParticleLimit = Setting.Int(this, "Particle limit")
            .SetDescription("The maximum number of particles to allow in a game. The default is 20,000 for good reason." +
            " If it's more, there is a quite a chance of lag (even here, it happens on occasion)." +
            " If you set it past 100,000, you deserve to die a slow and painful lag death and you probably will.")
            .SetDefault(20000);

            MinDeltaTimeGameTick = Setting.Hidden<TimeSpan>(this, "Minimum Game Tick Time")
            .SetDescription("The smallest amount of time allowed for a single game tick." +
            " This probably should not be less than 1/3 of a millisecond or we run into numeric" +
            " precision issues in the physics engine.")
            .SetDefault(TimeSpan.FromMilliseconds(0.5));

            MaxDeltaTimeGameTick = Setting.Hidden<TimeSpan>(this, "Maximum Game Tick Time")
            .SetDescription("The maximum amount of time allowed for a single game tick. If it's greater," +
            " multiple game ticks will be performed for the one tick. Unfortunately, while" +
            " this improves the stability of collisions, it can cause a 'spiral of death' if the" +
            " game tick loop is the bottleneck of the game.")
            .SetDefault(TimeSpan.FromMilliseconds(34));

            ParticleEmitterMaxDeltaTime = Setting.Hidden<TimeSpan>(this, "Particle Emitter max tick time")
            .SetDescription("The maximum amount of time each tick of the particle engine can be. Changes to this value" +
            " can stop some particle rendering bugs due to faulty velocity calculations. Higher values lower" +
            " CPU time needed while lower values increase particle trail accuracy.")
            .SetDefault(TimeSpan.FromMilliseconds(17));

            MaxStateChangeSize = Setting.Hidden<int>(this, "Maximum GameObject State Change Size")
            .SetDescription("The maximum size, in bytes, of a single state change for a GameObject.")
            .SetDefault(3072);

            MaxStateChangeFrequency = Setting.Hidden<TimeSpan>(this, "Max state change frequency")
            .SetDescription("The maximum frequency of state changes from objects in milliseconds. Basically, rate limiting.")
            .SetDefault(TimeSpan.FromMilliseconds(100));//10 of them per second

            HotJoinTankSelectionTime = Setting.Time(this, "Tank selection time for hot join players")
            .SetDescription("The amount of time players who join while in game are given to select their tanks (before being given a default tank)")
            .SetDefault(TimeSpan.FromMilliseconds(15000f));

        }
    }
}
