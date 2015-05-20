using Microsoft.Xna.Framework;
using MPTanks.Engine.Gamemodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine
{
    public class GamePlayer
    {
        public string SelectedTankReflectionName { get; set; }
        public bool HasSelectedTankYet { get; set; }
        public Guid Id { get; set; }
        public object UserData { get; private set; }
        public string[] AllowedTankTypes { get; set; }
        public Tanks.Tank Tank { get; set; }
        public Team Team { get; set; }
        public GameCore Game { get; set; }
        public Vector2 SpawnPoint { get; set; }

        private string _name;
        public string Name
        {
            get
            {
                if (_name == null)
                    return "Player ID: " + Id.ToString();

                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Respawns a player in the game
        /// </summary>
        public void Respawn(bool authorized = false)
        {
            if (Game.GameObjects.Contains(Tank))
                Game.RemoveGameObject(Tank);

            Tank = Tanks.Tank.ReflectiveInitialize(SelectedTankReflectionName, this, this.Team, Game, authorized);
            Tank.Position = SpawnPoint;
            Game.AddGameObject(Tank);
        }
    }
}
