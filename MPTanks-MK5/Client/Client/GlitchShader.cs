using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client
{
    public class GlitchShader
    {
        private Effect _fx;
        private Game _game;
        private RenderTarget2D _rt;
        private SpriteBatch _sb;
        public GlitchShader(Game game)
        {
            _fx = game.Content.Load<Effect>("chromaticaberration");
            _sb = new SpriteBatch(game.GraphicsDevice);
            _game = game;
        }

        public void BeginDraw()
        {
            if (_rt == null || _rt.Width != _game.GraphicsDevice.Viewport.Width || _rt.Height != _game.GraphicsDevice.Viewport.Height)
            {
                _rt?.Dispose();
                _rt = new RenderTarget2D(_game.GraphicsDevice, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height);
            }

            _game.GraphicsDevice.SetRenderTarget(_rt);
        }

        private Vector2 _shiftR, _shiftG, _shiftB;
        private TimeSpan _switchTime, _lastSw;
        private bool _enabled;
        private Random _rng = new Random();
        public void Draw(GameTime gameTime)
        {
            if (gameTime.TotalGameTime > _switchTime)
            {
                _enabled = _rng.Next(0, 10) > 7;
                if (_enabled)
                    _switchTime = gameTime.TotalGameTime + TimeSpan.FromSeconds(_rng.NextDouble() / 1.5f);
                else _switchTime = gameTime.TotalGameTime + TimeSpan.FromSeconds(_rng.NextDouble() * 3);

            }
            if (gameTime.TotalGameTime > _lastSw)
            {

                _shiftR = new Vector2((float)_rng.NextDouble() / 4, (float)_rng.NextDouble() / 4);
                _shiftG = new Vector2((float)_rng.NextDouble() / 4, (float)_rng.NextDouble() / 4);
                _shiftB = new Vector2((float)_rng.NextDouble() / 4, (float)_rng.NextDouble() / 4);
                _lastSw = gameTime.TotalGameTime + TimeSpan.FromMilliseconds(_rng.NextDouble() * 100);
            }
            _game.GraphicsDevice.SetRenderTarget(null);
            if (!_enabled)
            {
                _fx.CurrentTechnique = _fx.Techniques["Sin"];
                _fx.Parameters["rendering"].SetValue(_rt);
                _fx.Parameters["sec"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
                _sb.Begin(effect: _fx);
                _sb.Draw(_rt, _game.GraphicsDevice.Viewport.Bounds, Color.White);
                _sb.End();
                return;
            }

            _fx.CurrentTechnique = _fx.Techniques["Aberrate"];
            _fx.Parameters["rendering"].SetValue(_rt);
            _fx.Parameters["offsetR"].SetValue(_shiftR);
            _fx.Parameters["offsetG"].SetValue(_shiftG);
            _fx.Parameters["offsetB"].SetValue(_shiftB);
            _sb.Begin(effect: _fx);
            _sb.Draw(_rt, _game.GraphicsDevice.Viewport.Bounds, Color.White);
            _sb.End();
            return;
        }
    }
}
