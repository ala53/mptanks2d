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
        public InputState InputState { get; private set; }

        public int Health { get; protected set; }

        protected abstract float RotationSpeed { get; }
        protected abstract float MovementSpeed { get; }

        public Tank(Guid playerId, GameCore game, bool authorized)
            : base(game, authorized, Settings.TankDensity, 0, default(Vector2), 0)
        {
            PlayerId = playerId;
        }

        private bool _killed = false;
        protected override bool CollideInternal(GameObject other, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (other.GetType().IsSubclassOf(typeof(Projectiles.Projectile)) && other.Alive)
            {
                var o = (Projectiles.Projectile)other;

                if (!IsDamageAllowed(o.Owner))
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

        private bool IsDamageAllowed(Tank tank)
        {
            if (Game.FriendlyFireEnabled)
                return true;
            if (tank.Team != Team)
                return true;
            return false;
        }


        protected virtual void TankKilled(GameObject destroyer)
        {
            if (Game.Authoritative)
                Game.RemoveGameObject(this, destroyer);
        }

        public void Input(InputState state)
        {
            InputState = state;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            var velocity = InputState.MovementSpeed *
                MovementSpeed;

            var rotationAmount = InputState.RotationSpeed *
                RotationSpeed * (time.ElapsedGameTime.TotalMilliseconds / 16.66666);

            //calculate the actual movement by axis
            var x = velocity * Math.Sin(Rotation);
            var y = velocity * -Math.Cos(Rotation);

            LinearVelocity = new Vector2((float)x, (float)y);
            AngularVelocity = 0;
            Rotation += (float)rotationAmount;
        }

        public override string ToString()
        {
            return "Player ID: " + PlayerId.ToString();
        }
    }
}
