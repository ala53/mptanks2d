using Microsoft.Xna.Framework;
using MPTanks.Engine.Gamemodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Rendering.Lighting
{
    public class Light
    {
        public float Intensity { get; set; }
        public Color Color { get; set; }
        public Vector2 PositionCenter { get; set; }
        public Vector2 Size { get; set; }
        public SpriteInfo SpriteInfo { get; set; }

        public short[] TeamIds { get; set; }
        private static short[] _allTeams = new short[] { Team.Null.TeamId };
        private static short[] _noTeams = new short[] { };
        public bool ShowForAllTeams
        {
            get { return TeamIds.Contains(Team.Null.TeamId); }
            set { TeamIds = value ? _allTeams : _noTeams; }
        }

        public Light()
        {
            Intensity = 1; //Max
            Color = Color.White; //White light
            ShowForAllTeams = true;
        }
    }
}
