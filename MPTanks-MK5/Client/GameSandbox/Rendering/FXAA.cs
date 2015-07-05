using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.GameClient.Rendering
{
    class FXAA : IDisposable
    {
        private Game _game;
        private Effect _fxaa;
        private RenderTarget2D _target;
        private SpriteBatch _sb;
        public FXAA(Game game)
        {
            _sb = new SpriteBatch(game.GraphicsDevice);
            _game = game;
            _fxaa = _game.Content.Load<Effect>("fxaaEffect");
            _target = new RenderTarget2D(_game.GraphicsDevice,
                _game.GraphicsDevice.Viewport.Width,
                _game.GraphicsDevice.Viewport.Height, false,
                SurfaceFormat.Bgra32, DepthFormat.Depth24Stencil8, 0,
                RenderTargetUsage.DiscardContents, false);
        }

        public void BeginRender()
        {
            if ( _target.Width != _game.GraphicsDevice.Viewport.Width
                 || _target.Height != _game.GraphicsDevice.Viewport.Height)
            {
                //Resize the rendertarget
                _target.Dispose();
                _target = new RenderTarget2D(_game.GraphicsDevice,
                    _game.GraphicsDevice.Viewport.Width,
                    _game.GraphicsDevice.Viewport.Height, false,
                    SurfaceFormat.Bgra32, DepthFormat.Depth24Stencil8, 0,
                    RenderTargetUsage.DiscardContents, false);
            }

            _game.GraphicsDevice.SetRenderTarget(_target);
        }

        public void Render()
        {
            _game.GraphicsDevice.SetRenderTarget(null);
        }

        void IDisposable.Dispose()
        {
            _fxaa.Dispose();
            _target.Dispose();
            _sb.Dispose();
        }

        public void Destroy()
        {
            ((IDisposable)this).Dispose();
        }
    }
}
