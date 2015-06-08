using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Rendering
{
    public class RenderableComponent
    {
        public Vector2 Offset { get; set; } = Vector2.Zero;
        /// <summary>
        /// The origin of the rotation, relative to the offset
        /// </summary>
        public Vector2 RotationOrigin { get; set; } = Vector2.Zero;
        public float Rotation { get; set; } = 0;
        public float Scale { get; set; } = 1;
        public Color Mask { get; set; } = Color.White;
        public Vector2 Size { get; set; }
        public bool Visible { get; set; } = true;

        /// <summary>
        /// The layer that the object draws on. Higher layers are drawn last while lower ones are drawn first.
        /// So, 0 is below 1 which is below 2 which...etc.
        /// </summary>
        public int DrawLayer { get; set; }

        //And for rendering, we let the renderer know what we want to show
        public string SheetName { get; set; }
        public string FrameName { get; set; }
    }
}
