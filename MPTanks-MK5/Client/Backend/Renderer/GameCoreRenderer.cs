using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Client.Backend.Renderer.Assets;
using MPTanks.Client.Backend.Renderer.LayerRenderers;
using MPTanks.Engine;
using MPTanks.Engine.Core;
using MPTanks.Engine.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Renderer
{
    public class GameCoreRenderer : IDisposable
    {
        public Game Client { get; private set; }
        private GameCore _game;
        public GameCore Game
        {
            get { return _game; }
            set { _game = value; }
        }
        public float PhysicsCompensation { get; set; } = 0.085f;
        public ILogger Logger => Game.Logger;
        internal AssetFinder Finder { get; private set; }
        public RenderTarget2D Target { get; set; }
        public RectangleF View { get; set; }
        public bool FXAAEnabled
        {
            get { return _fxaaRenderer.Enabled; }
            set { _fxaaRenderer.Enabled = value; }
        }
        public int[] TeamsToDisplayLightsFor { get; private set; }
        private List<LayerRenderer> _renderers = new List<LayerRenderer>();
        private GameWorldRenderer _gameRenderer;
        private FXAARenderer _fxaaRenderer;

        public GameCoreRenderer(Game client, GameCore game, string[] assetPaths, int[] teamsToDisplayFor)
        {
            Client = client;
            Game = game;
            TeamsToDisplayLightsFor = teamsToDisplayFor;

            Finder = new AssetFinder(this, new AssetCache(client.GraphicsDevice,
                new AssetLoader(this, client.GraphicsDevice, new AssetResolver(assetPaths)
                ), this));

            _fxaaRenderer = new FXAARenderer(
                this, client.GraphicsDevice, client.Content, Finder);
            FXAAEnabled = true;
            _gameRenderer = new GameWorldRenderer(
                this, client.GraphicsDevice, client.Content, Finder);

            _renderers.Add(new MapBackgroundRenderer(
                this, client.GraphicsDevice, client.Content, Finder));
            _renderers.Add(_gameRenderer);
            //For another time...
            //_renderers.Add(new LightRenderer(
            //    this, client.GraphicsDevice, client.Content, Finder));
            _renderers.Add(_fxaaRenderer);
        }

        public void Draw(GameTime gameTime)
        {
            _gameRenderer.SetShadowParameters(Game.Map.ShadowOffset, Game.Map.ShadowColor);
            foreach (var renderer in _renderers)
            {
                renderer.ViewRect = View;
                renderer.Draw(gameTime, Target);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                foreach (var obj in _renderers)
                {
                    (obj as IDisposable)?.Dispose();
                }
                Finder.Cache.Dispose();

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
