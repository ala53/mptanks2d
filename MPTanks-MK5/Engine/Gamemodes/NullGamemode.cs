using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MPTanks.Modding;

namespace MPTanks.Engine.Gamemodes
{
    [Gamemode(DefaultTankTypeReflectionName = "BasicTankMP", 
        MinPlayersCount = 0, HotJoinEnabled = false)]
    public class NullGamemode : Gamemode
    {

        public NullGamemode()
        {

        }
        public override void MakeTeams(GamePlayer[] players)
        {
            Teams = new[]
            {
                new Team()
                {
                    Objective = "NULL",
                    Players = players,
                    TeamColor = Color.White,
                    TeamId = 15,
                    TeamName = "NULL"
                }
            };
        }

        public override string[] GetPlayerAllowedTankTypes(GamePlayer player)
        {
            return Tanks.Tank.GetAllTankTypes().ToArray();
        }

        public override bool CheckPlayerTankSelectionValid(GamePlayer player, string tankType)
        {
            player.SelectedTankReflectionName = tankType;
            return true;
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
