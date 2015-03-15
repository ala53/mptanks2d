using Engine.Tanks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public partial class GameCore
    {
        private List<Guid> _playerIds = new List<Guid>();
        public void AddPlayer(Guid playerId)
        {
            _playerIds.Add(playerId);
        }
        /// <summary>
        /// Kill and remove the player
        /// </summary>
        /// <param name="playerId"></param>
        public void RemovePlayer(Guid playerId)
        {
            _playerIds.Remove(playerId);
        }

        public void InjectPlayerInput(Guid playerId, InputState state)
        {
            if (IsGameRunning)
                Players[playerId].Input(state);
        }

        /// <summary>
        /// Sets up the players for the game. Server only.
        /// </summary>
        private void SetUpGamePlayers()
        {
            Gamemode.MakeTeams(_playerIds.ToArray());

            foreach (var player in _playerIds)
            {
                var type = Gamemode.GetAssignedTankType(player);
                if (type == Gamemodes.PlayerTankType.BasicTank)
                {
                    var tank = new Tanks.BasicTank(player, this, false)
                    {
                        Position = Map.GetSpawnPosition(Gamemode.GetTeamIndex(player)),
                        ColorMask = Gamemode.GetTeam(player).TeamColor,
                    };

                    Gamemode.SetTank(player, tank);
                    AddGameObject(tank);
                    _players.Add(player, tank);
                }
            }
        }

        private bool HasEnoughPlayersToStart()
        {
            return _playerIds.Count >= Gamemode.MinPlayerCount;
        }
    }
}
