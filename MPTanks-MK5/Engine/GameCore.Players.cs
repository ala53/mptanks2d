using MPTanks.Engine.Tanks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine
{
    public partial class GameCore
    {
        private List<Guid> _playerIds = new List<Guid>();
        public void AddPlayer(Guid playerId)
        {
            if (!_playerIds.Contains(playerId)) //Ignore existing
                _playerIds.Add(playerId);
        }
        /// <summary>
        /// Kill and remove the player
        /// </summary>
        /// <param name="playerId"></param>
        public void RemovePlayer(Guid playerId)
        {
            _playerIds.Remove(playerId);
            if (_players.ContainsKey(playerId))
            {
                RemoveGameObject(_players[playerId]);
                _players.Remove(playerId);
            }
        }

        public void InjectPlayerInput(Guid playerId, InputState state)
        {
            if (IsGameRunning)
                Players[playerId].Input(state);
        }

        public bool CheckPlayerIsAlive(Guid playerId)
        {
            return Players.ContainsKey(playerId) && Players[playerId].Alive;
        }

        public bool CheckPlayerHasTank(Guid playerId)
        {
            return Players.ContainsKey(playerId);
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
                    var tank = MPTanks.Engine.Tanks.Tank.ReflectiveInitialize("BasicTankMP", player, this, false);
                    tank.Position = Map.GetSpawnPosition(Gamemode.GetTeamIndex(player));
                    tank.ColorMask = Gamemode.GetTeam(player).TeamColor;

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
