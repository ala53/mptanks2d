using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Powerups
{
    /// <summary>
    /// A class that manages the spawning of power ups for the game
    /// </summary>
    public class PowerupManager
    {
        public static readonly Type[] PowerupTypes = { 
            typeof(HealthPowerup)
        };
        public Type[] AllowedPowerupTypes { get; set; }
        public float PowerupSpawnsPerSecond { get; set; }

        private GameCore _game;
        public PowerupManager(GameCore game)
        {
            AllowedPowerupTypes = PowerupTypes;
            PowerupSpawnsPerSecond = 0.1f; //1 every 10 sec
            _game = game;
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
