using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MPTanks.Client.Backend.Renderer.Assets;
using System.IO;

namespace MPTanks.Client.Backend.Renderer.LayerRenderers
{
    class LightRenderer : LayerRenderer, IDisposable
    {
        private Effect _lightRenderer;
        private Effect _lightMaskPreCompositor;
        private RenderTarget2D _preCompositorTarget;
        private RenderTarget2D _worldTempTarget;
        private SpriteBatch _sb;
        public Color AmbientColor { get; set; }
        public float AmbientIntensity { get; set; }
        public LightRenderer(GameCoreRenderer renderer, GraphicsDevice gd,
            ContentManager content, AssetFinder finder)
            : base(renderer, gd, content, finder)
        {
            _sb = new SpriteBatch(GraphicsDevice);
            _lightRenderer = Content.Load<Effect>("lightMaskRenderer");
            _lightMaskPreCompositor = Content.Load<Effect>("lightMaskPreCompositor");
        }
        public override void Draw(GameTime gameTime, RenderTarget2D target)
        {
            CheckTargets(target);
            //Copy to temp buffer
            GraphicsDevice.SetRenderTarget(_worldTempTarget);
            _sb.Begin(SpriteSortMode.Immediate);
            _sb.Draw(target, _worldTempTarget.Bounds, Color.White);
            _sb.End();

            GraphicsDevice.SetRenderTarget(_preCompositorTarget);
            BuildLightMaskSet(gameTime);
            GraphicsDevice.SetRenderTarget(target);
            _sb.Begin();
            _sb.Draw(_preCompositorTarget, _preCompositorTarget.Bounds, Color.White);
            /*var fs = new FileStream($"save{sv++}.png", FileMode.Create, FileAccess.ReadWrite);
            _preCompositorTarget.SaveAsPng(fs, _preCompositorTarget.Width, _preCompositorTarget.Height);
            fs.Flush();
            fs.Close();
            fs.Dispose();*/
            _sb.End();
            Draw(gameTime, _preCompositorTarget, _worldTempTarget);
        }

        int sv;
        private VertexPositionTexture[] _lightMaskPrimitiveArray = new[]
                    {
                        new VertexPositionTexture(Vector3.Zero, Vector2.Zero),
                        new VertexPositionTexture(new Vector3(1, 0, 0), new Vector2(1, 0)),
                        new VertexPositionTexture(new Vector3(0, 1, 0), new Vector2(0, 1)),
                        new VertexPositionTexture(new Vector3(1, 1, 0), Vector2.One),
                    };
        private void BuildLightMaskSet(GameTime gameTime)
        {

            foreach (var light in Renderer.Game.LightEngine.Lights)
            {
                var info = light.SpriteInfo;
                var sprite = Finder.RetrieveAsset(ref info);
                if (info.IsAnimation) Finder.IncrementAnimation(ref info, gameTime);
                light.SpriteInfo = info;

                var transform = Matrix.CreateTranslation(
                    new Vector3(-light.RotationOrigin, 1)) *
                    Matrix.CreateRotationZ(light.Rotation) *
                    Matrix.CreateTranslation(
                    new Vector3(light.RotationOrigin, 1));

                //_lightMaskPrimitiveArray[0].Position = Vector3.Zero;
                _lightMaskPrimitiveArray[1].Position = new Vector3(light.Size.X, 0, 0);
                _lightMaskPrimitiveArray[2].Position = new Vector3(0, light.Size.Y, 0);
                _lightMaskPrimitiveArray[3].Position = new Vector3(light.Size, 0);

                _lightMaskPrimitiveArray[0].TextureCoordinate = sprite.Rectangle.TopLeft;
                _lightMaskPrimitiveArray[1].TextureCoordinate = sprite.Rectangle.TopRight;
                _lightMaskPrimitiveArray[2].TextureCoordinate = sprite.Rectangle.BottomLeft;
                _lightMaskPrimitiveArray[3].TextureCoordinate = sprite.Rectangle.BottomRight;

                var view = Matrix.CreateOrthographic(
                           /* Renderer.View.X, Renderer.View.X + */ Renderer.View.Width,
                           /* Renderer.View.Y, Renderer.View.Y + */ Renderer.View.Height, 1, -1);

                var transformView = view;// * transform;

                foreach (var technique in _lightMaskPreCompositor.Techniques)
                    foreach (var pass in technique.Passes)
                    {
                        _lightMaskPreCompositor.Parameters["intensity"]?.SetValue(light.Intensity);
                        _lightMaskPreCompositor.Parameters["txt"]?.SetValue(sprite.SpriteSheet.Texture);
                        _lightMaskPreCompositor.Parameters["transform"]?.SetValue(transformView);
                        _lightMaskPreCompositor.Parameters["color"]?.SetValue(light.Color.ToVector4());
                        pass.Apply();
                        GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, _lightMaskPrimitiveArray, 0, 2);
                    }
            }
        }

        private void Draw(GameTime gameTime, RenderTarget2D compositor, RenderTarget2D world)
        {

        }

        private void CheckTargets(RenderTarget2D target)
        {
            if (_preCompositorTarget == null || _preCompositorTarget.Width != target.Width ||
                _preCompositorTarget.Height != target.Height)
            {
                _preCompositorTarget?.Dispose();
                _preCompositorTarget = new RenderTarget2D(GraphicsDevice, target.Width, target.Height);
            }
            if (_worldTempTarget == null || _worldTempTarget.Width != target.Width ||
                _worldTempTarget.Height != target.Height)
            {
                _worldTempTarget?.Dispose();
                _worldTempTarget = new RenderTarget2D(GraphicsDevice, target.Width, target.Height);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                _sb?.Dispose();
                _worldTempTarget?.Dispose();
                _preCompositorTarget.Dispose();
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~LightRenderer() {
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
}
