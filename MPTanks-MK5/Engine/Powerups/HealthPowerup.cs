using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Powerups
{
    public class HealthPowerup : Powerup
    {
        public HealthPowerup(GameCore game, Vector2 position, bool authorized = false)
            : base(game, authorized, position)
        {

        }
        protected override void AddComponents()
        {
            throw new NotImplementedException();
        }
        public override bool SpawnRandomly
        {
            get { throw new NotImplementedException(); }
        }

        public override void CollidedWithTank(Tanks.Tank tank)
        {
            throw new NotImplementedException();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            throw new NotImplementedException();
        }
    }
}
