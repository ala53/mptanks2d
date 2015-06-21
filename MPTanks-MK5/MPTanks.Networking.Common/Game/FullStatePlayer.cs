using Microsoft.Xna.Framework;
using MPTanks.Engine.Tanks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Game
{
    public class FullStatePlayer
    {
        public bool IsSpectator { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPremium { get; set; }
        public Color UsernameDisplayColor { get; set; }
        public bool TankHasCustomStyle { get; set; }
        public bool HasTank { get; set; }
        public ushort TankObjectId { get; set; }
        public string ClanName { get; set; }
        public string Username { get; set; }
        public Guid Id { get; set; }
        public short TeamId { get; set; }
        public Vector2 SpawnPoint { get; set; }
        public bool HasSelectedTank { get; set; }
        public string TankReflectionName { get; set; }
        public string[] AllowedTankTypes { get; set; }
        public NetworkPlayer PlayerObject { get; set; }
        public InputState Input { get; set; }
    }
}
