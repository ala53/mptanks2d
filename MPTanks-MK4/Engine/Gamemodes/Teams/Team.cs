using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Gamemodes.Teams
{
    public class Team
    {
        /// <summary>
        /// The total number of tanks on the team
        /// </summary>
        public int PlayersCount { get { return NormalTanksCount + SuperTanksCount; } }
        /// <summary>
        /// The color used to render the team (e.g. red, green, blue, purple, etc.)
        /// </summary>
        public Microsoft.Xna.Framework.Color TeamColor { get; set; }
        /// <summary>
        /// The number of normal tanks allowed for this team
        /// </summary>
        public int NormalTanksCount { get; set; }
        /// <summary>
        /// The number of supertanks allowed for this team
        /// </summary>
        public int SuperTanksCount { get; set; }

        public TankDescriptor[] Tanks { get; set; }

        public class TankDescriptor
        {
            /// <summary>
            /// Whether to allow any type of tank in the slot.
            /// </summary>
            public bool AllowAnyTank { get; set; }
            /// <summary>
            /// The name of the tank type to allow if AllowAnyTank is false.
            /// </summary>
            public string AllowedTankName { get; set; }
        }
    }
}
