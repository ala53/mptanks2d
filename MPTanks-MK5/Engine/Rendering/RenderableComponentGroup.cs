using Microsoft.Xna.Framework;
using MPTanks.Engine.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Rendering
{
    public class RenderableComponentGroup 
    {
        public RenderableComponent[] Components { get; set; }
        public Vector2 Offset
        {
            set { foreach (var cmp in Components) cmp.Offset = value; }
        }
        /// <summary>
        /// The origin of the rotation, relative to the offset
        /// </summary>
        public Vector2 RotationOrigin
        {
            set { foreach (var cmp in Components) cmp.RotationOrigin = value; }
        }
        public float Rotation
        {
            set { foreach (var cmp in Components) cmp.Rotation = value; }
        }
        public float RotationVelocity
        {
            set { foreach (var cmp in Components) cmp.RotationVelocity = value; }
        }
        public Vector2 Scale
        {
            set { foreach (var cmp in Components) cmp.Scale = value; }
        }
        public Color Mask
        {
            set { foreach (var cmp in Components) cmp.Mask = value; }
        }
        public Vector2 Size
        {
            set { foreach (var cmp in Components) cmp.Size = value; }
        }
        public bool Visible
        {
            set { foreach (var cmp in Components) cmp.Visible = value; }
        }

        public bool AffectedByObjectColorMask
        {
            set { foreach (var cmp in Components) cmp.IgnoresObjectMask = value; }
        }

        /// <summary>
        /// The layer that the object draws on. Higher layers are drawn last while lower ones are drawn first.
        /// So, 0 is below 1 which is below 2 which...etc.
        /// </summary>
        public int DrawLayer
        {
            set { foreach (var cmp in Components) cmp.DrawLayer = value; }
        }

        //And for rendering, we let the renderer know what we want to show
        public SpriteInfo DefaultSprite
        {
            set { foreach (var cmp in Components) cmp.DefaultSprite = value; }
        }
        public RenderableComponent.RenderableComponentDamageLevel[] DamageLevels
        {
            set { foreach (var cmp in Components) cmp.DamageLevels = value; }
        }
    }
}
