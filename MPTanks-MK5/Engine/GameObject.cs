using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public abstract class GameObject
    {
        public Color ColorMask { get; set; }
        public int ObjectId { get; private set; }
        public FarseerPhysics.Dynamics.Body Body { get; private set; }
        public GameCore Game { get; private set; }
        public bool Alive { get; set; }
        public bool IsDestructionCompleted { get; protected set; }

        private Dictionary<string, Rendering.RenderableComponent> _components;
        public virtual Dictionary<string, Rendering.RenderableComponent>
            Components
        {
            get
            {
                if (_components == null)
                {
                    _components = new Dictionary<string, Rendering.RenderableComponent>();
                    //This is the first access so we call the component constructor
                    AddComponents();
                }

                return _components;
            }
        }

        public Vector2 Position
        {
            get
            {
                if (_hasBeenCreated)
                    return Body.Position / Settings.PhysicsScale;
                else
                    return _startPosition;
            }
            set
            {
                if (_hasBeenCreated)
                    Body.Position = value * Settings.PhysicsScale;
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
                    return Body.LinearVelocity / Settings.PhysicsScale;
                else
                    return _startVelocity;
            }
            set
            {
                if (_hasBeenCreated)
                    Body.LinearVelocity = value * Settings.PhysicsScale;
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
                if (_hasBeenCreated)
                    Body.AngularVelocity = value;
                else
                    _startAngularVelocity = value;
            }
        }

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

        private Vector2 _size;
        public Vector2 Size
        {
            get { return _size; }
            protected set
            {
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

        private Vector2 _startPosition;
        private float _startDensity;
        private float _startBounciness;
        private float _startRotation;
        private Vector2 _startVelocity;
        private float _startAngularVelocity;
        private bool _startIsStatic;
        private bool _startIsSensor;

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

            if (!game.Authoritative && !authorized)
                game.Logger.Error("Object Created without authorization. Type: " + this.GetType().ToString() + ", ID: " + ObjectId);
        }

        private bool _hasBeenCreated;
        internal void Create()
        {
            if (_hasBeenCreated)
            {
                Game.Logger.Error("Multiple calls to Create()");
            }
            _hasBeenCreated = true;
            //Create the body in physics space, which is smaller than world space, which is smaller than render space
            Body = BodyFactory.CreateRectangle(Game.World, Size.X * Settings.PhysicsScale,
                 Size.Y * Settings.PhysicsScale, _startDensity, Vector2.Zero, _startRotation,
                 FarseerPhysics.Dynamics.BodyType.Dynamic, this);
            Body.Restitution = _startBounciness;
            Body.OnCollision += Body_OnCollision;
            //And initialize the object
            Alive = true;
            Position = _startPosition;
            LinearVelocity = _startVelocity;
            AngularVelocity = _startAngularVelocity;
            IsSensor = _startIsSensor;
            IsStatic = _startIsStatic;

            //And call the internal function
            CreateInternal();
        }

        /// <summary>
        /// Called when the object is supposed to add the rendering components. Usually 
        /// on the first access but this is not guaranteed.
        /// </summary>
        protected abstract void AddComponents();

        protected virtual void CreateInternal()
        {

        }

        private bool Body_OnCollision(FarseerPhysics.Dynamics.Fixture fixtureA,
            FarseerPhysics.Dynamics.Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
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
        /// <param name="point"></param>
        /// <returns></returns>
        protected Vector2 TransformPoint(Vector2 point, float componentRotation = 0, bool useOnlyComponentRot = false)
        {
            float cos, sin;
            //Cache for performance
            if (useOnlyComponentRot)
            {
                cos = (float)Math.Cos(componentRotation);
                sin = (float)-Math.Sin(componentRotation);
            }
            else
            {
                cos = (float)Math.Cos(Rotation + componentRotation);
                sin = (float)-Math.Sin(Rotation + componentRotation);
            }

            point = point - (Size / 2);
            var rotatedX = point.X * cos + point.Y * sin;
            var rotatedY = point.X * sin + point.Y * cos;
            var centered = new Vector2(rotatedX, rotatedY);
            var transformed = centered + Position;

            return transformed;
        }

        abstract public void Update(GameTime time);

        private bool _hasBeenDeleted;
        internal bool Destroy(GameObject destructor = null)
        {
            if (_hasBeenDeleted)
            {
                Game.Logger.Error("Multiple calls to Destroy()");
            }
            _hasBeenDeleted = true;
            var canDeleteRightAway = DestroyInternal(destructor);
            if (!Body.IsDisposed && canDeleteRightAway == false)
                Body.Dispose(); //Kill the physics body if allowed to delete

            return canDeleteRightAway;
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
            OnRemovedFromGame(); //And call the destructor logic
        }

        protected virtual void OnRemovedFromGame()
        {

        }

        public event EventHandler<Core.Events.Types.GameObjects.StateChanged> OnStateChanged;

        public void RaiseStateChangeEvent(byte[] newStateData)
        {
            if (OnStateChanged == null || !Game.Authoritative || newStateData == null || newStateData.Length == 0)
                return;

            OnStateChanged(this, new Core.Events.Types.GameObjects.StateChanged()
            {
                GameObject = this,
                StateData = newStateData
            });
        }
    }
}
