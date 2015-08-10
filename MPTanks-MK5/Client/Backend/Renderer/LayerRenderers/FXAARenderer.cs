using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Client.Backend.Renderer.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MPTanks.Client.Backend.Renderer.LayerRenderers
{
    class FXAARenderer : LayerRenderer
    {
        #region Parameters
        private const float MainImageDepth = 0.9f;    // Near the back
        private const float ThingsInFrontDepth = 0.5f;

        // This effects sub-pixel AA quality and inversely sharpness.
        //   Where N ranges between,
        //     N = 0.50 (default)
        //     N = 0.33 (sharper)
        private float N = 0.40f;

        // Choose the amount of sub-pixel aliasing removal.
        // This can effect sharpness.
        //   1.00 - upper limit (softer)
        //   0.75 - default amount of filtering
        //   0.50 - lower limit (sharper, less sub-pixel aliasing removal)
        //   0.25 - almost off
        //   0.00 - completely off
        private float subPixelAliasingRemoval = 0.75f;

        // The minimum amount of local contrast required to apply algorithm.
        //   0.333 - too little (faster)
        //   0.250 - low quality
        //   0.166 - default
        //   0.125 - high quality 
        //   0.063 - overkill (slower)
        private float edgeTheshold = 0.166f;

        // Trims the algorithm from processing darks.
        //   0.0833 - upper limit (default, the start of visible unfiltered edges)
        //   0.0625 - high quality (faster)
        //   0.0312 - visible limit (slower)
        // Special notes when using FXAA_GREEN_AS_LUMA,
        //   Likely want to set this to zero.
        //   As colors that are mostly not-green
        //   will appear very dark in the green channel!
        //   Tune by looking at mostly non-green content,
        //   then start at zero and increase until aliasing is a problem.
        private float edgeThesholdMin = 0f;

        // This does not effect PS3, as this needs to be compiled in.
        //   Use FXAA_CONSOLE__PS3_EDGE_SHARPNESS for PS3.
        //   Due to the PS3 being ALU bound,
        //   there are only three safe values here: 2 and 4 and 8.
        //   These options use the shaders ability to a free *|/ by 2|4|8.
        // For all other platforms can be a non-power of two.
        //   8.0 is sharper (default!!!)
        //   4.0 is softer
        //   2.0 is really soft (good only for vector graphics inputs)
        private float consoleEdgeSharpness = 8.0f;

        // This does not effect PS3, as this needs to be compiled in.
        //   Use FXAA_CONSOLE__PS3_EDGE_THRESHOLD for PS3.
        //   Due to the PS3 being ALU bound,
        //   there are only two safe values here: 1/4 and 1/8.
        //   These options use the shaders ability to a free *|/ by 2|4|8.
        // The console setting has a different mapping than the quality setting.
        // Other platforms can use other values.
        //   0.125 leaves less aliasing, but is softer (default!!!)
        //   0.25 leaves more aliasing, and is sharper
        private float consoleEdgeThreshold = 0.125f;

        // Trims the algorithm from processing darks.
        // The console setting has a different mapping than the quality setting.
        // This only applies when FXAA_EARLY_EXIT is 1.
        // This does not apply to PS3, 
        // PS3 was simplified to avoid more shader instructions.
        //   0.06 - faster but more aliasing in darks
        //   0.05 - default
        //   0.04 - slower and less aliasing in darks
        // Special notes when using FXAA_GREEN_AS_LUMA,
        //   Likely want to set this to zero.
        //   As colors that are mostly not-green
        //   will appear very dark in the green channel!
        //   Tune by looking at mostly non-green content,
        //   then start at zero and increase until aliasing is a problem.
        private float consoleEdgeThresholdMin = 0f;
        #endregion
        private Effect _fxaaEffect;
        private RenderTarget2D _tempTarget;
        private SpriteBatch _sb;
        private VertexPositionTexture[] _fxaaPrimitiveArray = new[]
                    {
                        new VertexPositionTexture(Vector3.Zero, Vector2.Zero),
                        new VertexPositionTexture(new Vector3(1, 0, 0), new Vector2(1, 0)),
                        new VertexPositionTexture(new Vector3(0, 1, 0), new Vector2(0, 1)),
                        new VertexPositionTexture(new Vector3(1, 1, 0), Vector2.One),
                    };

        public bool Enabled { get; set; }
        public FXAARenderer(GameCoreRenderer renderer, GraphicsDevice gd,
            ContentManager content, AssetFinder finder)
            : base(renderer, gd, content, finder)
        {
            _fxaaEffect = Content.Load<Effect>("fxaa3");
            _sb = new SpriteBatch(GraphicsDevice);
        }
        int i = 0;
        public override void Draw(GameTime gameTime, RenderTarget2D target)
        {
            CheckTarget(target);

            if (!Enabled) return;

            //Copy to temp
            GraphicsDevice.SetRenderTarget(_tempTarget);
            _sb.Begin(SpriteSortMode.Immediate);
            _sb.Draw(target, new Rectangle(0, 0, _tempTarget.Width, _tempTarget.Height), Color.White);
            _sb.End();
            GraphicsDevice.SetRenderTarget(target);
            _fxaaEffect.CurrentTechnique = _fxaaEffect.Techniques["Draw"];
            //var fs =
            //    new System.IO.FileStream($"pn{i++}.png", System.IO.FileMode.Create,
            //    System.IO.FileAccess.ReadWrite);
            //_tempTarget.SaveAsPng(fs, _tempTarget.Width, _tempTarget.Height);
            //fs.Flush(); fs.Dispose();
            foreach (var technique in _fxaaEffect.Techniques)
            {
                foreach (var pass in technique.Passes)
                {
                    _fxaaEffect.Parameters["InverseViewportSize"]?.SetValue(new Vector2(1f / target.Width, 1f / target.Height));
                    _fxaaEffect.Parameters["ConsoleSharpness"]?.SetValue(new Vector4(
                        -N / target.Width,
                        -N / target.Height,
                        N / target.Width,
                        N / target.Height
                        ));
                    _fxaaEffect.Parameters["ConsoleOpt1"]?.SetValue(new Vector4(
                        -2.0f / target.Width,
                        -2.0f / target.Height,
                        2.0f / target.Width,
                        2.0f / target.Height
                        ));
                    _fxaaEffect.Parameters["ConsoleOpt2"]?.SetValue(new Vector4(
                        8.0f / target.Width,
                        8.0f / target.Height,
                        -4.0f / target.Width,
                        -4.0f / target.Height
                        ));
                    _fxaaEffect.Parameters["projection"]?.SetValue(Matrix.CreateOrthographicOffCenter(0, target.Width, 0, target.Height, -1, 1));
                    _fxaaEffect.Parameters["SubPixelAliasingRemoval"]?.SetValue(subPixelAliasingRemoval);
                    _fxaaEffect.Parameters["EdgeThreshold"]?.SetValue(edgeTheshold);
                    _fxaaEffect.Parameters["EdgeThresholdMin"]?.SetValue(edgeThesholdMin);
                    _fxaaEffect.Parameters["ConsoleEdgeSharpness"]?.SetValue(consoleEdgeSharpness);
                    _fxaaEffect.Parameters["ConsoleEdgeThreshold"]?.SetValue(consoleEdgeThreshold);
                    _fxaaEffect.Parameters["ConsoleEdgeThresholdMin"]?.SetValue(consoleEdgeThresholdMin);

                    _fxaaEffect.Parameters["txt"]?.SetValue(_tempTarget);
                    _fxaaEffect.Parameters["projection"].SetValue(Matrix.CreateOrthographicOffCenter(
                        0, 1, 1, 0, -1, 1));
                    pass.Apply();
                    GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, _fxaaPrimitiveArray, 0, 2);
                }
            }

        }

        private void CheckTarget(RenderTarget2D target)
        {
            if (_tempTarget == null || target.Width != _tempTarget.Width ||
                target.Height != _tempTarget.Height)
            {
                if (_tempTarget != null)
                    _tempTarget.Dispose();

                _tempTarget = new RenderTarget2D(GraphicsDevice, target.Width, target.Height);
            }
        }
    }
}
