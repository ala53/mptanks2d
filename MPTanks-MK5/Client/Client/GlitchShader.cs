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
            _fx = game.Content.Load<Effect>("menushader");
            _sb = new SpriteBatch(game.GraphicsDevice);
            _game = game;
        }

        public void BeginDraw()
        {
            if (_game.GraphicsDevice.Viewport.Height < 1 || _game.GraphicsDevice.Viewport.Width < 1) return;
            if (_rt == null || _rt.Width != _game.GraphicsDevice.Viewport.Width || _rt.Height != _game.GraphicsDevice.Viewport.Height)
            {
                _rt?.Dispose();
                _rt = new RenderTarget2D(_game.GraphicsDevice, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height);
            }

            _game.GraphicsDevice.SetRenderTarget(_rt);
        }

        private Vector2 _shiftR, _shiftG, _shiftB, _targetShiftR, _targetShiftG, _targetShiftB;
        private TimeSpan _switchTime, _nextGlitchDirectionSwitch;
        private mode _mode;
        private Random _rng = new Random();
        public bool Enabled { get; set; } = false;
        enum mode
        {
            disabled, glitch, breakApart
        }
        public void Draw(GameTime gameTime)
        {
            if (_game.GraphicsDevice.Viewport.Height < 1 || _game.GraphicsDevice.Viewport.Width < 1) return;
            if (gameTime.TotalGameTime > _switchTime)
            {
                switch (_rng.Next(0, 10))
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        _mode = mode.disabled;
                        break;
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        _mode = mode.breakApart;
                        break;
                    case 9:
                    case 10:
                        _mode = mode.glitch;
                        break;
                }

                if (_mode == mode.glitch)
                    _switchTime = gameTime.TotalGameTime + TimeSpan.FromSeconds(_rng.NextDouble() / 2f); //max .5sec of glitch

                else if (_mode == mode.disabled) _switchTime = gameTime.TotalGameTime + TimeSpan.FromSeconds(_rng.NextDouble() * 6);
                else if (_mode == mode.breakApart)
                {
                    _shiftR = Vector2.Zero;
                    _shiftG = Vector2.Zero;
                    _shiftB = Vector2.Zero;
                    _switchTime = gameTime.TotalGameTime + TimeSpan.FromSeconds(_rng.NextDouble() / 1.5);
                }
            }
            if (gameTime.TotalGameTime > _nextGlitchDirectionSwitch && _mode == mode.glitch)
            {

                _shiftR = new Vector2((float)_rng.NextDouble() / 2, (float)_rng.NextDouble() / 2);
                _shiftG = new Vector2((float)_rng.NextDouble() / 2, (float)_rng.NextDouble() / 2);
                _shiftB = new Vector2((float)_rng.NextDouble() / 2, (float)_rng.NextDouble() / 2);
                _nextGlitchDirectionSwitch = gameTime.TotalGameTime + TimeSpan.FromMilliseconds(_rng.NextDouble() * 100);
            }


            if (gameTime.TotalGameTime > _nextGlitchDirectionSwitch && _mode == mode.breakApart)
            {
                var amount = (float)(_rng.NextDouble() - 0.5) / 70;
                _targetShiftR = Vector2.Zero;
                _targetShiftG = new Vector2((float)_rng.NextDouble() / 70, (float)(_rng.NextDouble() - 0.5) / 70);
                _targetShiftB = new Vector2((float)-_rng.NextDouble() / 70, (float)(_rng.NextDouble() - 0.5) / 70);
                _nextGlitchDirectionSwitch = gameTime.TotalGameTime + TimeSpan.FromMilliseconds(_rng.NextDouble() * 500);
            }

            if (_mode == mode.breakApart)
            {
                _shiftR = Vector2.Lerp(_shiftR, _targetShiftR, (float)_rng.NextDouble() / 5);
                _shiftG = Vector2.Lerp(_shiftG, _targetShiftG, (float)_rng.NextDouble() / 5);
                _shiftB = Vector2.Lerp(_shiftB, _targetShiftB, (float)_rng.NextDouble() / 5);
            }

            _game.GraphicsDevice.SetRenderTarget(null);
            _sb.Begin(effect: _fx);
            _fx.Parameters["rendering"].SetValue(_rt);
            if (_mode == mode.disabled || !Enabled)
            {
                _fx.CurrentTechnique = _fx.Techniques["Sin"];
                _fx.Parameters["sec"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            }
            else if (_mode == mode.glitch)
            {

                _fx.CurrentTechnique = _fx.Techniques["GlitchOut"];
                _fx.Parameters["offsetR"].SetValue(_shiftR);
                _fx.Parameters["offsetG"].SetValue(_shiftG);
                _fx.Parameters["offsetB"].SetValue(_shiftB);
            }
            else if (_mode == mode.breakApart)
            {

                _fx.CurrentTechnique = _fx.Techniques["Aberrate"];
                _fx.Parameters["offsetR"].SetValue(_shiftR);
                _fx.Parameters["offsetG"].SetValue(_shiftG);
                _fx.Parameters["offsetB"].SetValue(_shiftB);
            }
            _sb.Draw(_rt, _game.GraphicsDevice.Viewport.Bounds, Color.White);
            _sb.End();
            return;
        }
    }
}
