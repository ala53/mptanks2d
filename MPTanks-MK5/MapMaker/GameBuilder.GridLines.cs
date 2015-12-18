using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.MapMaker
{
    public partial class GameBuilder
    {
        private Texture2D _blankTx;
        private void DrawGridLines()
        {
            if (_blankTx == null)
            {
                _blankTx = new Texture2D(GraphicsDevice, 1, 1);
                _blankTx.SetData(new[] { Color.White });
            }
            Color gridLineColor = new Color(Color.Gray, 0.2f);
            var blockSize = GridLineBlockSize();
            var viewRect = ComputeDrawRectangle();

            var minX = (float)Math.Round(viewRect.Left / blockSize) * blockSize;
            var maxX = (float)Math.Round(viewRect.Right / blockSize) * blockSize + blockSize;
            var minY = (float)Math.Round(viewRect.Top / blockSize) * blockSize;
            var maxY = (float)Math.Round(viewRect.Bottom / blockSize) * blockSize + blockSize;

            _sb.Begin(blendState: BlendState.NonPremultiplied);
            //Draw the vertical bars
            for (var x = minX; x < maxX; x += blockSize)
            {
                var screenSpace = ComputeScreenSpace(new Vector2(x, 0), viewRect);
                _sb.Draw(_blankTx,
                    new Rectangle(
                        (int)screenSpace.X,
                        0,
                        1,
                        GraphicsDevice.Viewport.Height), gridLineColor);
            }
            for (var y = minY; y < maxY; y += blockSize)
            {
                var screenSpace = ComputeScreenSpace(new Vector2(0, y), viewRect);
                _sb.Draw(_blankTx,
                    new Rectangle(
                        0,
                        (int)screenSpace.Y,
                        GraphicsDevice.Viewport.Width,
                        1), gridLineColor);
            }

            _sb.End();
        }

        private Vector2 ComputeScreenSpace(Vector2 pos, RectangleF rect)
        {
            pos -= rect.TopLeft;
            var scale = new Vector2(
                GraphicsDevice.Viewport.Width / rect.Width,
                GraphicsDevice.Viewport.Height / rect.Height);
            return pos * scale;
        }

        private float GridLineBlockSize()
        {
            if (_cameraZoom >= 10)
                return 20;
            else if (_cameraZoom >= 5)
                return 10;
            else if (_cameraZoom >= 2.5)
                return 5;
            else if (_cameraZoom >= 1)
                return 3;
            else if (_cameraZoom >= 0.75)
                return 2;
            else if (_cameraZoom >= 0.5)
                return 1;
            else if (_cameraZoom >= 0.25)
                return 0.5f;
            else if (_cameraZoom >= 0.1)
                return 0.25f;
            else return 0.1f;
        }
    }
}
