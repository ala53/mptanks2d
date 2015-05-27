using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Rendering.UI
{
    public class UserInterface
    {
        public UserInterfacePage UIPage
        {
            get
            {
                return _page;
            }
            set
            {
                if (_page != null)
                {
                    _transitioningBetweenPages = true;
                    _percentTransitioned = 0;
                }

                _lastPage = _page;
                _lastPage.UserInterface.IsHitTestVisible = false;
                _page = value;
                FontManager.Instance.LoadFonts(_content);
                ImageManager.Instance.LoadImages(_content);
                SoundManager.Instance.LoadSounds(_content);
            }
        }

        public dynamic ActiveBinder
        {
            get
            {
                return _page.Binder;
            }
        }

        public TimeSpan PageTransitionTime { get; set; }

        private ContentManager _content;
        private UserInterfacePage _lastPage;
        private UserInterfacePage _page;
        private GraphicsDevice _graphics;
        private EmptyKeys.UserInterface.Engine _engine;

        public UserInterface(ContentManager contentManager, GraphicsDevice gd)
        {
            _content = new ContentManager(contentManager.ServiceProvider, "assets/ui/imgs");
            _graphics = gd;
            _engine = new MonoGameEngine(gd, gd.Viewport.Width, gd.Viewport.Height);
            PageTransitionTime = TimeSpan.FromMilliseconds(500);
        }

        public void SetPage(string page)
        {
            UIPage = new UserInterfacePage(page);
        }

        private int currentWidth;
        private int currentHeight;
        public void Update(GameTime gameTime)
        {
            if (currentWidth != _graphics.Viewport.Width ||
                currentHeight != _graphics.Viewport.Height)
            {
                currentWidth = _graphics.Viewport.Width;
                currentHeight = _graphics.Viewport.Height;
                if (_page != null)
                {
                    _page.UserInterface.Resize(_graphics.Viewport.Width, _graphics.Viewport.Height);
                }
                if (_lastPage != null)
                {
                    _lastPage.UserInterface.Resize(_graphics.Viewport.Width, _graphics.Viewport.Height);
                }
            }

            if (_page != null)
                _page.Update(gameTime);
            if (_lastPage != null)
                _lastPage.Update(gameTime);
        }

        private bool _transitioningBetweenPages = false;
        private float _percentTransitioned = 0;
        public void Draw(GameTime gameTime)
        {
            if (_transitioningBetweenPages)
            {
                if (_lastPage != null)
                _lastPage.Draw(gameTime, _engine.Renderer, 1 - _percentTransitioned);
                if (_page != null)
                    _page.Draw(gameTime, _engine.Renderer, _percentTransitioned);

                //and increment the transition
                var percentageToAdd = GetPercentageStepForTransition(gameTime);
                if (_percentTransitioned + percentageToAdd >= 1)
                {
                    _transitioningBetweenPages = false;
                    _lastPage = null; //help with gc
                }
            }
            else
            {
                if (_page != null)
                    _page.Draw(gameTime, _engine.Renderer, 1);
            }
        }

        private float GetPercentageStepForTransition(GameTime gameTime)
        {
            return (float)(gameTime.ElapsedGameTime.TotalMilliseconds / PageTransitionTime.TotalMilliseconds);
        }
    }
}
