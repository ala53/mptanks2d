using Microsoft.Xna.Framework;
using MPTanks.Engine.Settings;
using MPTanks.Engine.Tanks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine
{
    public partial class GameCore
    {

        #region Add and Remove GameObjects
        private bool _inUpdateLoop = false;
        private HashSet<GameObject> _addQueue =
            new HashSet<GameObject>();
        private HashSet<GameObject> _removeQueue =
            new HashSet<GameObject>();
        private HashSet<GameObject> _objectsCurrentlyInDestructors =
            new HashSet<GameObject>();
        public void AddGameObject(GameObject obj, GameObject creator = null, bool authorized = false)
        {
            if (!authorized && !Authoritative)
            {
                Logger.Error("Unauthorized object addition attempted.");
                return;
            }

            if (creator == null)
                Logger.Trace("Object created: " + $"{obj.GetType().FullName}" +
                    $"[{obj.ObjectId}]. Authorized: {authorized}");
            else
                Logger.Trace("Object created: " + $"{obj.GetType().FullName}" +
                    $"[{obj.ObjectId}]. Created by {creator.GetType().FullName}" +
                    $"[{creator.ObjectId}]. Authorized: {authorized}");

            if (_inUpdateLoop) //In update loop, wait a frame.
            {
                _addQueue.Add(obj);
            }
            else
            {
                _gameObjects.Add(obj.ObjectId, obj);
                obj.Create(); //Call the creator function
                obj.OnStateChanged += HandleGameObjectStateChangedEvent;
                _isDirty = true; //Mark dirty flag
            }
        }

        public void RemoveGameObject(GameObject obj, GameObject destructor = null, bool authorized = false)
        {
            if (GlobalSettings.Debug)
            {
                if (!authorized && !Authoritative)
                    throw new Exception("Unauthorized removal of object");
            }
            else
            {
                if (!authorized && !Authoritative)
                {
                    Logger.Error("Unauthorized object destruction attempted.");
                    return;
                }
            }

            if (destructor == null)
                Logger.Trace("Object destroyed: " + $"{obj.GetType().FullName}" +
                    $"[{obj.ObjectId}]. Authorized: {authorized}");
            else
                Logger.Trace("Object destroyed: " + $"{obj.GetType().FullName}" +
                    $"[{obj.ObjectId}]. Destroyed by {destructor.GetType().FullName}" +
                    $"[{destructor.ObjectId}]. Authorized: {authorized}");

            //Sanity checks
            bool found = _gameObjects.ContainsValue(obj) || _addQueue.Contains(obj);
            //We want to prevent people from disposing of the bodies
            if (obj.Body.IsDisposed && found)
                Logger.Warning("Body already disposed, Trace:\n" + Environment.StackTrace);
            if (!found)
                return; //It doesn't exist - probably was already deleted by a previous object

            BeginDeletion(obj, destructor);
        }

        /// <summary>
        /// Processes the add and remove queue for gameobjects
        /// </summary>
        private void ProcessGameObjectQueues()
        {
            ProcessGameObjectDestructorQueue();
            ProcessGameObjectDeletionQueue();
        }

        private void ProcessGameObjectDeletionQueue()
        {
            foreach (var obj in _addQueue)
            {
                _gameObjects.Add(obj.ObjectId, obj);
                obj.Create(); //Call the creator function
                obj.OnStateChanged += HandleGameObjectStateChangedEvent;
                _isDirty = true; //Mark the dirty flag
            }

            foreach (var obj in _removeQueue)
            {
                _gameObjects.Remove(obj.ObjectId);
                obj.EndDestruction(); //Call final destructor
                _isDirty = true; //Mark the dirty flag
            }

            _addQueue.Clear();
            _removeQueue.Clear();
        }

        private HashSet<GameObject> _tempDestructorRemovalQueue =
            new HashSet<GameObject>();
        private void ProcessGameObjectDestructorQueue()
        {
            foreach (var obj in _objectsCurrentlyInDestructors)
            {
                if (obj.IsDestructionCompleted)
                    _tempDestructorRemovalQueue.Add(obj);
            }

            foreach (var obj in _tempDestructorRemovalQueue)
                MarkGameObjectForDeletion(obj);
            _tempDestructorRemovalQueue.Clear();
        }

        private void MarkGameObjectForDeletion(GameObject obj)
        {
            if (_inUpdateLoop) //We're in the for loop so wait a frame
            {
                if (_addQueue.Contains(obj))
                    _addQueue.Remove(obj);
                else
                    _removeQueue.Add(obj);
            }
            else
            {
                _gameObjects.Remove(obj.ObjectId);
                obj.EndDestruction(); //Call final destructor
                _isDirty = true; //Mark the dirty flag
            }
        }

        private void AddGameObjectToDestructorQueue(GameObject obj)
        {
            _objectsCurrentlyInDestructors.Add(obj);
        }

        private void BeginDeletion(GameObject obj, GameObject destructor = null)
        {
            //Housekeeping
            obj.OnStateChanged -= HandleGameObjectStateChangedEvent;

            if (obj.Destroy(destructor))
            {
                //It wants some time to finish up
                AddGameObjectToDestructorQueue(obj);
            }
            else
            {
                //No time needed, delete it
                MarkGameObjectForDeletion(obj);
            }
        }
        #endregion

        //Helper method for the event engine
        private void HandleGameObjectStateChangedEvent(object sender, Core.Events.Types.GameObjects.StateChangedEventArgs args)
        {
            EventEngine.RaiseGameObjectStateChanged(args);
        }

        #region Helpers for object creation
        public Tank AddTank(string reflectionName, GamePlayer player, bool authorized)
        {
            return AddTank(reflectionName, player, Vector2.Zero, 0, authorized);
        }

        public Tank AddTank(string reflectionName, GamePlayer player, Vector2 position = default(Vector2), float rotation = 0, bool authorized = false)
        {
            var tank = Tank.ReflectiveInitialize(reflectionName, player, player.Team, this, Authoritative);
            tank.Position = position;
            tank.Rotation = rotation;
            player.Tank = tank;
            AddGameObject(tank, null, Authoritative);
            return tank;
        }

        public Projectiles.Projectile AddProjectile(string reflectionName, Tank spawner, bool authorized)
        {
            return AddProjectile(reflectionName, spawner, Vector2.Zero, 0, authorized);
        }
        public Projectiles.Projectile AddProjectile(string reflectionName, Tank spawner, Vector2 position = default(Vector2), float rotation = 0, bool authorized = false)
        {
            var projectile = Projectiles.Projectile.ReflectiveInitialize(
                reflectionName, spawner, this, Authoritative, position, rotation);
            projectile.Position = position;
            projectile.Rotation = rotation;
            AddGameObject(projectile, null, Authoritative);
            return projectile;
        }
        public Maps.MapObjects.MapObject AddMapObject(string reflectionName, bool authorized)
        {
            return AddMapObject(reflectionName, Vector2.Zero, 0, authorized);
        }
        public Maps.MapObjects.MapObject AddMapObject(string reflectionName, Vector2 position = default(Vector2), float rotation = 0, bool authorized = false)
        {
            var obj = Maps.MapObjects.MapObject.ReflectiveInitialize(reflectionName, this, Authoritative, position, rotation);
            AddGameObject(obj, null, Authoritative);
            return obj;
        }

        public GameObject AddGameObject(string reflectionName, bool authorized)
        {
            return AddGameObject(reflectionName, Vector2.Zero, 0, authorized);
        }
        public GameObject AddGameObject(string reflectionName, Vector2 position = default(Vector2), float rotation = 0, bool authorized = false)
        {
            var obj = GameObject.ReflectiveInitialize(reflectionName, this, Authoritative);
            obj.Position = position;
            obj.Rotation = rotation;
            AddGameObject(obj, null, Authoritative);
            return obj;
        }
        #endregion
    }
}
