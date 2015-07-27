using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Client.Backend.Renderer.Assets.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    //A 72 byte structure
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct GPUDrawable : IVertexType
    {
        public Vector2 Position;
        public Vector2 Offset;
        public Vector2 Size;
        public Vector2 RotationOrigin;
        public Vector2 Scale;
        public float Rotation;
        public Color Color;
        public Vector4 TextureBounds;
        public Vector2 TexCoord;

        private static VertexDeclaration _decl = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(16, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1),
            new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 2),
            new VertexElement(32, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 3),
            new VertexElement(40, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 4),
            new VertexElement(44, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(48, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 5),
            new VertexElement(64, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 6));
        public static VertexDeclaration VertexDeclaration => _decl;

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get
            {
                return _decl;
            }
        }

        public GPUDrawable(Vector2 vertex, Vector2 offset, Vector2 size, Vector2 rotationOrigin, Vector2 scale, 
            float rotation, Color color, Vector4 texBounds, Vector2 texCoord)
        {
            Position = vertex;
            Offset = offset;
            Size = size;
            Scale = scale;
            RotationOrigin = rotationOrigin;
            Rotation = rotation;
            Color = color;
            TextureBounds = texBounds;
            TexCoord = texCoord;
        }
    }
}
