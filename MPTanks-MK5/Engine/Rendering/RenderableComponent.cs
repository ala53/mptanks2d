using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Rendering
{
    public class RenderableComponent
    {
        public Vector2 Offset = Vector2.Zero;
        /// <summary>
        /// The origin of the rotation, relative to the offset
        /// </summary>
        public Vector2 RotationOrigin = Vector2.Zero;
        public float Rotation = 0;
        public float Scale = 1;
        public Color Mask = Color.White;
        public Vector2 Size;
        public bool Visible = true;

        /// <summary>
        /// The layer that the object draws on. Higher layers are drawn last while lower ones are drawn first.
        /// So, 0 is below 1 which is below 2 which...etc.
        /// </summary>
        public int DrawLayer;

        //And for rendering, we let the engine know what we want to show
        public string SpriteSheetName;
        public string AssetName;
    }
}
