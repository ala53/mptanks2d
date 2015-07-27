using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Client.Backend.Renderer.Assets;
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
        public GameCore Game { get; private set; }
        public ILogger Logger => Game.Logger;
        internal AssetFinder Finder { get; private set; }
        internal RenderCompositor Compositor { get; private set; }
        internal FXAA Fxaa { get; private set; }
        public bool EnableFxaa { get; set; }
        public RenderTarget2D Target { get; set; }
        public RectangleF View { get; set; }
        public int[] TeamsToDisplayLightsFor { get; private set; }
        private List<PreProcessor> _preProcessors = new List<PreProcessor>();
        internal IReadOnlyList<PreProcessor> PreProcessors => _preProcessors;
        public GameCoreRenderer(Game client, GameCore game, string[] assetPaths, int[] teamsToDisplayFor)
        {
            Client = client;
            Game = game;
            TeamsToDisplayLightsFor = teamsToDisplayFor;

            Finder = new AssetFinder(this, new AssetCache(client.GraphicsDevice,
                new AssetLoader(this, client.GraphicsDevice, new AssetResolver(assetPaths)
                ), this));

            Fxaa = new FXAA(client.GraphicsDevice);

            Compositor = new RenderCompositor(this);

            _preProcessors.Add(new PreProcessorTypes.AnimationPreProcessor(this, Finder, Compositor));
            _preProcessors.Add(new PreProcessorTypes.GameObjectPreProcessor(this, Finder, Compositor));
            _preProcessors.Add(new PreProcessorTypes.MapBackgroundPreProcessor(this, Finder, Compositor));
            _preProcessors.Add(new PreProcessorTypes.ParticlePreProcessor(this, Finder, Compositor));
        }

        public void Draw(GameTime gameTime)
        {
            if (EnableFxaa)
            {
                Fxaa.BeginDraw();
                if (Target != null)
                    Fxaa.Resize(Target.Width, Target.Height);
            }
            else
            {
                Client.GraphicsDevice.SetRenderTarget(Target);
            }

            foreach (var processor in PreProcessors)
                processor.Process(gameTime);

            Compositor.SetShadowParameters(Game.Map.ShadowOffset, Game.Map.ShadowColor);
            Compositor.SetView(View);
            Compositor.Draw(gameTime);

            if (EnableFxaa)
                Fxaa.Draw(Target);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Compositor.Dispose();

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
