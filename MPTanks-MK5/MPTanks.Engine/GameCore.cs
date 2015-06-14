using MPTanks.Engine.Core.Timing;
using MPTanks.Engine.Logging;
using MPTanks.Engine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Engine.Settings;

namespace MPTanks.Engine
{
    public partial class GameCore
    {
        #region Properties
        /// <summary>
        /// Gets or sets whether this instance is just another client or it is the server.
        /// Helps with deciding when to play death sequences, etc.
        /// </summary>
        public bool Authoritative { get; set; }
        /// <summary>
        /// The animations that are currently playing (sprite sheet animation descriptions).
        /// Note: animations are long running, controllable objects with a significant overhead
        /// while particles are small simplistic objects that can't be controlled well.
        /// </summary>
        public Rendering.Animations.AnimationEngine AnimationEngine { get; private set; }
        /// <summary>
        /// The particle system for the game. Use this for short lived objects that
        /// do not need fine grain control. Once created, you have no control over the particle.
        /// </summary>
        public Rendering.Particles.ParticleEngine ParticleEngine { get; private set; }
        /// <summary>
        /// The timer manager which lets game objects create timers for their own use
        /// </summary>
        public Timer.Factory TimerFactory { get; private set; }
        /// <summary>
        /// The sound engine for the game. Manages where sounds should be and when they're playing.
        /// </summary>
        public Sound.SoundEngine SoundEngine { get; private set; }
        /// <summary>
        /// The lighting positions.
        /// </summary>
        public Rendering.Lighting.LightEngine LightEngine { get; private set; }
        #region Diagnostics & Logging
        /// <summary>
        /// The Logger to use for logging important events
        /// </summary>
        public ILogger Logger { get; private set; }
        public Diagnostics Diagnostics { get; private set; }
        /// <summary>
        /// The parent for logging the diagnostics under.
        /// </summary>
        public string DiagnosticsParent { get; set; }
        #endregion
        /// <summary>
        /// The game mode that dictates the rules for this instance.
        /// </summary>
        public Gamemodes.Gamemode Gamemode { get; private set; }
        public EngineSettings Settings { get; private set; }
        public RPC.RemoteProcedureCallHelper RPCHelper { get; private set; }
        public float Timescale { get; set; }
        #region Game Status
        /// <summary>
        /// Gets whether the game is waiting for players or if it has started.
        /// </summary>
        public bool IsWaitingForPlayers { get { return !HasEnoughPlayersToStart(); } }
        private bool _gameStarted = false;
        public bool IsGameRunning
        {
            get
            {
                bool running = _gameStarted;
                if (Gamemode.GameEnded)
                    running = IsStillInPostGamePhysicsPhase();
                return running;
            }
        }
        public float RemainingCountdownSeconds { get; private set; }
        public bool IsCountingDownToStart { get { return RemainingCountdownSeconds > 0; } }
        #endregion
        private Dictionary<Guid, GamePlayer> _playersById = new Dictionary<Guid, GamePlayer>();
        public IReadOnlyDictionary<Guid, GamePlayer> PlayersById { get { return _playersById; } }

        public IList<GamePlayer> Players { get { return _playersById.Values.ToList(); } }

        public Maps.Map Map { get; private set; }

        #region World Management
        public Random SharedRandom { get; private set; }
        /// <summary>
        /// The physics world that the game runs in.
        /// </summary>
        internal FarseerPhysics.Dynamics.World World { get; private set; }
        private int _nextObjectId = 0;
        /// <summary>
        /// The next available object id to use
        /// </summary>
        internal int NextObjectId { get { return _nextObjectId++; } }

        private Dictionary<int, GameObject> _gameObjects = new Dictionary<int, GameObject>();
        /// <summary>
        /// All GameObjects currently in game.
        /// </summary>
        public IReadOnlyDictionary<int, GameObject> GameObjectsWithIds { get { return _gameObjects; } }

        public IEnumerable<GameObject> GameObjects { get { return _gameObjects.Values; } }

        public Core.Events.EventEngine EventEngine { get; private set; }

        /// <summary>
        /// Whether the game allows for friendly fire among players
        /// </summary>
        public bool FriendlyFireEnabled { get; set; }

        #region Tanks (collections by type)
        /// <summary>
        /// The tanks alive in the game, excluding SuperTanks
        /// </summary>
        public IEnumerable<Tanks.Tank> Tanks
        {
            get
            {
                foreach (var obj in GameObjects)
                    if (obj.GetType().IsSubclassOf(typeof(Tanks.Tank)) &&
                        !obj.GetType().IsSubclassOf(typeof(Tanks.SuperTank)))
                        yield return (Tanks.Tank)obj;
            }
        }

        /// <summary>
        /// The SuperTanks alive in the game
        /// </summary>
        public IEnumerable<Tanks.SuperTank> SuperTanks
        {
            get
            {
                foreach (var obj in GameObjects)
                    if (obj.GetType().IsSubclassOf(typeof(Tanks.SuperTank)))
                        yield return (Tanks.SuperTank)obj;
            }
        }

        /// <summary>
        /// All tanks alive in the game, both normal and super.
        /// </summary>
        public IEnumerable<Tanks.Tank> AllTanks
        {
            get
            {
                foreach (var obj in GameObjects)
                    if (obj.GetType().IsSubclassOf(typeof(Tanks.Tank)))
                        yield return (Tanks.Tank)obj;
            }
        }
        #endregion

        private bool _isDirty = false;
        /// <summary>
        /// Tells whether the GameObject collection has changed since the last time IsDirty was checked.
        /// </summary>
        public bool IsDirty
        {
            get
            {
                var _dirty = _isDirty;
                _isDirty = false;
                return _dirty;
            }
        }
        #endregion
        #endregion
        private bool _skipInit;

        /// <summary>
        /// Creates a new GameCore instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="gameMode"></param>
        /// <param name="skipInit">Whether to skip the customary X second init and gamemode setup</param>
        public GameCore(ILogger logger, Gamemodes.Gamemode gameMode, string mapData, bool skipInit = false, EngineSettings settings = null)
        {
            Logger = logger;
            if (settings == null)
                Settings = new EngineSettings();
            else
                Settings = settings;
            //Safety: call all global Ctors
            ConstructorHelper.CallGlobalCtors();

            _skipInit = skipInit;
            if (skipInit)
                _gameStarted = true;

            Map = Maps.Map.LoadMap(mapData, this);

            //Set up the game mode internally
            Gamemode = gameMode;
            Gamemode.SetGame(this);

            //Initialize game
            Timescale = 1;
            World = new FarseerPhysics.Dynamics.World(Vector2.Zero);
            TimerFactory = new Timer.Factory();
            AnimationEngine = new Rendering.Animations.AnimationEngine();
            ParticleEngine = new Rendering.Particles.ParticleEngine(this);
            EventEngine = new Core.Events.EventEngine(this);
            SharedRandom = new Random();
            Diagnostics = new Diagnostics();
            RPCHelper = new RPC.RemoteProcedureCallHelper(this);
            LightEngine = new Rendering.Lighting.LightEngine();
            SoundEngine = new Sound.SoundEngine(this);
            DiagnosticsParent = "Game Update";
            Logger.Info(Strings.Engine.GameCreated);
        }

        private bool _hasDoneCleanup;
        private GameTime _internalGameTime = new GameTime();
        private double _deficitGameTime;
        public void Update(GameTime gameTime)
        {
            _deficitGameTime += gameTime.ElapsedGameTime.TotalMilliseconds * Timescale;
            if (_deficitGameTime <= 0) return;
            //
            var totalGameTime = gameTime.TotalGameTime - TimeSpan.FromMilliseconds(_deficitGameTime);
            //Loop until we have fulfilled the game time deficit
            while (_deficitGameTime > 0)
            {
                //Figure out how big the step is
                var tickMs = _deficitGameTime;
                if (tickMs < Settings.MinDeltaTimeGameTick) tickMs = Settings.MinDeltaTimeGameTick;
                if (tickMs > Settings.MaxDeltaTimeGameTick) tickMs = Settings.MaxDeltaTimeGameTick;
                //Build the gametime instance
                _internalGameTime.ElapsedGameTime = TimeSpan.FromMilliseconds(tickMs);
                _internalGameTime.TotalGameTime = totalGameTime;
                //Do the tick
                DoUpdate(_internalGameTime);

                //And correct the total game time
                totalGameTime += TimeSpan.FromMilliseconds(tickMs);

                //Subtract from the deficit
                _deficitGameTime -= tickMs;
                //...and repeat
            }
        }

        private void DoUpdate(GameTime gameTime)
        {
            if (Gamemode.GameEnded)
            {
                TickGameEnd(gameTime);
                //Check if whe should still be updating
                if (IsStillInPostGamePhysicsPhase())
                    UpdateInGame(gameTime);

                //Do nothing, cleanup time
                if (!_hasDoneCleanup)
                {
                    _hasDoneCleanup = true;
                    EndGame();
                }
            }
            else if (IsGameRunning)
            {
                //Run the game *cough* like you're supposed to *cough*
                UpdateInGame(gameTime);
            }
            else
            {
                if (!IsWaitingForPlayers) //Only tick game start if we have enough players
                    TickGameStart(gameTime);
                else
                    _timeSinceGameBeganStarting = 0; //Reset the counter if not ready
            }
        }

        private void EndGame()
        {
        }

        private double _timeSinceGameEnded = 0;
        private bool IsStillInPostGamePhysicsPhase()
        {
            return _timeSinceGameEnded
                < Settings.TimePostGameToContinueRunning;
        }

        private void TickGameEnd(GameTime gameTime)
        {
            _timeSinceGameEnded += gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        private double _timeSinceGameBeganStarting = 0;
        /// <summary>
        /// Does a loop to wait before starting the game
        /// </summary>
        private void TickGameStart(GameTime gameTime)
        {
            _timeSinceGameBeganStarting += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_timeSinceGameBeganStarting > Settings.TimeToWaitBeforeStartingGame)
            {
                _gameStarted = true; //Start the game when allowed
                BeginGame();
                Logger.Info(Strings.Engine.GameStarted);
            }

            RemainingCountdownSeconds = (Settings.TimeToWaitBeforeStartingGame / 1000)
                - (float)(_timeSinceGameBeganStarting / 1000);
        }

        private void BeginGame()
        {
            //Create the player objects (server only)
            SetUpGamePlayers();
            //And load the map / create the map objects
            CreateMapObjects();
        }

        /// <summary>
        /// The core update loop of the game
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateInGame(GameTime gameTime)
        {
            var hasControlOfParent = !Diagnostics.IsMeasuring(DiagnosticsParent);
            if (hasControlOfParent) Diagnostics.BeginMeasurement(DiagnosticsParent);

            Diagnostics.BeginMeasurement("Begin UpdateInGame()", DiagnosticsParent);

            //Mark the in-loop flag so any removals happen next frame and don't corrupt the state
            _inUpdateLoop = true;

            Diagnostics.MonitorCall(() => TimerFactory.Update(gameTime), "Timer Updates", DiagnosticsParent);

            Diagnostics.MonitorCall(() => AnimationEngine.Update(gameTime), "Animation Updates", DiagnosticsParent);

            Diagnostics.MonitorForEach(GameObjects, o => o.Update(gameTime),
                "GameObject.Update() calls", DiagnosticsParent);

            Diagnostics.MonitorCall(() => World.Step((float)gameTime.ElapsedGameTime.TotalSeconds),
                "Physics step", DiagnosticsParent);

            Diagnostics.MonitorForEach(GameObjects, o => o.UpdatePostPhysics(gameTime),
                "GameObject.UpdatePostPhysics() calls", DiagnosticsParent);

            Diagnostics.MonitorCall(() => ParticleEngine.Update(gameTime), "Particle Updates", DiagnosticsParent);

            Diagnostics.MonitorCall(() => Gamemode.Update(gameTime), "Gamemode update", DiagnosticsParent);

            //And notify that we exited the update loop
            _inUpdateLoop = false;

            Diagnostics.MonitorCall(ProcessGameObjectQueues, "Process add/remove queue", DiagnosticsParent);

            Diagnostics.EndMeasurement("Begin UpdateInGame()", DiagnosticsParent);
            if (hasControlOfParent) Diagnostics.EndMeasurement(DiagnosticsParent);
        }
    }
}
