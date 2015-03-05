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
    public class GameCore
    {
        /// <summary>
        /// Whether this instance is just another client or it is the server.
        /// Helps with deciding when to play death sequences, etc.
        /// </summary>
        public bool Authoritative { get; private set; }
        /// <summary>
        /// The animations that are currently playing (sprite sheet animations)
        /// </summary>
        public AnimationEngine Animations { get; private set; }

        /// <summary>
        /// The internally accessible random number generator, for determinism
        /// </summary>
        internal Random RandomGenerator { get; private set; }

        /// <summary>
        /// The manager which spawns in game powerups over time
        /// </summary>
        public Powerups.PowerupManager PowerupManager { get; private set; }
        /// <summary>
        /// The Logger to use for logging important events
        /// </summary>
        public ILogger Logger { get; private set; }

        #region World Management
        /// <summary>
        /// The timer manager which lets game objects create timers for their own use
        /// </summary>
        public Timer.Factory TimerFactory { get; private set; }
        internal FarseerPhysics.Dynamics.World World { get; private set; }
        private int _nextObjectId = 0;
        /// <summary>
        /// The next available object id to use
        /// </summary>
        internal int NextObjectId { get { return _nextObjectId++; } }

        private List<GameObject> _gameObjects = new List<GameObject>();
        public GameObject[] GameObjects { get { return _gameObjects.ToArray(); } }

        private bool _isDirty = false;
        /// <summary>
        /// Tells whether the GameObject collection has changed since the last time IsDirty was checked.
        /// </summary>
        public bool IsDirty { get { return _isDirty; } }
        #endregion

        public GameCore(ILogger logger)
        {
            Logger = logger;
            World = new FarseerPhysics.Dynamics.World(Vector2.Zero);
            RandomGenerator = new Random();
            TimerFactory = new Timer.Factory();
            Animations = new AnimationEngine();
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

            if (_inUpdateLoop)
            {
                _addQueue.Add(obj);
            }
            else
            {
                _gameObjects.Add(obj);
                _isDirty = true;
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

            if (_inUpdateLoop)
            {
                if (!found)
                    return; //It doesn't exist - probably was already deleted by a previous object

                if (_addQueue.Contains(obj))
                    _addQueue.Remove(obj);
                else
                    _removeQueue.Add(obj);
            }
            else
            {
                if (!obj.Body.IsDisposed)
                    obj.Body.Dispose();
                obj.Destroy();
                _gameObjects.Remove(obj);
                _isDirty = true;
            }
        }

        /// <summary>
        /// Processes the add and remove queue for gameobjects
        /// </summary>
        private void ProcessQueue()
        {
            foreach (var obj in _addQueue)
                _gameObjects.Add(obj);

            foreach (var obj in _removeQueue)
            {
                if (!obj.Body.IsDisposed)
                    obj.Body.Dispose();
                obj.Destroy();
                _gameObjects.Remove(obj);
            }

            _addQueue.Clear();
            _removeQueue.Clear();
        }

        public void Update(GameTime gameTime)
        {
            ProcessQueue();
            _inUpdateLoop = true;

            //Process timers
            TimerFactory.Update(gameTime);

            //Process animations
            Animations.Update(gameTime);

            foreach (var obj in _gameObjects)
                obj.Update(gameTime);

            World.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
        }
    }
}
