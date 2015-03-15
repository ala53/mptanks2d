using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Gamemodes
{
    public abstract class Gamemode
    {
        public GameCore Game { get; private set; }
        public abstract bool GameEnded { get; }
        public virtual Team WinningTeam { get { return Team.Null; } }
        public abstract Team[] Teams { get; }
        public abstract int MinPlayerCount { get; }

        /// <summary>
        /// An event that is fired when the gamemode updates and changes state
        /// </summary>
        public event EventHandler<GamemodeChangedArgs> OnGamemodeStateChanged;

        public class GamemodeChangedArgs
        {
            public byte[] NewStateData;
        }
        public Gamemode(byte[] serverState = null)
        {

        }

        internal void SetGame(GameCore game)
        {
            Game = game;
        }

        /// <summary>
        /// Puts all of the tanks on teams
        /// </summary>
        /// <param name="tanks"></param>
        public abstract void MakeTeams(Guid[] playerIds);

        /// <summary>
        /// Gets the tank type for a single player.
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public abstract PlayerTankType GetAssignedTankType(Guid playerId);

        /// <summary>
        /// Notifies the Gamemode that the game has started. So, it can do whatever it wants 
        /// related to that.
        /// </summary>
        public abstract void StartGame();
        /// <summary>
        /// Lets the game mode run its internal logic for players
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Sets the tank associated to a specific player.
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="tank"></param>
        public virtual void SetTank(Guid playerId, Tanks.Tank tank)
        {
            foreach (var team in Teams)
                for (var i = 0; i < team.Players.Length; i++)
                    if (team.Players[i].PlayerId == playerId)
                    {
                        //Simply do an in place replace and assign the new tank to 
                        var tmp = team.Players[i];
                        tmp.Tank = tank;
                        tank.Team = team;
                        team.Players[i] = tmp;
                        return;
                    }

        }

        public Team GetTeam(Guid playerId)
        {
            foreach (var team in Teams)
                for (var i = 0; i < team.Players.Length; i++)
                    if (team.Players[i].PlayerId == playerId)
                        return team;

            return null;
        }

        public int GetTeamIndex(Guid playerId)
        {
            var t = GetTeam(playerId);
            for (var i = 0; i < Teams.Length; i++)
                if (Teams[i] == t)
                    return i;

            return int.MaxValue;
        }

        public virtual void ReceiveStateChange(byte[] data)
        {
        }

        /// <summary>
        /// Converts the gamemode's state to a network representation
        /// </summary>
        /// <returns></returns>
        public abstract byte[] Networkify();
    }
}
