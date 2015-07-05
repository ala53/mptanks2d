using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.GameSandbox.Rendering
{
    class LoadingScreen : IDisposable
    {
        public float Value { get; set; }
        public float Maximum { get; set; }
        public string Billboard { get; set; }
        public string Status { get; set; }
        public Color BillboardColor { get; set; }
        public Color StatusColor { get; set; }
        public Color LoadedColor { get; set; }
        public Color UnloadedColor { get; set; }

        public bool Completed { get; private set; }

        public bool IsSlidingOut { get; private set; }

        private GameClient _game;
        private Texture2D _helperTexture;
        private SpriteFont _font;
        public LoadingScreen(GameClient game)
        {
            _game = game;
            _helperTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            _helperTexture.SetData(new[] { Color.White });
            _font = _game.Content.Load<SpriteFont>("font");

            Billboard = "Loading";
            Status = "Working";

            BillboardColor = Color.Gray;
            StatusColor = Color.DarkGray;
            LoadedColor = Color.SlateGray;
            UnloadedColor = Color.DarkSlateGray;
        }
        const float width = 0.75f; //3/4 of the screen wide
        const float height = 0.1f; //1/10 of the screen high
        public void Render(SpriteBatch sb, GameTime gameTime)
        {
            SlideOutOffset(gameTime);

            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, 
                DepthStencilState.None, RasterizerState.CullNone, null, Matrix.Identity);

            sb.Draw(_helperTexture, new Rectangle(0, verticalOffset,  //Draw background
                _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height), Color.Black);
            
            sb.DrawString(_font, Status, GetStatusPosition(), StatusColor);
            sb.DrawString(_font, Billboard, GetBillboardPosition(), BillboardColor,
                0, Vector2.Zero, 4, SpriteEffects.None, 0);

            var loadedRect = new Rectangle((int)GetBarPosition().X, (int)GetBarPosition().Y,
               (int)(GetBarWidth() * (Value / Maximum)), (int)GetBarHeight());
            var unloadedRect = new Rectangle((int)GetBarPosition().X, (int)GetBarPosition().Y,
               (int)GetBarWidth(), (int)GetBarHeight());

            sb.Draw(_helperTexture, unloadedRect, UnloadedColor);
            sb.Draw(_helperTexture, loadedRect, LoadedColor);
            sb.End();
        }

        private int verticalOffset = 0;
        private float _verticalOffset = 0;
        private void SlideOutOffset(GameTime gameTime)
        {
            if (Value < Maximum - 0.1f) //Account for FPU errors
            {
                verticalOffset = 0;
                _verticalOffset = 0;
                return;
            }
            _verticalOffset-= (3f * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 16.66666f));
            verticalOffset = (int)_verticalOffset;
            if (_verticalOffset < _game.GraphicsDevice.Viewport.Height)
                IsSlidingOut = true;
            else
                IsSlidingOut = false;
        }

        private Vector2 GetStatusPosition()
        {
            var offset = _font.MeasureString(Status) / 2;
            var sCenter = GetScreenCenter();
            return new Vector2(sCenter.X - offset.X, sCenter.Y + GetBarHeight() + verticalOffset + offset.Y);
        }
        private Vector2 GetBillboardPosition()
        {
            var offset = _font.MeasureString(Billboard) * 2; //x4 offset
            var sCenter = GetScreenCenter();
            return new Vector2(sCenter.X, sCenter.Y + verticalOffset - GetBarHeight() * 2) - offset;
        }

        private float GetBarWidth()
        {
            return _game.GraphicsDevice.Viewport.Width * width;
        }
        private float GetBarHeight()
        {
            return _game.GraphicsDevice.Viewport.Height * height;
        }
        private Vector2 GetBarPosition()
        {
            var sCenter = GetScreenCenter();
            return new Vector2(sCenter.X - (GetBarWidth() / 2), sCenter.Y + verticalOffset - (GetBarHeight() / 2));
        }

        private Vector2 GetScreenCenter()
        {
            return new Vector2(_game.GraphicsDevice.Viewport.Width / 2, _game.GraphicsDevice.Viewport.Height / 2);
        }

        void IDisposable.Dispose()
        {
            _helperTexture.Dispose();
        }

        public void Destroy()
        {
            ((IDisposable)this).Dispose();
        }
    }
}
