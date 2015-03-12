using Engine.Gamemodes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Tanks
{

    public abstract class Tank : GameObject
    {
        public List<Powerups.Powerup> Powerups { get; private set; }
        public Guid PlayerId { get; private set; }
        public Team Team { get; internal set; }
        public Platoon Platoon { get; set; }

        public InputState InputState { get; private set; }

        public int Health { get; protected set; }

        protected abstract float RotationSpeed { get; }
        protected abstract float MovementSpeed { get; }

        public Tank(Guid playerId, GameCore game, bool authorized)
            : base(game, authorized, Settings.TankDensity, 0, default(Vector2), 0)
        {
            PlayerId = playerId;
        }

        private static Dictionary<int, Type> _tankTypes =
            new Dictionary<int, Type>();
        protected static void AddTankType(int id, Type t)
        {
            _tankTypes.Add(id, t);
        }
        public static Tank GetInstanceFromId(int id, Guid playerId, byte[] data)
        {
            return (Tank)Activator.CreateInstance(_tankTypes[id], playerId, data);
        }

        private bool _killed = false;
        protected override bool CollideInternal(GameObject other, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (other.GetType().IsSubclassOf(typeof(Projectiles.Projectile)))
            {
                var o = (Projectiles.Projectile)other;

                if (o.Owner == this)
                    return true;

                Health -= o.DamageAmount;

                o.CollidedWithTank(this);

                if (Health <= 0 && !_killed)
                {
                    TankKilled(o);
                    _killed = true;
                }
                return true;
            }

            return base.CollideInternal(other, contact);
        }


        protected virtual void TankKilled(GameObject destroyer)
        {
            Game.RemoveGameObject(this, destroyer);
        }

        public void Input(InputState state)
        {
            InputState = state;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            var velocity = InputState.MovementSpeed *
                MovementSpeed * (time.ElapsedGameTime.TotalMilliseconds / Settings.MSPerFrame);

            var rotationAmount = InputState.RotationSpeed *
                RotationSpeed * (time.ElapsedGameTime.TotalMilliseconds / Settings.MSPerFrame);

            //calculate the actual movement by axis
            var x = velocity * Math.Sin(Body.Rotation);
            var y = velocity * -Math.Cos(Body.Rotation);

            Body.LinearVelocity = new Vector2((float)x, (float)y);
            Body.AngularVelocity = 0;
            Body.Rotation += (float)rotationAmount;
        }

        public override string ToString()
        {
            return "Player ID: " + PlayerId.ToString();
        }
    }
}
