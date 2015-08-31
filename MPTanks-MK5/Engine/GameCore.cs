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
using Newtonsoft.Json;
using MPTanks.Modding;

namespace MPTanks.Engine
{
    public partial class GameCore
    {
        #region 
        public bool Running
        {
            get
            {
                return Status == CurrentGameStatus.GameRunning ||
                    Status == CurrentGameStatus.GameEndedStillRunning;
            }
        }
        public bool Ended
        {
            get
            {
                return Status == CurrentGameStatus.GameEndedStillRunning ||
                    Status == CurrentGameStatus.GameEnded;
            }
        }
        public bool WaitingForPlayers { get { return Status == CurrentGameStatus.WaitingForPlayers; } }
        public CurrentGameStatus Status { get; private set; }
        public enum CurrentGameStatus : byte
        {
            WaitingForPlayers,
            GameRunning,
            GameEndedStillRunning,
            GameEnded
        }
        #endregion
        #region Properties
        /// <summary>
        /// Displays whether the game has been started by calling HasStarted() or not
        /// </summary>
        public bool HasStarted { get; private set; }
        /// <summary>
        /// Gets whether the game is able to start (read: has enough players)
        /// </summary>
        public bool HasEnoughPlayersToStart => _playerIds.Count >= Gamemode.MinPlayerCount;
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
        [JsonIgnore]
        public Rendering.Animations.AnimationEngine AnimationEngine { get; private set; }
        /// <summary>
        /// The game time, accounting for slow motion, that the game is currently at.
        /// </summary>
        public TimeSpan Time { get; private set; }
        /// <summary>
        /// The particle system for the game. Use this for short lived objects that
        /// do not need fine grain control. Once created, you have no control over the particle.
        /// </summary>
        [JsonIgnore]
        public Rendering.Particles.ParticleEngine ParticleEngine { get; private set; }
        /// <summary>
        /// The timer manager which lets game objects create timers for their own use
        /// </summary>
        public Timer.Factory TimerFactory { get; private set; }
        /// <summary>
        /// The sound engine for the game. Manages where sounds should be and when they're playing.
        /// </summary>
        [JsonIgnore]
        public Sound.SoundEngine SoundEngine { get; private set; }
        /// <summary>
        /// The lighting positions.
        /// </summary>
        [JsonIgnore]
        public Rendering.Lighting.LightEngine LightEngine { get; private set; }
        #region Diagnostics & Logging
        /// <summary>
        /// The Logger to use for logging important events
        /// </summary>
        [JsonIgnore]
        public ILogger Logger { get; private set; }
        [JsonIgnore]
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
        [JsonIgnore]
        public RPC.RemoteProcedureCallHelper RPCHelper { get; private set; }

        public IEnumerable<GamePlayer> Spectators
        {
            get
            {
                foreach (var player in Players)
                    if (player.IsSpectator) yield return player;
            }
        }

        public IEnumerable<GamePlayer> ActivePlayers
        {
            get
            {
                foreach (var player in Players)
                    if (!player.IsSpectator) yield return player;
            }
        }

        [JsonIgnore]
        public Maps.Map Map { get; private set; }

        #region World Management
        public Random SharedRandom { get; private set; }
        /// <summary>
        /// The physics world that the game runs in.
        /// </summary>
        [JsonIgnore]
        internal FarseerPhysics.Dynamics.World World { get; private set; }
        private ushort _nextObjectId = 0;
        /// <summary>
        /// The next available object id to use
        /// </summary>
        internal ushort NextObjectId
        {
            get
            {
                if (++_nextObjectId == ushort.MaxValue) _nextObjectId = NextObjectId;
                while (GameObjectsById.ContainsKey(_nextObjectId)) _nextObjectId++;
                
                return _nextObjectId;
            }
        }

        private Dictionary<int, GameObject> _gameObjects = new Dictionary<int, GameObject>();
        /// <summary>
        /// All GameObjects currently in game.
        /// </summary>
        public IReadOnlyDictionary<int, GameObject> GameObjectsById { get { return _gameObjects; } }

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
        #endregion
        #endregion
        public GameCore(ILogger logger, string gamemodeReflectionName, ModAssetInfo map, EngineSettings settings = null)
            : this(logger, Gamemodes.Gamemode.ReflectiveInitialize(gamemodeReflectionName), map, settings)
        { }

        /// <summary>
        /// Creates a new GameCore instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="gamemode"></param>
        /// <param name="skipInit">Whether to skip the customary X second init and gamemode setup</param>
        public GameCore(ILogger logger, Gamemodes.Gamemode gamemode, ModAssetInfo map, EngineSettings settings = null)
        {
            Logger = logger;
            if (Logger == null) Logger = new NullLogger();
            if (settings == null)
                Settings = new EngineSettings();
            else
                Settings = settings;

            //Set up the game mode internally
            Gamemode = gamemode;
            Gamemode.SetGame(this);

            Map = Maps.Map.LoadMap(map, this);

            InitializeGame();
        }
        /// <summary>
        /// Creates a new GameCore instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="gamemode"></param>
        /// <param name="skipInit">Whether to skip the customary X second init and gamemode setup</param>
        public GameCore(ILogger logger, Gamemodes.Gamemode gamemode, string map, EngineSettings settings = null)
        {
            Logger = logger;
            if (Logger == null) Logger = new NullLogger();
            if (settings == null)
                Settings = new EngineSettings();
            else
                Settings = settings;

            //Set up the game mode internally
            Gamemode = gamemode;
            Gamemode.SetGame(this);

            Map = Maps.Map.LoadMap(map, this);

            InitializeGame();
        }

        private void InitializeGame()
        {
            //Initialize game
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

            Gamemode.Create();
            Logger.Info(Strings.Engine.GameCreated + $" ({Gamemode.DisplayName})");

        }
        /// <summary>
        /// Begins the game, letting it start its update loop
        /// </summary>
        /// <param name="shouldDoWorldSetup">Whether to create tanks and map objects</param>
        public void BeginGame(bool shouldDoWorldSetup = true)
        {
            if (HasStarted) return;
            if (!HasEnoughPlayersToStart) return;

            EventEngine.UnsafeDisableEvents();

            HasStarted = true;

            Status = CurrentGameStatus.GameRunning;

            if (shouldDoWorldSetup)
            {
                //Create the player objects (server only)
                SetupGamePlayers();
                //And load the map / create the map objects
                CreateMapObjects();
            }
            Gamemode.StartGame();

            EventEngine.UnsafeEnableEvents();
            //And raise events
            EventEngine.RaiseGameStarted();
        }

        public void UnsafeTickGameWorld(float deltaMs)
        {
            while (deltaMs > 0)
            {
                var tickTime = Math.Min(deltaMs, Settings.MaxDeltaTimeGameTick);
                World.Step(tickTime / 1000);
                deltaMs -= tickTime;
            }
        }

        private bool _hasDoneCleanup;
        private GameTime _internalGameTime = new GameTime();
        private double _deficitGameTime;
        public void Update(GameTime gameTime)
        {
            _deficitGameTime += gameTime.ElapsedGameTime.TotalMilliseconds * Timescale.Fractional;
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
            if (!HasStarted && !HasEnoughPlayersToStart)
            {
                Status = CurrentGameStatus.WaitingForPlayers;
                return;
            }
            if (!HasStarted) return;

            Time += gameTime.ElapsedGameTime;

            if (Gamemode.GameEnded)
            {
                if (_gameEndedTime == default(TimeSpan)) _gameEndedTime = Time;
                //Check if whe should still be updating
                if (IsStillInPostGamePhysicsPhase())
                {
                    Status = CurrentGameStatus.GameEndedStillRunning;
                    UpdateInGame(gameTime);
                }
                else
                {
                    Status = CurrentGameStatus.GameEnded;
                    //Do nothing, cleanup time
                    if (!_hasDoneCleanup)
                    {
                        _hasDoneCleanup = true;
                        EndGame();
                    }
                }
            }
            else if (Status == CurrentGameStatus.GameRunning)
            {
                //Run the game *cough* like you're supposed to *cough*
                UpdateHotJoinPlayers();
                UpdateInGame(gameTime);
            }
        }

        private void EndGame()
        {
            EventEngine.RaiseGameEnded(Gamemode.WinningTeam);
        }

        private TimeSpan _gameEndedTime;
        private bool IsStillInPostGamePhysicsPhase()
        {
            return (Time - _gameEndedTime)
                < TimeSpan.FromMilliseconds(Settings.TimePostGameToContinueRunning);
        }

        /// <summary>
        /// The core update loop of the game
        /// </summary>
        /// <param name="gameTime"></param>
        private GameTime _currentGameTime;
        private void UpdateInGame(GameTime gameTime)
        {
            _currentGameTime = gameTime;
            var hasControlOfParent = !Diagnostics.IsMeasuring(DiagnosticsParent);
            if (hasControlOfParent) Diagnostics.BeginMeasurement(DiagnosticsParent);

            Diagnostics.BeginMeasurement("Begin UpdateInGame()", DiagnosticsParent);

            //Mark the in-loop flag so any removals happen next frame and don't corrupt the state
            _inUpdateLoop = true;

            Diagnostics.MonitorCall(TimerFactory.Update, _currentGameTime, "Timer Updates", DiagnosticsParent);

            Diagnostics.MonitorCall(AnimationEngine.Update, _currentGameTime, "Animation Updates", DiagnosticsParent);

            Diagnostics.MonitorForEach(GameObjects, o => o.Update(_currentGameTime),
                "GameObject.Update() calls", DiagnosticsParent);

            Diagnostics.MonitorCall(World.Step, (float)_currentGameTime.ElapsedGameTime.TotalSeconds,
                "Physics step", DiagnosticsParent);

            Diagnostics.MonitorForEach(GameObjects, o => o.UpdatePostPhysics(_currentGameTime),
                "GameObject.UpdatePostPhysics() calls", DiagnosticsParent);

            Diagnostics.MonitorCall(ParticleEngine.Update, _currentGameTime, "Particle Updates", DiagnosticsParent);

            Diagnostics.MonitorCall(Gamemode.Update, _currentGameTime, "Gamemode update", DiagnosticsParent);

            //And notify that we exited the update loop
            _inUpdateLoop = false;

            Diagnostics.MonitorCall(ProcessGameObjectQueues, "Process add/remove queue", DiagnosticsParent);

            //And Events
            Diagnostics.MonitorCall(EventEngine.RaiseGameUpdate, gameTime, "OnGameUpdate event", DiagnosticsParent);

            Diagnostics.EndMeasurement("Begin UpdateInGame()", DiagnosticsParent);
            if (hasControlOfParent) Diagnostics.EndMeasurement(DiagnosticsParent);

            Time += gameTime.ElapsedGameTime;
        }
    }
}
