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
        private static Effect _shadowEffect;
        private static Effect _drawEffect;
        private RectangleF _viewRect;
        private Vector2 _shadowOffset = new Vector2(0.2f);
        private Color _shadowColor = new Color(50, 50, 50, 100);
        private DynamicVertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;
        private RenderTarget2D _shadowBuffer;
        public RenderCompositor(GameCoreRenderer worldRenderer)
        {
            _renderer = worldRenderer;
            _gd = _renderer.Client.GraphicsDevice;
            if (_shadowEffect == null)
                _shadowEffect = worldRenderer.Client.Content.Load<Effect>("shadowRenderer");
            if (_drawEffect == null)
                _drawEffect = worldRenderer.Client.Content.Load<Effect>("componentRenderer");
            _vertexBuffer = new DynamicVertexBuffer(_gd, GPUDrawable.VertexDeclaration, 1000, BufferUsage.None);

            //Generate indices
            var indices = new ushort[ushort.MaxValue];

            //6 to 4 ratio
            //0, 1, 2 | 2, 3, 0

            int arrCounter = 0;
            ushort vertCounter = 0;
            while (arrCounter < indices.Length - 8)
            {
                indices[arrCounter++] = (ushort)(vertCounter);
                indices[arrCounter++] = (ushort)(vertCounter + 1);
                indices[arrCounter++] = (ushort)(vertCounter + 2);
                indices[arrCounter++] = (ushort)(vertCounter + 2);
                indices[arrCounter++] = (ushort)(vertCounter + 3);
                indices[arrCounter++] = (ushort)(vertCounter);
                vertCounter += 4;
            }

            _indexBuffer = new IndexBuffer(_gd, IndexElementSize.SixteenBits, ushort.MaxValue, BufferUsage.WriteOnly);
            _indexBuffer.SetData(indices);
        }
        public void SetView(RectangleF viewRect)
        {
            _viewRect = viewRect;
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

        public RenderCompositorLayer GetOrAddLayer(int number)
        {
            lock (_layers)
            {
                if (_layers.ContainsKey(number)) return _layers[number];
                if (number < -100000000 || number > 100000000)
                    throw new ArgumentOutOfRangeException("number", "Must be between -100,000,000 and +100,000,000");

                //Create layer
                var spriteInfo = new Engine.Assets.SpriteInfo(null, null);
                var layer = new RenderCompositorLayer(number,
                    _renderer.Finder.RetrieveAsset(ref spriteInfo).SpriteSheet);
                _layers.Add(number, layer);
                _sorted.Add(layer);
                _sorted.Sort((a, b) => a.LayerNumber - b.LayerNumber);
                return layer;
            }
        }

        public void Draw(GameTime gameTime, RenderTarget2D target)
        {
            if (disposedValue)
                return;

            _gd.RasterizerState = RasterizerState.CullNone;
            _gd.BlendState = BlendState.NonPremultiplied;

            var projection = Matrix.CreateOrthographicOffCenter(
                _viewRect.Left, _viewRect.Right, _viewRect.Bottom, _viewRect.Top, -1, float.MaxValue);

            CheckTarget(target.Width, target.Height);

            _gd.SetRenderTarget(_shadowBuffer);
            _gd.Clear(Color.Transparent);

            DrawObjects(gameTime, projection);
            DrawShadows(target);

            //And finally, clear the layers so we can draw for the next frame
            foreach (var layer in _sorted)
            {
                layer.Clear();
            }
        }

        private void DrawObjects(GameTime gameTime, Matrix projection)
        {

            foreach (var layer in _sorted)
            {
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
                    _gd.Indices = _indexBuffer;
                    //Draw the object
                    foreach (var technique in _drawEffect.Techniques)
                        foreach (var pass in technique.Passes)
                        {
                            _drawEffect.Parameters["txt"].SetValue(kvp.Key.Texture);
                            _drawEffect.Parameters["projection"].SetValue(projection);
                            pass.Apply();
                            _gd.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, kvp.Value.Count, 0, kvp.Value.Count / 2);
                        }
                }
            }
        }

        private VertexPositionTexture[] _shadowScreenPrimitiveArray = new[]
                    {
                        new VertexPositionTexture(Vector3.Zero, Vector2.Zero),
                        new VertexPositionTexture(new Vector3(1, 0, 0), new Vector2(1, 0)),
                        new VertexPositionTexture(new Vector3(0, 1, 0), new Vector2(0, 1)),
                        new VertexPositionTexture(new Vector3(1, 1, 0), Vector2.One),
                    };
        private void DrawShadows(RenderTarget2D target)
        {
            _gd.SetRenderTarget(target);

            _gd.SetVertexBuffer(null);
            _gd.Indices = null;
            //And do shadowing
            foreach (var technique in _shadowEffect.Techniques)
                foreach (var pass in technique.Passes)
                {
                    _shadowEffect.Parameters["txt"].SetValue(_shadowBuffer);
                    //Compute shadow offset in 0-1 scale of the render target
                    var blocksToPixels = new Vector2(
                        target.Width / _viewRect.Width,
                        target.Height / _viewRect.Height
                        );
                    _shadowEffect.Parameters["projection"].SetValue(Matrix.CreateOrthographicOffCenter(
                        0, 1, 1, 0, -1, 1));
                    _shadowEffect.Parameters["shadowOffset"].SetValue(
                        (_shadowOffset * blocksToPixels) / new Vector2(target.Width, target.Height));
                    _shadowEffect.Parameters["shadowColor"].SetValue(_shadowColor.ToVector4());
                    pass.Apply();
                    _gd.DrawUserPrimitives(PrimitiveType.TriangleStrip, _shadowScreenPrimitiveArray, 0, 2);
                }
        }

        private void CheckTarget(int drawWidth, int drawHeight)
        {
            if (_shadowBuffer == null)
                _shadowBuffer = new RenderTarget2D(_gd, drawWidth, drawHeight);
            else if (_shadowBuffer.Width != drawWidth || _shadowBuffer.Height != drawHeight)
            {
                _shadowBuffer.Dispose();
                _shadowBuffer = new RenderTarget2D(_gd, drawWidth, drawHeight);
            }
        }

        #region IDisposable Support
        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;
                _shadowBuffer.Dispose();
                _vertexBuffer.Dispose();
                _indexBuffer.Dispose();
                //These are left commented out because
                //MonoGame leaves shaders as singleton objects,
                //which means that if we dispose here, we corrupt the entire appdomain 
                //and will never be able to use the shaders again
                //_shadowEffect.Dispose();
                //_drawEffect.Dispose();
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
        private ResizableArray<GPUDrawable> _baseSheetArr;
        private SpriteSheet _baseSheet;
        public RenderCompositorLayer(int number, SpriteSheet baseSheet)
        {
            LayerNumber = number;
            _baseSheetArr = new ResizableArray<GPUDrawable>();
            _baseSheet = baseSheet;
            SpriteSorted.Add(baseSheet, _baseSheetArr);
        }
        public void AddObject(DrawableObject obj)
        {
            lock (SpriteSorted)
            {
                ResizableArray<GPUDrawable> list;
                if (obj.Texture.SpriteSheet == _baseSheet)
                {
                    list = _baseSheetArr;
                }
                else
                {
                    if (!SpriteSorted.ContainsKey(obj.Texture.SpriteSheet))
                        SpriteSorted.Add(obj.Texture.SpriteSheet, new ResizableArray<GPUDrawable>());
                    list = SpriteSorted[obj.Texture.SpriteSheet];
                }
                //Tesellate into 2 triangles
                //
                // A--B
                // -\ -
                // - \-
                // D--C
                list.Add(new GPUDrawable( //A / 0
                    obj.Rectangle.TopLeft, obj.Position, obj.Size, obj.RotationOrigin,
                    obj.Scale, obj.Rotation, obj.ObjectRotation, obj.Mask, obj.Texture.Rectangle.TopLeft));
                list.Add(new GPUDrawable( //B / 1
                    obj.Rectangle.TopRight, obj.Position, obj.Size, obj.RotationOrigin,
                    obj.Scale, obj.Rotation, obj.ObjectRotation, obj.Mask, obj.Texture.Rectangle.TopRight));
                list.Add(new GPUDrawable( //C / 2
                    obj.Rectangle.BottomRight, obj.Position, obj.Size, obj.RotationOrigin,
                    obj.Scale, obj.Rotation, obj.ObjectRotation, obj.Mask, obj.Texture.Rectangle.BottomRight));
                list.Add(new GPUDrawable( //D / 3
                    obj.Rectangle.BottomLeft, obj.Position, obj.Size, obj.RotationOrigin,
                    obj.Scale, obj.Rotation, obj.ObjectRotation, obj.Mask, obj.Texture.Rectangle.BottomLeft));
            }
        }

        public void Clear()
        {
            foreach (var itm in SpriteSorted)
                itm.Value.Clear();
        }
    }
}
