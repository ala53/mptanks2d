using Microsoft.Xna.Framework;
using MPTanks.Engine.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Rendering.Animations
{
    /// <summary>
    /// An abstract description of an Animation / specific
    /// set of images that should be rendered on screen at a 
    /// specific time. 
    /// </summary>
    public class Animation: IHasSpriteInfo
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public float Rotation { get; set; }
        public Vector2 RotationOrigin { get; set; }
        public SpriteInfo SpriteInfo { get; set; }
        public Action<Animation> CompletionCallback { get; private set; }
        public int DrawLayer { get; set; }
        public Color Mask { get; set; }

        public Animation(SpriteInfo spriteInfo, Vector2 center, Vector2 size, Color? mask = null, Vector2? rotationOrigin = null, Action<Animation> callback = null, int drawLayer = 0)
        {
            Position = center;
            SpriteInfo = spriteInfo;
            Size = size;
            CompletionCallback = callback;
            DrawLayer = drawLayer;
            Mask = mask ?? Color.White;
            RotationOrigin = rotationOrigin ?? Size / 2;
        }
    }
}
