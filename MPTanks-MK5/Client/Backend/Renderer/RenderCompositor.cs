using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Client.Backend.Renderer.Assets.Sprites;
using MPTanks.Engine.Core;
using System;
using System.Collections.Generic;
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
        public RenderCompositor(GameCoreRenderer worldRenderer)
        {
            _renderer = worldRenderer;
            _gd = _renderer.Client.GraphicsDevice;
            _shadowEffect = worldRenderer.Client.Content.Load<Effect>("shadowRenderer");
            _drawEffect = worldRenderer.Client.Content.Load<Effect>("componentRenderer");
            _vertexBuffer = new DynamicVertexBuffer(_gd, GPUDrawable.Declaration, 1000, BufferUsage.None);

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
                indices[i + 5] = (ushort)(1 + i);
            }

            _indexBuffer = new IndexBuffer(_gd, IndexElementSize.SixteenBits, ushort.MaxValue, BufferUsage.WriteOnly);
            _indexBuffer.SetData(indices);
        }
        public void SetView(RectangleF viewRect)
        {
            _projection = Matrix.CreateOrthographicOffCenter(viewRect.Left, viewRect.Right,
                viewRect.Bottom, viewRect.Top, 1, -1);
        }
        public void SetShadowParameters(Vector2 offset, Color color)
        {
            _shadowOffset = offset;
            _shadowColor = color;
        }
        private Dictionary<int, RenderCompositorLayer> _layers { get; set; }
        private List<RenderCompositorLayer> _sorted { get; set; }
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
            _drawEffect.Parameters["projection"].SetValue(_projection);
            _shadowEffect.Parameters["projection"].SetValue(_projection);
            _drawEffect.Parameters["view"].SetValue(Matrix.Identity);
            _shadowEffect.Parameters["view"].SetValue(Matrix.Identity);
            _shadowEffect.Parameters["offset"].SetValue(_shadowOffset);
            _shadowEffect.Parameters["color"].SetValue(_shadowColor.ToVector4());

            _gd.RasterizerState = RasterizerState.CullNone;
            _gd.BlendState = BlendState.NonPremultiplied;

            foreach (var layer in _sorted)
            {
                foreach (var kvp in layer.SpriteSorted)
                {
                    _drawEffect.Parameters["txt"].SetValue(kvp.Key.Texture);

                    //Check if the buffer is large enough
                    if (kvp.Value.Count > _vertexBuffer.VertexCount)
                    {
                        //New buffer, larger
                        _vertexBuffer.Dispose();
                        _vertexBuffer = new DynamicVertexBuffer(_gd, GPUDrawable.Declaration,
                            kvp.Value.Count, BufferUsage.WriteOnly);
                    }

                    //Upload vertices
                    _vertexBuffer.SetData(kvp.Value.InternalArray, 0, kvp.Value.Count, SetDataOptions.Discard);
                    _gd.SetVertexBuffer(_vertexBuffer);
                    _gd.Indices = _indexBuffer;

                    foreach (var technique in _shadowEffect.Techniques)
                        foreach (var pass in technique.Passes)
                        {
                            pass.Apply();
                            _gd.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                0, kvp.Value.Count, 0, kvp.Value.Count / 2);
                        }

                    foreach (var technique in _drawEffect.Techniques)
                        foreach (var pass in technique.Passes)
                        {
                            pass.Apply();
                            _gd.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                0, kvp.Value.Count, 0, kvp.Value.Count / 2);
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
        public Dictionary<SpriteSheet, ResizableArray<GPUDrawable>> SpriteSorted { get; set; }
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
            list.Add(new GPUDrawable //A
            {
                Mask = obj.Mask,
                Vertex = obj.Position,
                Rotation = obj.Rotation,
                RotationOrigin = obj.RotationOrigin,
                Scale = obj.Scale,
                Size = obj.Size,
                SpritePosition = obj.Texture.Rectangle,
                TextureCoordinates = new Vector2(0, 0)
            });
            list.Add(new GPUDrawable //B
            {
                Mask = obj.Mask,
                Vertex = obj.Position + new Vector2(obj.Size.X, 0),
                Rotation = obj.Rotation,
                RotationOrigin = obj.RotationOrigin,
                Scale = obj.Scale,
                Size = obj.Size,
                SpritePosition = obj.Texture.Rectangle,
                TextureCoordinates = new Vector2(1, 0)
            });
            list.Add(new GPUDrawable //C
            {
                Mask = obj.Mask,
                Vertex = obj.Position + obj.Size,
                Rotation = obj.Rotation,
                RotationOrigin = obj.RotationOrigin,
                Scale = obj.Scale,
                Size = obj.Size,
                SpritePosition = obj.Texture.Rectangle,
                TextureCoordinates = new Vector2(1, 1)
            });
            list.Add(new GPUDrawable //D
            {
                Mask = obj.Mask,
                Vertex = obj.Position + new Vector2(0, obj.Size.Y),
                Rotation = obj.Rotation,
                RotationOrigin = obj.RotationOrigin,
                Scale = obj.Scale,
                Size = obj.Size,
                SpritePosition = obj.Texture.Rectangle,
                TextureCoordinates = new Vector2(0, 1)
            });
        }

        public void Clear()
        {
            foreach (var itm in SpriteSorted)
                itm.Value.Clear();
        }
    }
}
