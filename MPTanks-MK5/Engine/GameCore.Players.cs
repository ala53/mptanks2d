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
        public GamePlayer AddPlayer(Guid playerId)
        {
            return AddPlayer(new GamePlayer()
            {
                Id = playerId,
                HasSelectedTankYet = false,
                Game = this
            });
        }

        public GamePlayer AddPlayer<GamePlayer>(GamePlayer player) where GamePlayer : Engine.GamePlayer
        {
            if (!_playerIds.Contains(player.Id))
            {
                _playerIds.Add(player.Id);
                player.Game = this;
                _playersById.Add(player.Id, player);
            }

            if (Authoritative)
            {
                if (Running && !Gamemode.HotJoinEnabled)
                    player.IsSpectator = true; //Force spectator
                else if (Running && Gamemode.HotJoinCanPlayerJoin(player))
                {
                    player.IsSpectator = false;
                    //Let them join - first find the team and size the player list correctly
                    player.Team = Gamemode.HotJoinGetPlayerTeam(player);
                    var newPlayerArray = new GamePlayer[player.Team.Players.Length + 1];
                    Array.Copy(player.Team.Players, newPlayerArray, player.Team.Players.Length);
                    newPlayerArray[newPlayerArray.Length - 1] = player;
                    player.Team.Players = newPlayerArray;

                    //Then tanks
                    player.AllowedTankTypes = Gamemode.HotJoinGetAllowedTankTypes(player);
                }
                player.HasSelectedTankYet = false;
            }
            return player;
        }
        /// <summary>
        /// Kill and remove the player
        /// </summary>
        /// <param name="playerId"></param>
        public void RemovePlayer(Guid playerId)
        {
            _playerIds.Remove(playerId);
            if (_playersById.ContainsKey(playerId))
            {
                RemoveGameObject(_playersById[playerId].Tank);
                _playersById.Remove(playerId);
            }
        }

        /// <summary>
        /// Kill and remove the player
        /// </summary>
        /// <param name="playerId"></param>
        public void RemovePlayer(GamePlayer player)
        {
            _playerIds.Remove(player.Id);
            if (_playersById.ContainsKey(player.Id))
            {
                RemoveGameObject(_playersById[player.Id].Tank);
                _playersById.Remove(player.Id);
            }
        }

        public GamePlayer FindPlayer(Guid playerId)
        {
            return PlayersById.ContainsKey(playerId) ? PlayersById[playerId] : null;
        }

        public void InjectPlayerInput(Guid playerId, InputState state)
        {
            if (Running)
                PlayersById[playerId].Tank.InputState = state;
        }

        public void InjectPlayerInput(GamePlayer player, InputState state)
        {
            if (Running)
                player.Tank.InputState = state;
        }

        public bool CheckPlayerIsAlive(GamePlayer player)
        {
            return player.Tank != null && player.Tank.Alive;
        }

        public bool CheckPlayerHasTank(GamePlayer player)
        {
            return player.Tank != null;
        }

        /// <summary>
        /// Sets up the players for the game. Server only.
        /// </summary>
        private void SetUpGamePlayers()
        {
            Gamemode.MakeTeams(Players.ToArray());

            foreach (var player in Players)
            {
                if (player.IsSpectator) continue; //Ignore spectators

                if (!player.HasSelectedTankYet)
                    player.SelectedTankReflectionName = Gamemode.DefaultTankTypeReflectionName;

                var tank = Tank.ReflectiveInitialize(player.SelectedTankReflectionName, player, this, false);
                player.SpawnPoint = Map.GetSpawnPosition(Gamemode.GetTeamIndex(player));

                tank.Position = player.SpawnPoint;
                tank.ColorMask = player.Team.TeamColor;

                AddGameObject(tank);
                player.Tank = tank;
            }
        }

        private bool HasEnoughPlayersToStart()
        {
            return _playerIds.Count >= Gamemode.MinPlayerCount;
        }
    }
}
