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
        /// The Logger to use for logging important events
        /// </summary>
        public ILogger Logger { get; private set; }
        /// <summary>
        /// The game mode that dictates the rules for this instance.
        /// </summary>
        public Gamemodes.Gamemode Gamemode { get; private set; }

        #region World Management
        /// <summary>
        /// The timer manager which lets game objects create timers for their own use
        /// </summary>
        public Timer.Factory TimerFactory { get; private set; }
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
        public bool IsDirty { get { return _isDirty; } }
        #endregion
        #endregion
        public GameCore(ILogger logger, Gamemodes.Gamemode gameMode)
        {
            Logger = logger; 

            //Set up the game mode internally
            Gamemode = gameMode;
            Gamemode.SetGame(this);

            //Initialize game
            World = new FarseerPhysics.Dynamics.World(Vector2.Zero);
            TimerFactory = new Timer.Factory();
            AnimationEngine = new Rendering.Animations.AnimationEngine();
            EventEngine = new Core.Events.EventEngine(this);
            Logger.Log("Game started");
        }

        private bool _inUpdateLoop = false;
        private List<GameObject> _addQueue =
            new List<GameObject>();
        private List<GameObject> _removeQueue =
            new List<GameObject>();
        public void AddGameObject(GameObject obj, GameObject creator = null)
        {
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

        public void RemoveGameObject(GameObject obj, GameObject destructor = null)
        {
            Logger.LogObjectDestroyed(obj, destructor);
            obj.Alive = false;

            bool found = _gameObjects.Contains(obj) || _addQueue.Contains(obj);
            //We want to prevent people from disposing of the bodies
            if (obj.Body.IsDisposed && found)
                Logger.Warning("Body already disposed, Trace:\n" + Environment.StackTrace);

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
                if (!obj.Body.IsDisposed)
                    obj.Body.Dispose(); //In case it isn't disposed, remove the entire body from physics
                obj.Destroy(); //Call destructor
                _gameObjects.Remove(obj);
                _isDirty = true; //Mark the dirty flag
            }
        }

        /// <summary>
        /// Processes the add and remove queue for gameobjects
        /// </summary>
        private void ProcessQueue()
        {
            foreach (var obj in _addQueue) { 
                _gameObjects.Add(obj); 
                _isDirty = true; //Mark the dirty flag
            }

            foreach (var obj in _removeQueue)
            {
                if (!obj.Body.IsDisposed)
                    obj.Body.Dispose(); //In case it isn't disposed, remove the entire body from physics
                obj.Destroy(); //Call the destructor
                _gameObjects.Remove(obj);
                _isDirty = true; //Mark the dirty flag
            }

            _addQueue.Clear();
            _removeQueue.Clear();
        }

        public void Update(GameTime gameTime)
        {
            //Mark the in-loop flag so any removals happen next frame and don't corrupt the state
            _inUpdateLoop = true;
            //Remove objects that were supposed to be removed last frame (foreach loop issues)
            ProcessQueue();
            //Process timers
            TimerFactory.Update(gameTime);
            //Process animations
            AnimationEngine.Update(gameTime);
            //Process individual objects
            foreach (var obj in _gameObjects)
                obj.Update(gameTime);
            //Tick physics
            World.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
            //Let the gamemode do it's calculations
            Gamemode.Update(gameTime);
            //And notify that we exited the update loop
            _inUpdateLoop = false;
        }
    }
}
