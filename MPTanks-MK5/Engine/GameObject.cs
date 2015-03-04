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

        private Dictionary<string, Rendering.RenderableComponent> _components;
        public virtual Dictionary<string, Rendering.RenderableComponent>
            Components
        {
            get
            {
                if (_components == null)
                    _components = new Dictionary<string, Rendering.RenderableComponent>();

                return _components;
            }
        }

        public Vector2 Position
        {
            get { return Body.Position / Settings.PhysicsScale; }
            set { Body.Position = value * Settings.PhysicsScale; }
        }

        /// <summary>
        /// The rotation of the physics object in radians
        /// </summary>
        public float Rotation
        {
            get { return Body.Rotation; }
            set { Body.Rotation = value; }
        }

        public abstract Vector2 Size { get; }

        public GameObject(GameCore game, float density = 1, float bounciness = 0.1f, Vector2 position = default(Vector2), float rotation = 0)
        {
            Body = BodyFactory.CreateRectangle(game.World, Size.X * Settings.PhysicsScale,
                 Size.Y * Settings.PhysicsScale, density, position, rotation,
                 FarseerPhysics.Dynamics.BodyType.Dynamic, this);
            Body.Restitution = bounciness;

            Body.OnCollision += Body_OnCollision;

            Alive = true;
            ObjectId = game.NextObjectId;

            Game = game;
            Rotation = rotation;
            Position = position;
            ColorMask = Color.White;
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

        public void Destroy()
        {
            Alive = false;
            DestroyInternal();
        }

        protected virtual void DestroyInternal()
        {

        }
    }
}
