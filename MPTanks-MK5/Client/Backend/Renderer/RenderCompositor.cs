using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Client.Backend.Renderer.Assets.Sprites;
using MPTanks.Engine.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Renderer
{
    class RenderCompositor : IDisposable
    {
        private GameCoreRenderer _renderer;
        private GraphicsDevice _gd;
        private Effect _shadowEffect;
        private Effect _drawEffect;
        private Matrix _projection;
        private Vector2 _shadowOffset = new Vector2(0.5f, 0.3f);
        private Color _shadowColor = new Color(50, 50, 50, 100);
        private DynamicVertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;
        private Texture2D _tx;
        public RenderCompositor(GameCoreRenderer worldRenderer)
        {
            _renderer = worldRenderer;
            _gd = _renderer.Client.GraphicsDevice;
            _shadowEffect = worldRenderer.Client.Content.Load<Effect>("shadowRenderer");
            _drawEffect = worldRenderer.Client.Content.Load<Effect>("componentRenderer");
            _vertexBuffer = new DynamicVertexBuffer(_gd, GPUDrawable.VertexDeclaration, 1000, BufferUsage.None);

            _tx = new Texture2D(_gd, 1, 1);
            _tx.SetData(new[] { Color.White });
            //Generate indices
            ushort[] indices = new ushort[ushort.MaxValue];

            //6 to 4 ratio
            //0, 1, 2 | 2, 3, 1

            for (ushort i = 0; i < indices.Length - 8; i += 6)
            {
                indices[i] = (ushort)(0 + i);
                indices[i + 1] = (ushort)(1 + i);
                indices[i + 2] = (ushort)(2 + i);

                indices[i + 3] = (ushort)(2 + i);
                indices[i + 4] = (ushort)(3 + i);
                indices[i + 5] = (ushort)(0 + i);
            }

            _indexBuffer = new IndexBuffer(_gd, IndexElementSize.SixteenBits, ushort.MaxValue, BufferUsage.WriteOnly);
            _indexBuffer.SetData(indices);
        }
        public void SetView(RectangleF viewRect)
        {
            _projection = Matrix.CreateOrthographicOffCenter(viewRect.Left, viewRect.Right, viewRect.Bottom, viewRect.Top, -1, float.MaxValue);
        }
        public void SetShadowParameters(Vector2 offset, Color color)
        {
            _shadowOffset = offset;
            _shadowColor = color;
        }
        private Dictionary<int, RenderCompositorLayer> _layers
        { get; set; }
        = new Dictionary<int, RenderCompositorLayer>();
        private List<RenderCompositorLayer> _sorted
        { get; set; }
        = new List<RenderCompositorLayer>();
        public void AddDrawable(DrawableObject drawable, int layer)
        {
            AddDrawable(ref drawable, layer);
        }
        public void AddDrawable(ref DrawableObject drawable, int layerNumber)
        {
            var layer = GetOrAddLayer(layerNumber);
            layer.AddObject(drawable);
        }

        private RenderCompositorLayer GetOrAddLayer(int number)
        {
            if (_layers.ContainsKey(number)) return _layers[number];

            //Create layer
            var layer = new RenderCompositorLayer(number);
            _layers.Add(number, layer);
            int insertPoint = 0;
            for (int i = 0; i < _sorted.Count; i++)
            {
                insertPoint = i;
                if (_sorted[i].LayerNumber > layer.LayerNumber)
                    break;
            }
            _sorted.Insert(insertPoint, layer);
            return layer;
        }

        public void Draw(GameTime gameTime)
        {
            _gd.RasterizerState = RasterizerState.CullNone;
            _gd.BlendState = BlendState.NonPremultiplied;

            for (var i = _sorted.Count - 1; i >= 0; i--)
            {
                var layer = _sorted[i];
                foreach (var kvp in layer.SpriteSorted)
                {

                    if (kvp.Value.Count == 0) continue;
                    //Check if the buffer is large enough
                    if (kvp.Value.Count > _vertexBuffer.VertexCount)
                    {
                        //New buffer, larger
                        _vertexBuffer.Dispose();
                        _vertexBuffer = new DynamicVertexBuffer(_gd, GPUDrawable.VertexDeclaration,
                            kvp.Value.Count, BufferUsage.WriteOnly);
                    }

                    //Upload vertices
                    _vertexBuffer.SetData(kvp.Value.InternalArray, 0, kvp.Value.Count, SetDataOptions.Discard);
                    _gd.SetVertexBuffer(_vertexBuffer);

                    foreach (var technique in _shadowEffect.Techniques)
                        foreach (var pass in technique.Passes)
                        {
                            _shadowEffect.Parameters["txt"].SetValue(kvp.Key.Texture);
                            _shadowEffect.Parameters["view"].SetValue(Matrix.Identity);
                            _shadowEffect.Parameters["projection"].SetValue(_projection);
                            _shadowEffect.Parameters["shadowOffset"].SetValue(_shadowOffset);
                            _shadowEffect.Parameters["shadowColor"].SetValue(_shadowColor.ToVector4());
                            pass.Apply();
                            _gd.DrawPrimitives(PrimitiveType.TriangleList, 0, kvp.Value.Count / 3);
                        }


                    foreach (var technique in _drawEffect.Techniques)
                        foreach (var pass in technique.Passes)
                        {
                            _drawEffect.Parameters["txt"].SetValue(kvp.Key.Texture);
                            _drawEffect.Parameters["projection"].SetValue(_projection);
                            pass.Apply();
                            var rot = (float)gameTime.TotalGameTime.TotalSeconds;
                            _gd.DrawPrimitives(PrimitiveType.TriangleList, 0, kvp.Value.Count / 3);
                        }
                }
            }



            //And finally, clear the layers so we can draw for the next frame
            foreach (var layer in _sorted)
            {
                layer.Clear();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                _vertexBuffer.Dispose();
                _indexBuffer.Dispose();
                _shadowEffect.Dispose();
                _drawEffect.Dispose();
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RenderCompositor() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
    class RenderCompositorLayer
    {
        public int LayerNumber { get; set; }
        public Dictionary<SpriteSheet, ResizableArray<GPUDrawable>> SpriteSorted
        { get; set; }
        = new Dictionary<SpriteSheet, ResizableArray<GPUDrawable>>();
        public RenderCompositorLayer(int number)
        {
            LayerNumber = number;
        }
        public void AddObject(DrawableObject obj)
        {
            if (!SpriteSorted.ContainsKey(obj.Texture.SpriteSheet))
                SpriteSorted.Add(obj.Texture.SpriteSheet, new ResizableArray<GPUDrawable>());
            //Tesellate into 2 triangles
            //
            // A--B
            // -\ -
            // - \-
            // D--C
            var list = SpriteSorted[obj.Texture.SpriteSheet];
            list.Add(new GPUDrawable( //A / 0
                obj.Rectangle.TopLeft, obj.Position, obj.Size, obj.RotationOrigin,
                obj.Scale, obj.Rotation, obj.ObjectRotation, obj.Mask, obj.Texture.Rectangle.TopLeft));
            list.Add(new GPUDrawable( //B / 1
                obj.Rectangle.TopRight, obj.Position, obj.Size, obj.RotationOrigin,
                obj.Scale, obj.Rotation, obj.ObjectRotation, obj.Mask, obj.Texture.Rectangle.TopRight));
            list.Add(new GPUDrawable( //C / 2
                obj.Rectangle.BottomRight, obj.Position, obj.Size, obj.RotationOrigin,
                obj.Scale, obj.Rotation, obj.ObjectRotation, obj.Mask, obj.Texture.Rectangle.BottomRight));
            list.Add(new GPUDrawable( //C / 2
                obj.Rectangle.BottomRight, obj.Position, obj.Size, obj.RotationOrigin,
                obj.Scale, obj.Rotation, obj.ObjectRotation, obj.Mask, obj.Texture.Rectangle.BottomRight));
            list.Add(new GPUDrawable( //D / 3
                obj.Rectangle.BottomLeft, obj.Position, obj.Size, obj.RotationOrigin,
                obj.Scale, obj.Rotation, obj.ObjectRotation, obj.Mask, obj.Texture.Rectangle.BottomLeft));
            list.Add(new GPUDrawable( //A / 0
                obj.Rectangle.TopLeft, obj.Position, obj.Size, obj.RotationOrigin,
                obj.Scale, obj.Rotation, obj.ObjectRotation, obj.Mask, obj.Texture.Rectangle.TopLeft));
            //list.Add(new GPUDrawable //A
            //{
            //    Mask = obj.Mask,
            //    Position = obj.Position,
            //    Vertex = Vector2.Zero,
            //    Rotation = obj.Rotation,
            //    RotationOrigin = obj.RotationOrigin,
            //    Scale = obj.Scale,
            //    Size = obj.Size,
            //    SpritePosition = obj.Texture.Rectangle,
            //    TextureCoordinates = new Vector2(0, 0)
            //});
            //list.Add(new GPUDrawable //B
            //{
            //    Mask = obj.Mask,
            //    Position = obj.Position,
            //    Vertex = new Vector2(obj.Size.X, 0),
            //    Rotation = obj.Rotation,
            //    RotationOrigin = obj.RotationOrigin,
            //    Scale = obj.Scale,
            //    Size = obj.Size,
            //    SpritePosition = obj.Texture.Rectangle,
            //    TextureCoordinates = new Vector2(1, 0)
            //});
            //list.Add(new GPUDrawable //C
            //{
            //    Mask = obj.Mask,
            //    Position = obj.Position,
            //    Vertex = obj.Size,
            //    Rotation = obj.Rotation,
            //    RotationOrigin = obj.RotationOrigin,
            //    Scale = obj.Scale,
            //    Size = obj.Size,
            //    SpritePosition = obj.Texture.Rectangle,
            //    TextureCoordinates = new Vector2(1, 1)
            //});
            //list.Add(new GPUDrawable //C
            //{
            //    Mask = obj.Mask,
            //    Position = obj.Position,
            //    Vertex = obj.Size,
            //    Rotation = obj.Rotation,
            //    RotationOrigin = obj.RotationOrigin,
            //    Scale = obj.Scale,
            //    Size = obj.Size,
            //    SpritePosition = obj.Texture.Rectangle,
            //    TextureCoordinates = new Vector2(1, 1)
            //});
            //list.Add(new GPUDrawable //D
            //{
            //    Mask = obj.Mask,
            //    Position = obj.Position,
            //    Vertex = new Vector2(0, obj.Size.Y),
            //    Rotation = obj.Rotation,
            //    RotationOrigin = obj.RotationOrigin,
            //    Scale = obj.Scale,
            //    Size = obj.Size,
            //    SpritePosition = obj.Texture.Rectangle,
            //    TextureCoordinates = new Vector2(0, 1)
            //});
            //list.Add(new GPUDrawable //A
            //{
            //    Mask = obj.Mask,
            //    Position = obj.Position,
            //    Vertex = Vector2.Zero,
            //    Rotation = obj.Rotation,
            //    RotationOrigin = obj.RotationOrigin,
            //    Scale = obj.Scale,
            //    Size = obj.Size,
            //    SpritePosition = obj.Texture.Rectangle,
            //    TextureCoordinates = new Vector2(0, 0)
            //});
        }

        public void Clear()
        {
            foreach (var itm in SpriteSorted)
                itm.Value.Clear();
        }
    }
}
