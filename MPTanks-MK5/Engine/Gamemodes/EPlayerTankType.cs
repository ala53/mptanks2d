using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Gamemodes
{
    /// <summary>
    /// All allowed tank types for a player.
    /// </summary>
    [Flags]
    public enum PlayerTankType : ulong
    {
        NormalTank = 1 << 0,
        SuperTank = 1 << 1,
        BasicTank = 1 << 2,
        ArmoredSuperTank = 1 << 3
    }
}
