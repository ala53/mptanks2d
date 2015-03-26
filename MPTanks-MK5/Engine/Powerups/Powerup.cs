using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Powerups
{
    public abstract class Powerup : GameObject
    {
        /// <summary>
        /// The public constructor for powerups. Due to using reflection, you *MUST*
        /// maintain this signature for derived classes or the PowerupManager will
        /// not work because it will not be able to instantiate a copy of your powerup.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="position"></param>
        public Powerup(GameCore game, bool authorized, Vector2 position)
            : base(game, authorized, 100000, 0, position, 0)
        {
            //Powerups don't need to collide with things...
            Body.IsSensor = true;
            Body.IsStatic = true;
        }

        /// <summary>
        /// Whether the powerup spawns randomly or must be spawned by a tank
        /// </summary>
        public abstract bool SpawnRandomly { get; }
        protected override bool CollideInternal(GameObject other, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (other.GetType().IsSubclassOf(typeof(Tanks.Tank)) && Alive)
            {
                CollidedWithTank((Tanks.Tank)other);
            }
            return true;
        }
        public abstract void CollidedWithTank(Tanks.Tank tank);
    }
}
