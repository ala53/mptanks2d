using Engine.Core.Timing;
using Engine.Logging;
using Engine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
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
        /// The manager which spawns in game powerups over time
        /// </summary>
        public Powerups.PowerupManager PowerupManager { get; private set; }
        /// <summary>
        /// The timer manager which lets game objects create timers for their own use
        /// </summary>
        public Timer.Factory TimerFactory { get; private set; }
        /// <summary>
        /// The Logger to use for logging important events
        /// </summary>
        public ILogger Logger { get; private set; }
        /// <summary>
        /// The game mode that dictates the rules for this instance.
        /// </summary>
        public Gamemodes.Gamemode Gamemode { get; private set; }
        private float _timescale = 1;
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
        private Dictionary<Guid, Tanks.Tank> _players = new Dictionary<Guid, Tanks.Tank>();
        public IReadOnlyDictionary<Guid, Tanks.Tank> Players { get { return _players; } }
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

        private List<GameObject> _gameObjects = new List<GameObject>();
        /// <summary>
        /// All GameObjects currently in game.
        /// </summary>
        public GameObject[] GameObjects { get { return _gameObjects.ToArray(); } }

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
        public GameCore(ILogger logger, Gamemodes.Gamemode gameMode, string mapName, bool skipInit = false)
        {
            Logger = logger;
            _skipInit = skipInit;
            if (skipInit)
                _gameStarted = true;

            Map = Maps.Map.LoadMap("", this);

            //Set up the game mode internally
            Gamemode = gameMode;
            Gamemode.SetGame(this);

            //Initialize game
            World = new FarseerPhysics.Dynamics.World(Vector2.Zero);
            TimerFactory = new Timer.Factory();
            AnimationEngine = new Rendering.Animations.AnimationEngine();
            ParticleEngine = new Rendering.Particles.ParticleEngine(this);
            EventEngine = new Core.Events.EventEngine(this);
            SharedRandom = new Random();
            Logger.Log("Game initialized");
        }

        #region Add and Remove GameObjects
        private bool _inUpdateLoop = false;
        private List<GameObject> _addQueue =
            new List<GameObject>();
        private List<GameObject> _removeQueue =
            new List<GameObject>();
        public void AddGameObject(GameObject obj, GameObject creator = null, bool authorized = false)
        {
#if DEBUG
            if (!authorized && !Authoritative)
                throw new Exception("Unauthorized addition of object");
#else
            if (!authorized && !Authoritative)
            {
                Logger.Error("Unauthorized object addition attempted.");
                return;
            }
#endif
            Logger.LogObjectCreated(obj, creator);
            obj.Alive = true;

            if (_inUpdateLoop) //In update loop, wait a frame.
            {
                _addQueue.Add(obj);
            }
            else
            {
                _gameObjects.Add(obj);
                _isDirty = true; //Mark dirty flag
            }
        }

        public void RemoveGameObject(GameObject obj, GameObject destructor = null, bool authorized = false)
        {
#if DEBUG
            if (!authorized && !Authoritative)
                throw new Exception("Unauthorized removal of object");
#else
            if (!authorized && !Authoritative)
            {
                Logger.Error("Unauthorized object destruction attempted.");
                return;
            }
#endif
            Logger.LogObjectDestroyed(obj, destructor);
            obj.Alive = false;

            bool found = _gameObjects.Contains(obj) || _addQueue.Contains(obj);
            //We want to prevent people from disposing of the bodies
            if (obj.Body.IsDisposed && found)
                Logger.Warning("Body already disposed, Trace:\n" + Environment.StackTrace);

            obj.Destroy(destructor); //Call destructor
            if (!found)
                return; //It doesn't exist - probably was already deleted by a previous object

            if (_inUpdateLoop) //We're in the for loop so wait a frame
            {
                if (_addQueue.Contains(obj))
                    _addQueue.Remove(obj);
                else
                    _removeQueue.Add(obj);
            }
            else
            {
                _gameObjects.Remove(obj);
                _isDirty = true; //Mark the dirty flag
            }
        }

        /// <summary>
        /// Processes the add and remove queue for gameobjects
        /// </summary>
        private void ProcessQueue()
        {
            foreach (var obj in _addQueue)
            {
                _gameObjects.Add(obj);
                _isDirty = true; //Mark the dirty flag
            }

            foreach (var obj in _removeQueue)
            {
                _gameObjects.Remove(obj);
                _isDirty = true; //Mark the dirty flag
            }

            _addQueue.Clear();
            _removeQueue.Clear();
        }
        #endregion
        private bool _hasDoneCleanup;
        public void Update(GameTime gameTime)
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
                Logger.Log("Game started");
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
            if (Timescale != 1)
            {
                gameTime = new GameTime(gameTime.TotalGameTime, TimeSpan.FromMilliseconds(
                    gameTime.ElapsedGameTime.TotalMilliseconds * Timescale));
            }

            //Mark the in-loop flag so any removals happen next frame and don't corrupt the state
            _inUpdateLoop = true;
            //Process timers
            TimerFactory.Update(gameTime);
            //start particle engine task
            var pTask = Task.Run(() => ParticleEngine.Update(gameTime));
            //Process animations
            AnimationEngine.Update(gameTime);
            //Process individual objects
            foreach (var obj in _gameObjects)
                obj.Update(gameTime);
            //Tick physics
            World.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            //Let the gamemode do it's calculations
            Gamemode.Update(gameTime);
            //Wait for particle engine
            pTask.Wait();
            //And notify that we exited the update loop
            _inUpdateLoop = false;
            //Remove objects that were supposed to be removed last frame (foreach loop issues)
            ProcessQueue();
        }
    }
}
