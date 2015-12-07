using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using MPTanks.Engine.Rendering;
using MPTanks.Modding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MPTanks.Engine
{
    /// <summary>
    /// The base type all gameobjects derive from. You can create a completely custom type if you want, but
    /// reflective initialization and useful utilities will mostly not exist.
    /// </summary>
    public abstract partial class GameObject
    {
        #region Basic Properties
        public Color ColorMask { get; set; }
        public ushort ObjectId { get; internal set; }
        public int DrawLayer { get; set; }
        [JsonIgnore]
        public Body Body { get; protected set; }
        [JsonIgnore]
        public GameCore Game { get; private set; }
        /// <summary>
        /// Whether the object is alive or not;
        /// </summary>
        public bool Alive { get; private set; }
        /// <summary>
        /// Whether the object will be deestroyed if its health goes below 0; if not, the minimum possible health
        /// will be 0.01 and the system will not allow you to go below said point.
        /// </summary>
        public bool CanBeDestroyed { get; protected set; } = true;
        private float _health;
        /// <summary>
        /// The current health level for the tank.
        /// </summary>
        public float Health
        {
            get { return _health; }
            set
            {
                if (_health != value) RaiseBasicPropertyChange(BasicPropertyChangeEventType.Health, Health, value);
                _health = value;
                if (!CanBeDestroyed && _health <= 0) _health = 0.01f;
            }
        }
        public bool Authoritative { get { return Game.Authoritative; } }
        /// <summary>
        /// The number of milliseconds that the object has been alive
        /// </summary>
        public TimeSpan TimeAlive { get; private set; }
        /// <summary>
        /// The lifespan in milliseconds of the object
        /// </summary>
        public TimeSpan Lifespan { get; protected set; }
        public TimeSpan PostDeathExistenceTime { get; protected set; }
        public bool IsDestructionCompleted { get; protected set; }
        #endregion

        #region Wrappers for Farseer Body
        //Since farseer is at a much smaller scale, we normalize to world space
        public Vector2 Position
        {
            get
            {
                if (_hasBeenCreated)
                    return Body.Position / Game.Settings.PhysicsScale;
                else
                    return _startPosition;
            }
            set
            {
                if (Position != value)
                    RaiseBasicPropertyChange(BasicPropertyChangeEventType.Position, Position, value);

                if (_hasBeenCreated)
                    Body.Position = value * Game.Settings.PhysicsScale;
                else
                    _startPosition = value;
            }
        }

        /// <summary>
        /// The rotation of the physics object in radians
        /// </summary>
        public float Rotation
        {
            get
            {
                if (_hasBeenCreated)
                    return Body.Rotation;
                else
                    return _startRotation;
            }
            set
            {
                if (Rotation != value)
                    RaiseBasicPropertyChange(BasicPropertyChangeEventType.Rotation, Rotation, value);

                value = value % ((float)Math.PI * 2);

                if (_hasBeenCreated)
                    Body.Rotation = value;
                else
                    _startRotation = value;
            }
        }

        public Vector2 LinearVelocity
        {
            get
            {
                if (_hasBeenCreated)
                    return Body.LinearVelocity / Game.Settings.PhysicsScale;
                else
                    return _startVelocity;
            }
            set
            {
                if (LinearVelocity != value)
                    RaiseBasicPropertyChange(BasicPropertyChangeEventType.LinearVelocity, LinearVelocity, value);

                if (_hasBeenCreated)
                    Body.LinearVelocity = value * Game.Settings.PhysicsScale;
                else
                    _startVelocity = value;
            }
        }

        public float AngularVelocity
        {
            get
            {
                if (_hasBeenCreated)
                    return Body.AngularVelocity;
                else
                    return _startAngularVelocity;
            }
            set
            {
                if (AngularVelocity != value)
                    RaiseBasicPropertyChange(BasicPropertyChangeEventType.AngularVelocity, AngularVelocity, value);

                if (_hasBeenCreated)
                    Body.AngularVelocity = value;
                else
                    _startAngularVelocity = value;
            }
        }
        private Vector2 _size;
        public Vector2 Size
        {
            get { return _size; }
            set
            {
                if (Size == value || value.X <= 0 || value.Y <= 0) return;

                RaiseBasicPropertyChange(BasicPropertyChangeEventType.Size, Size, value);

                //Set the internal note
                _size = value;

                if (_hasBeenCreated)
                {
                    //store info
                    var pos = Position;
                    var rot = Rotation;
                    var lin = LinearVelocity;
                    var ang = AngularVelocity;
                    var sen = IsSensor;
                    var sta = IsStatic;
                    var res = Restitution;
                    //Remove old handler
                    Body.OnCollision -= Body_OnCollision;
                    Body.Dispose();
                    //Build new shape
                    Body = CreateBody(value * Game.Settings.PhysicsScale, pos, rot);
                    Position = pos;
                    Rotation = rot;
                    LinearVelocity = lin;
                    AngularVelocity = ang;
                    IsSensor = sen;
                    IsStatic = sta;
                    Restitution = res;
                    Body.OnCollision += Body_OnCollision;
                }
            }
        }
        #endregion
        public Vector2 DefaultSize { get; set; }

        public Vector2 Scale { get { return Size / DefaultSize; } }
        #region Wrappers for Farseer Settings
        public bool IsStatic
        {
            get
            {
                if (_hasBeenCreated)
                    return Body.IsStatic;
                else
                    return _startIsStatic;
            }
            set
            {
                if (IsStatic != value)
                    RaiseBasicPropertyChange(BasicPropertyChangeEventType.IsStatic, IsStatic, value);

                if (_hasBeenCreated)
                    Body.IsStatic = value;
                else
                    _startIsStatic = value;
            }
        }
        private bool _isSensor;
        public bool IsSensor
        {
            get
            {
                if (_hasBeenCreated)
                    return _isSensor;
                else
                    return _startIsSensor;
            }
            set
            {
                if (IsSensor != value)
                    RaiseBasicPropertyChange(BasicPropertyChangeEventType.IsSensor, IsSensor, value);

                if (_hasBeenCreated)
                {
                    Body.IsSensor = value;
                    _isSensor = value;
                }
                else
                    _startIsSensor = value;
            }
        }

        private float _currentRestitution;
        public float Restitution
        {
            get
            {
                if (_hasBeenCreated)
                    return _currentRestitution;
                else return _startRestitution;
            }
            set
            {
                if (Restitution != value)
                    RaiseBasicPropertyChange(BasicPropertyChangeEventType.Restitution, Restitution, value);

                if (_hasBeenCreated)
                {
                    Body.Restitution = value;
                    _currentRestitution = value;
                }
                else _startRestitution = value;
            }
        }

        #endregion

        #region Places to store settings pre-initialization
        private Vector2 _startPosition;
        private float _startDensity;
        private float _startRestitution;
        private float _startRotation;
        private Vector2 _startVelocity;
        private float _startAngularVelocity;
        private bool _startIsStatic;
        private bool _startIsSensor;
        #endregion
        public GameObject(GameCore game, bool authorized, float density = 1, float restitution = 0.1f, Vector2 position = default(Vector2), float rotation = 0, ushort id = ushort.MaxValue)
        {
            UnsafeDisableEvents();
            ObjectId = id;
            Game = game;

            ColorMask = Color.White;
            _startPosition = position;
            _startRotation = rotation;
            _startDensity = density;
            _startRestitution = restitution;

            if (!game.Authoritative && !authorized)
                game.Logger.Error("Object created without authorization. Type: " + GetType().ToString() + ", ID: " + ObjectId);

            UnsafeEnableEvents();
        }

        private bool _hasBeenCreated;
        internal void Create()
        {
            UnsafeDisableEvents();
            if (_hasBeenCreated)
            {
                Game.Logger.Fatal("Multiple calls to Create()");
            }

            LoadBaseComponents();

            if (Size.X <= 0 || Size.Y <= 0)
                Size = DefaultSize;

            //Create the body in physics space, which is smaller than world space, which is smaller than render space
            Body = CreateBody(Size * Game.Settings.PhysicsScale, _startPosition, _startRotation);
            Body.Restitution = Restitution;
            Body.OnCollision += Body_OnCollision;

            _hasBeenCreated = true;
            //And initialize the object
            Alive = true;
            Position = _startPosition;
            LinearVelocity = _startVelocity;
            AngularVelocity = _startAngularVelocity;
            IsSensor = _startIsSensor;
            IsStatic = _startIsStatic;

            UnsafeEnableEvents();
            //Call the event
            RaiseOnCreated();

            InvokeTrigger("create");


            //And call the internal function
            CreateInternal();
        }

        protected virtual Body CreateBody(Vector2 size, Vector2 position, float rotation)
        {
            if (BaseComponents.Body != null)
            {
                //Load from file
                var b = BodyFactory.CreateCompoundPolygon(Game.World, BaseComponents.Body.GetFixtures(size),
                    _startDensity, position, rotation, BodyType.Dynamic, this);
                b.UserData = this;
                return b;
            }
            else
            {
                //Load rectangle
                var b = BodyFactory.CreateRectangle(Game.World, size.X,
                 size.Y, _startDensity, position, rotation,
                 BodyType.Dynamic, this);
                b.UserData = this;
                return b;
            }
        }

        protected virtual void CreateInternal()
        {

        }

        private bool Body_OnCollision(Fixture fixtureA,
            Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            return CollideInternal((GameObject)fixtureB.Body.UserData, contact);
        }

        protected virtual bool CollideInternal(GameObject other,
            FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            return true;
        }

        /// <summary>
        /// Transforms a point that is relative to the top left of the physics object,
        /// based on the current body position and rotation.
        /// </summary>
        /// <param name="point">The point to transform</param>
        /// <param name="secondaryRotation">The secondary rotation of the object (provided by you).</param>
        /// <param name="transformOnlyBySecondaryRotation">Whether to calculate from both the object rotation and the provided rotation or just the provided rotation.</param>
        /// <returns></returns>
        protected Vector2 TransformPoint(Vector2 point, float secondaryRotation = 0, bool transformOnlyBySecondaryRotation = false, bool ignoreScale = false)
        {
            float cos, sin;
            //Cache for performance
            if (transformOnlyBySecondaryRotation)
            {
                cos = (float)Math.Cos(secondaryRotation);
                sin = (float)Math.Sin(secondaryRotation);
            }
            else
            {
                cos = (float)Math.Cos(Rotation + secondaryRotation);
                sin = (float)Math.Sin(Rotation + secondaryRotation);
            }

            if (ignoreScale)
                point = point - (Size / 2);
            else
                point = (point * Scale) - (Size / 2);
            var rotatedX = point.X * cos - point.Y * sin;
            var rotatedY = point.X * sin + point.Y * cos;
            var centered = new Vector2(rotatedX, rotatedY);
            var transformed = centered + Position;

            return transformed;
        }

        public void Update(GameTime gameTime)
        {
            UpdateComponents(gameTime);
            ActivateAtTimeEmitters();

            if (TimeAlive > Lifespan && Lifespan.TotalMilliseconds > 0) Kill();

            TimeAlive += gameTime.ElapsedGameTime;

            UpdateInternal(gameTime);
        }

        private void UpdateRenderableComponentRotations(GameTime gameTime)
        {
            foreach (var cmp in Components)
            {
                if (cmp.Value.RotationVelocity != 0)
                    cmp.Value.Rotation += cmp.Value.RotationVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        protected virtual void UpdateInternal(GameTime gameTime)
        {

        }
        public void UpdatePostPhysics(GameTime gameTime)
        {
            if (_health <= 0 && CanBeDestroyed && Alive)
                Kill();
            UpdatePostPhysicsInternal(gameTime);
        }

        protected virtual void UpdatePostPhysicsInternal(GameTime gameTime)
        {

        }

        private bool _killedAlready;
        public void Kill(GameObject destroyer = null, bool authorized = false)
        {
            //This line is here as a sort of latency compensation
            //We assume that the client is right but not to the extent
            //that we will remove the game object from the game completely

            //If we're wrong, we don't have to revive an object (assuming we could)
            //which would look really strange from the user's perspective,
            //but instead merely desynchronize the game state a bit. This desync
            //will be corrected automatically the next time a game state update
            //occurs in the networking layer

            //If we're correct, however, it provides a much more pleasant user 
            //experience as things explode when they should           

            UnsafeDisableEvents();
            // if (PostDeathExistenceTime == TimeSpan.Zero)
            //    LinearVelocity = Vector2.Zero;
            UnsafeEnableEvents();

            if (!_killedAlready && (Authoritative || authorized))
                Game.RemoveGameObject(this, destroyer, authorized);

            _killedAlready = true;
        }

        private bool _hasBeenDeleted;
        internal bool Destroy(GameObject destructor = null)
        {
            if (_hasBeenDeleted)
            {
                Game.Logger.Error("Multiple calls to Destroy()");
            }
            Alive = false;
            _hasBeenDeleted = true;
            //Call the event
            RaiseOnDestroyed(destructor);

            InvokeTrigger("destroy");

            DestroyEmitters();

            var mustWaitToDelete = DestroyInternal(destructor) || PostDeathExistenceTime.TotalMilliseconds > 0;
            if (!Body.IsDisposed && !mustWaitToDelete)
                Body.Dispose(); //Kill the physics body if allowed to delete

            if (PostDeathExistenceTime.TotalMilliseconds > 0)
                Game.TimerFactory.CreateTimer(timer => IsDestructionCompleted = true, PostDeathExistenceTime);

            return mustWaitToDelete;
        }
        /// <summary>
        /// Does object destruction logic. Return true if you would like to defer destruction until IsDestructionCompleted = true.
        /// Return false to delete right away
        /// </summary>
        /// <param name="destructor"></param>
        /// <returns></returns>
        protected virtual bool DestroyInternal(GameObject destructor = null)
        {
            return false;
        }

        /// <summary>
        /// Finalizes destruction logic
        /// </summary>
        internal void EndDestruction()
        {
            if (!Body.IsDisposed)
                Body.Dispose(); //Kill the physics body for sure
            Alive = false;
            EndDestructionInternal(); //And call the destructor logic

            InvokeTrigger("destroy_ended");
            RaiseOnDestructionEnded();
        }

        protected virtual void EndDestructionInternal()
        {

        }
        #region Asset Resolver
        protected string ResolveAsset(string assetName)
        {
            if (GetType().IsSubclassOf(typeof(Tanks.Tank)))
                return AssetResolver.ResolveAsset(ContainingModule.Name, assetName, ((Tanks.Tank)this).Player);
            return AssetResolver.ResolveAsset(ContainingModule.Name, assetName);
        }

        protected string ResolveAsset(string modName, string assetName)
        {
            if (GetType().IsSubclassOf(typeof(Tanks.Tank)))
                return AssetResolver.ResolveAsset(modName, assetName, ((Tanks.Tank)this).Player);
            return AssetResolver.ResolveAsset(modName, assetName);
        }
        #endregion

        public override string ToString()
        {
            return $"Class: {base.ToString()}, Id: {ObjectId}, Name: {ReflectionName}";
        }
    }
}
