using Microsoft.Xna.Framework;
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
        public string SpriteName { get; set; }
        public string AssetName { get; set; }

        public int[] TeamIds { get; set; }
        private static int[] _allTeams = new[] { 0 };
        private static int[] _noTeams = new int[] { };
        public bool ShowForAllTeams
        {
            get { return TeamIds.Contains(0); }
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
