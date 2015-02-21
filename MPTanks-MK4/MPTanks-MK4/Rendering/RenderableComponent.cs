using MPTanks_MK4.Helpers;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4.Rendering
{
    class RenderableComponent
    {
        public Sprites.Image CurrentSprite;
        public Sprites.Animation.State CurrentAnimationState;
        public Matrix4 Offset;
        public Color4 Mask;
        public Rectangle Size;
        public bool Visible;
        /// <summary>
        /// The name of the component for access by JS code
        /// </summary>
        public string Name;
    }
}
