using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Client.Backend.Renderer.Assets.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Renderer
{
    struct DrawableObject
    {
        public Vector2 Size { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 RotationOrigin { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }
        public Sprite Texture { get; set; }
        public Color Mask { get; set; }
    }
    //A 64 byte structure
    struct GPUDrawable : IVertexType
    {
        public Vector2 Vertex;
        public Vector2 RotationOrigin;
        public Vector2 Scale;
        public Vector2 Size;
        public float Rotation;
        public Color Mask;
        public Vector4 SpritePosition;
        public Vector2 TextureCoordinates;

        private static VertexDeclaration _decl = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
            new VertexElement(8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(16, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1),
            new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 2),
            new VertexElement(32, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 3),
            new VertexElement(36, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(40, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 4),
            new VertexElement(56, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 5));
        public static VertexDeclaration Declaration => _decl;

        public VertexDeclaration VertexDeclaration
        {
            get
            {
                return _decl;
            }
        }
    }
}
