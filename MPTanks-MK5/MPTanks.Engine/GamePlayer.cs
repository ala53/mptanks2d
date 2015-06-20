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
        public virtual bool IsSpectator { get; set; }
        public virtual string SelectedTankReflectionName { get; set; }
        public virtual bool HasSelectedTankYet { get; set; }
        public virtual Guid Id { get; set; }
        public virtual string[] AllowedTankTypes { get; set; }
        public virtual Tanks.Tank Tank { get; set; }
        public virtual Team Team { get; set; }
        public virtual GameCore Game { get; set; }
        public virtual Vector2 SpawnPoint { get; set; }

        private string _name;
        public virtual string DisplayName
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
        public virtual Tanks.Tank Respawn(bool authorized = false)
        {
            if (Game.GameObjects.Contains(Tank))
                Game.RemoveGameObject(Tank);

            Tank = Tanks.Tank.ReflectiveInitialize(SelectedTankReflectionName, this, Game, authorized);
            Tank.Position = SpawnPoint;
            Game.AddGameObject(Tank);
            return Tank;
        }
    }
}
