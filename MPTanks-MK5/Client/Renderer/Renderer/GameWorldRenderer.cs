using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Engine;
using MPTanks.Engine.Logging;
using MPTanks.Renderer.Renderer.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Renderer.Renderer
{
    public class GameWorldRenderer : IDisposable
    {
        private GameCore _game;
        private RenderTarget2D _target;
        private GraphicsDevice _graphicsDevice;
        private bool _antiAlias;
        private FXAA _fxaa;
        private AssetResolver _resolver;

        private AnimationRenderer _animationRenderer;
        private GameObjectRenderer _objectRenderer;
        private LightMaskRenderer _lightRenderer;
        private MapBackgroundRenderer _backgroundRenderer;
        private ParticleRenderer _particleRenderer;

        private BasicEffect _effect;
        private SpriteBatch _spriteBatch;

        public ILogger Logger { get; private set; }

        /// <summary>
        /// Initializes a new game world renderer.
        /// </summary>
        /// <param name="game">The game to render.</param>
        /// <param name="drawTarget">The RenderTarget to draw to (can be null for screen).</param>
        /// <param name="gDevice">The GraphicsDevice for the game</param>
        /// <param name="antiAlias">Whether to use FXAA for the output.</param>
        /// <param name="playerTeamNumber">The team the player is on, for lighting calculations.</param>
        public GameWorldRenderer(GameCore game, RenderTarget2D drawTarget, GraphicsDevice gDevice,
            bool antiAlias, int playerTeamNumber, ILogger logger, string[] assetSearchPaths)
        {
            Logger = logger;

            _fxaa = new FXAA(_graphicsDevice);
            _game = game;
            _target = drawTarget;
            _graphicsDevice = gDevice;
            _antiAlias = antiAlias;

            _effect = new BasicEffect(gDevice);
            _spriteBatch = new SpriteBatch(gDevice);

            _animationRenderer = new AnimationRenderer(this, game.AnimationEngine, _spriteBatch, gDevice, _effect, _resolver);
            _objectRenderer = new GameObjectRenderer(this, game, _spriteBatch, gDevice, _effect, _resolver);
            _lightRenderer = new LightMaskRenderer(this, playerTeamNumber, game.LightEngine, _spriteBatch, gDevice, _effect, _resolver);
            _backgroundRenderer = new MapBackgroundRenderer(this, game.Map, _spriteBatch, gDevice, _effect, _resolver);
            _particleRenderer = new ParticleRenderer(this, game.ParticleEngine, _spriteBatch, gDevice, _effect, _resolver);
        }

        public void Draw(GameTime gameTime)
        {
            if (_antiAlias)
                _fxaa.BeginDraw(); //hook the rendertarget  
            else
                SetTarget();

            _backgroundRenderer.Draw(gameTime);
            _particleRenderer.DrawBelow(gameTime);
            _objectRenderer.Draw(gameTime);
            _animationRenderer.Draw(gameTime);
            _particleRenderer.DrawAbove(gameTime);

            _lightRenderer.Draw(gameTime);

            //And composite to the output buffer
            if (_antiAlias)
                _fxaa.Draw(_target);
        }

        private void SetTarget()
        {
            _graphicsDevice.SetRenderTarget(_target);
        }

        ~GameWorldRenderer()
        {
            Destroy();
        }

        private bool _disposeCalled = false;
        public void Dispose()
        {
            if (_disposeCalled) return;
            _disposeCalled = true;
            GC.SuppressFinalize(this);
            Destroy();
        }

        private void Destroy()
        {

        }
    }
}
