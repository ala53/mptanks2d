using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using MPTanks.Engine.Rendering;
using System;

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
        public int ObjectId { get; private set; }
        public Body Body { get; private set; }
        public GameCore Game { get; private set; }
        public bool Alive { get; private set; }
        public bool Authoritative { get { return Game.Authoritative; } }
        /// <summary>
        /// The number of milliseconds that the object has been alive
        /// </summary>
        public float TimeAliveMs { get; private set; }
        /// <summary>
        /// The lifespan in milliseconds of the object
        /// </summary>
        public float LifespanMs { get; private set; }
        public float PostDeathExistenceTime { get; private set; }
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
                    RaiseBasicPropertyChange(BasicPropertyChangeEventType.Position);

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
                    RaiseBasicPropertyChange(BasicPropertyChangeEventType.Rotation);

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
                    RaiseBasicPropertyChange(BasicPropertyChangeEventType.LinearVelocity);

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
                    RaiseBasicPropertyChange(BasicPropertyChangeEventType.AngularVelocity);

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
                if (Size != value)
                    RaiseBasicPropertyChange(BasicPropertyChangeEventType.Size);

                if (_hasBeenCreated)
                {
                    //Build new shape
                    var vertices = new FarseerPhysics.Common.Vertices(new[] {
                        new Vector2(-value.X / 2, -value.Y /2),
                        new Vector2(value.X / 2, -value.Y /2),
                        new Vector2(value.X / 2, value.Y /2),
                        new Vector2(-value.X / 2, value.Y /2)
                    });
                    var rect = new FarseerPhysics.Collision.Shapes.PolygonShape(
                        vertices, Body.FixtureList[0].Shape.Density);

                    //Destroy current fixture
                    Body.DestroyFixture(Body.FixtureList[0]);
                    //And add the new one
                    Body.CreateFixture(rect, this);
                }

                //And set the internal note
                _size = value;
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
                if (_hasBeenCreated)
                {
                    Body.IsSensor = value;
                    _isSensor = value;
                }
                else
                    _startIsSensor = value;
            }
        }
        #endregion

        #region Places to store settings pre-initialization
        private Vector2 _startPosition;
        private float _startDensity;
        private float _startBounciness;
        private float _startRotation;
        private Vector2 _startVelocity;
        private float _startAngularVelocity;
        private bool _startIsStatic;
        private bool _startIsSensor;
        #endregion
        public GameObject(GameCore game, bool authorized, float density = 1, float bounciness = 0.1f, Vector2 position = default(Vector2), float rotation = 0, int id = -1)
        {
            if (id == -1)
                ObjectId = game.NextObjectId;
            else
                ObjectId = id;
            Game = game;

            ColorMask = Color.White;
            _startPosition = position;
            _startRotation = rotation;
            _startDensity = density;
            _startBounciness = bounciness;

            LoadBaseComponents();

            if (!game.Authoritative && !authorized)
                game.Logger.Error("Object Created without authorization. Type: " + this.GetType().ToString() + ", ID: " + ObjectId);
        }

        private bool _hasBeenCreated;
        internal void Create()
        {
            if (_hasBeenCreated)
            {
                Game.Logger.Fatal("Multiple calls to Create()");
            }

            if (Size == Vector2.Zero)
                Size = DefaultSize;

            _hasBeenCreated = true;
            //Create the body in physics space, which is smaller than world space, which is smaller than render space
            Body = BodyFactory.CreateRectangle(Game.World, Size.X * Game.Settings.PhysicsScale,
                 Size.Y * Game.Settings.PhysicsScale, _startDensity, Vector2.Zero, _startRotation,
                 BodyType.Dynamic, this);
            Body.Restitution = _startBounciness;
            Body.OnCollision += Body_OnCollision;
            //And initialize the object
            Alive = true;
            Position = _startPosition;
            LinearVelocity = _startVelocity;
            AngularVelocity = _startAngularVelocity;
            IsSensor = _startIsSensor;
            IsStatic = _startIsStatic;

            //Call the event
            RaiseOnCreated();

            ActivateOnCreateEmitters();

            //And call the internal function
            CreateInternal();
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
        protected Vector2 TransformPoint(Vector2 point, float secondaryRotation = 0, bool transformOnlyBySecondaryRotation = false)
        {
            float cos, sin;
            //Cache for performance
            if (transformOnlyBySecondaryRotation)
            {
                cos = (float)Math.Cos(secondaryRotation);
                sin = (float)-Math.Sin(secondaryRotation);
            }
            else
            {
                cos = (float)Math.Cos(Rotation + secondaryRotation);
                sin = (float)-Math.Sin(Rotation + secondaryRotation);
            }

            point = point - (Size / 2);
            var rotatedX = point.X * cos + point.Y * sin;
            var rotatedY = point.X * sin + point.Y * cos;
            var centered = new Vector2(rotatedX, rotatedY);
            var transformed = centered + Position;

            return transformed;
        }

        public void Update(GameTime gameTime)
        {
            UpdateEmitters(gameTime);

            if (TimeAliveMs > LifespanMs && LifespanMs != 0) Kill();

            TimeAliveMs += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            ActivateAtTimeEmitters();

            UpdateRenderableComponentRotations(gameTime);

            UpdateInternal(gameTime);
        }

        private void UpdateRenderableComponentRotations(GameTime gameTime)
        {
            foreach (var cmp in Components)
            {
                if (cmp.Value.RotationVelocity > 0)
                    cmp.Value.Rotation += cmp.Value.RotationVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        protected virtual void UpdateInternal(GameTime gameTime)
        {

        }
        public void UpdatePostPhysics(GameTime gameTime)
        {
            UpdatePostPhysicsInternal(gameTime);
        }

        protected virtual void UpdatePostPhysicsInternal(GameTime gameTime)
        {

        }

        private bool _killedAlready;
        public void Kill(GameObject destroyer = null, bool authorized = false)
        {
            if (!_killedAlready)
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

            ActivateOnDestroyEmitters();

            DestroyEmitters();

            var mustWaitToDelete = DestroyInternal(destructor) || PostDeathExistenceTime > 0; 
            if (!Body.IsDisposed && !mustWaitToDelete)
                Body.Dispose(); //Kill the physics body if allowed to delete

            if (PostDeathExistenceTime > 0)
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

            ActivateOnDestroyEndedEmitters();
            RaiseOnDestructionEnded();
        }

        protected virtual void EndDestructionInternal()
        {

        }
        #region Asset Resolver
        protected string ResolveAsset(string assetName)
        {
            if (GetType().IsSubclassOf(typeof(Tanks.Tank)))
                return SpriteSheetLookupHelper.ResolveAsset(ContainingModule.Name, assetName, ((Tanks.Tank)this).Player);
            return SpriteSheetLookupHelper.ResolveAsset(ContainingModule.Name, assetName);
        }

        protected string ResolveAsset(string modName, string assetName)
        {
            if (GetType().IsSubclassOf(typeof(Tanks.Tank)))
                return SpriteSheetLookupHelper.ResolveAsset(modName, assetName, ((Tanks.Tank)this).Player);
            return SpriteSheetLookupHelper.ResolveAsset(modName, assetName);
        }
        #endregion
        public override string ToString()
        {
            return $"Class: {base.ToString()} Id: {ObjectId}, Name: {ReflectionName}"; 
        }
    }
}
