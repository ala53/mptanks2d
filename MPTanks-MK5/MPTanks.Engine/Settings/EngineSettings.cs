using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Settings
{
    public class EngineSettings : SettingsBase
    {

        /// <summary>
        /// The physics engine runs at 1/10 scale for some idiotic reason.
        /// Basically, that means that a 3x5 tank is 0.3x0.5 blocks because
        /// Box2D a.k.a. Farseer works best with objects between 0.1 and 10 units
        /// in size.
        /// </summary>
        public Setting<float> PhysicsScale { get; private set; }

        public Setting<float> TankDensity { get; private set; }

        /// <summary>
        /// Number of milliseconds after GameCore initialization to wait before starting
        /// the game because we want all of the setup to be done and for people to be connected.
        /// </summary>
        public Setting<float> TimeToWaitBeforeStartingGame { get; private set; }

        /// <summary>
        /// The number of milliseconds after game to continue updating
        /// </summary>
        public Setting<float> TimePostGameToContinueRunning { get; private set; }

        /// <summary>
        /// The maximum number of particles to allow in game
        /// </summary>
        public Setting<int> ParticleLimit { get; private set; }


        /// <summary>
        /// The minimum delta time in milliseconds for a game tick. If DT is less than this,
        /// we do one tick, then skip the next one, etc. until we have a deficit of game time,
        /// then do another and repeat.
        /// </summary>
        public Setting<float> MinDeltaTimeGameTick { get; private set; }

        /// <summary>
        /// The maximum delta time for a game tick. If the time is greater, multiple steps will be taken.
        /// </summary>
        public Setting<float> MaxDeltaTimeGameTick { get; private set; }

        /// <summary>
        /// The maximum delta time in milliseconds that a tick of the particle emitter can be.
        /// If DT is greater than this, multiple steps will be taken rather than a single one.
        /// </summary>
        public Setting<float> ParticleEmitterMaxDeltaTime { get; private set; }

        public Setting<int> MaxStateChangeSize { get; private set; }

        public Setting<float> MaxStateChangeFrequency { get; private set; }

        public EngineSettings(string file) : base(file)
        {
        }

        public EngineSettings()
        {
        }

        protected override void SetDefaults()
        {
            PhysicsScale = new Setting<float>(this, "Physics Scale",
                  "The scale of the physics engine relative to game world space",
                  0.1f, Setting.SettingDisplayType.Percentage);

            TankDensity = new Setting<float>(this, "Tank density",
            "The density of a tank in the physics engine.", 15f);

            TimeToWaitBeforeStartingGame = new Setting<float>(this, "Pre game connection wait time",
            "The amount of time, in seconds, to wait before starting a game." +
            " This is here primarily to give users time to connect to the server and download map" +
            " and server data (mods, etc) before the game starts.", 5000f, Setting.SettingDisplayType.TimeMS);

            TimePostGameToContinueRunning = new Setting<float>(this, "Post game time",
            "The amount of time to keep the game running after the winner has been determined." +
            " Sort of a post round deathmatch phase and a phase that helps people read the outcome" +
            " of the game.", 5000, Setting.SettingDisplayType.TimeMS);

            ParticleLimit = new Setting<int>(this, "Particle limit",
            "The maximum number of particles to allow in a game. The default is 20,000 for good reason." +
            " If it's more, there is a quite a chance of lag (even here, it happens on occasion)." +
            " If you set it past 100,000, you deserve to die a slow and painful lag death and you probably will.", 20000);

            MinDeltaTimeGameTick = new Setting<float>(this, "Minimum Game Tick Time",
            "The smallest amount of time allowed for a single game tick." +
            " This probably should not be less than 1/3 of a millisecond or we run into numeric" +
            " precision issues in the physics engine.", 0.5f, Setting.SettingDisplayType.TimeMS);

            MaxDeltaTimeGameTick = new Setting<float>(this, "Maximum Game Tick Time",
            "The maximum amount of time allowed for a single game tick. If it's greater," +
            " multiple game ticks will be performed for the one tick. Unfortunately, while" +
            " this improves the stability of collisions, it can cause a 'spiral of death' if the" +
            " game tick loop is the bottleneck of the game.", 34f, Setting.SettingDisplayType.TimeMS);

            ParticleEmitterMaxDeltaTime = new Setting<float>(this, "Particle Emitter max tick time",
            "The maximum amount of time each tick of the particle engine can be. Changes to this value" +
            " can stop some particle rendering bugs due to faulty velocity calculations. Higher values lower" +
            " CPU time needed while lower values increase particle trail accuracy.",
            17f, Setting.SettingDisplayType.TimeMS);

            MaxStateChangeSize = new Setting<int>(this, "Maximum GameObject State Change Size",
                "The maximum size, in bytes, of a single state change for a GameObject.", 3072);

            MaxStateChangeFrequency = new Setting<float>(this, "Max state change frequency",
                "The maximum frequency of state changes in milliseconds, basically rate limiting.", 100,
                Setting.SettingDisplayType.TimeMS); //10 of them per second

        }
    }
}
