using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Mvvm;
using EmptyKeys.UserInterface.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MPTanks.Client.Backend.UI.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.UI
{
    public class UserInterface
    {
        private int _currentWidth, _currentHeight;
        private int _lastWidth, _lastHeight;
        private Stack<UserInterfacePage> _pages = new Stack<UserInterfacePage>();
        private GraphicsDevice _gd => _game.GraphicsDevice;
        private Game _game;
        private EmptyKeys.UserInterface.Engine _engine;
        private ContentManager _content;
        public UserInterfacePage CurrentPage => _pages.Count > 0 ? _pages.Peek() : null;
        public object State
        {
            get { return CurrentPage.State; }
            set { UpdateState(value); }
        }
        public bool IsActive { get; set; }
        /// <summary>
        /// UNUSED
        /// </summary>
        public TimeSpan PageTransitionTime { get; set; }

        public UserInterface(Game game)
        {
            _game = game;
            _engine = new MonoGameEngine(_gd, 0, 0);
            _content = new ContentManager(_game.Content.ServiceProvider, "assets/ui/imgs");
            SpriteFont font = _content.Load<SpriteFont>("Segoe_UI_12_Regular");
            FontManager.DefaultFont = EmptyKeys.UserInterface.Engine.Instance.Renderer.CreateFont(font);
            PageTransitionTime = TimeSpan.FromMilliseconds(500);
            Empty();
        }

        public void GoToPage(string page, Action<UserInterfacePage> generator) =>
            GoToPage<object>(page, (a, b, c) => generator(a));
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="generator">A generator for the page which takes the page to draw to, 
        /// an object state, and (optionally) the last page, 
        /// which will be included when GoBack() is called or the state object is updated</param>
        /// <param name="state"></param>
        public void GoToPage<T>(string page, Action<UserInterfacePage, dynamic, OldPageObject> generator, T state = null) where T : class
        {
            var pg = new UserInterfacePage(page);
            pg.UserInterface = this;
            pg.Page.Resize(_currentWidth, _currentHeight);
            pg.Generator = generator;
            pg.State = state;
            _pages.Push(pg);
            FontManager.Instance.LoadFonts(_content);
            ImageManager.Instance.LoadImages(_content);
            SoundManager.Instance.LoadSounds(_content);

            generator(pg, state, null);
        }

        public void UpdateState<T>(T newState) where T : class
        {
            var currentPage = CurrentPage;
            currentPage.Generator.DynamicInvoke(currentPage, newState, new OldPageObject { OldPage = currentPage });
        }

        private UserInterfacePage CopyCurrentPage()
        {
            var oldPg = _pages.Pop();
            var pg = new UserInterfacePage(oldPg.Name);
            pg.UserInterface = this;
            pg.Page.Resize(_currentWidth, _currentHeight);
            pg.Generator = oldPg.Generator;
            pg.State = pg.State;
            _pages.Push(pg);
            FontManager.Instance.LoadFonts(_content);
            ImageManager.Instance.LoadImages(_content);
            SoundManager.Instance.LoadSounds(_content);
            return pg;
        }

        public void GoBack()
        {
            //pop the stack
            if (_pages.Count > 1)
            {
                _pages.Pop();
                var oldPg = CurrentPage;
                var newPg = CopyCurrentPage();
                newPg.Generator.DynamicInvoke(newPg, newPg.State, new OldPageObject { OldPage = oldPg });
            }
            //otherwise
            else UnwindAndEmpty();
        }

        public void UnwindAndEmpty()
        {
            _pages.Clear();
            Empty();
        }

        public bool IsOnPage(string name) => CurrentPage == null ?
            false : CurrentPage.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase);

        public void Empty()
        {
            if (!IsOnPage("emptypage"))
                GoToPage("emptypage", a => { });
        }

        public void Resize(int newWidth, int newHeight)
        {
            foreach (var pg in _pages)
                pg.Page.Resize(newWidth, newHeight);
            _currentWidth = newWidth;
            _currentHeight = newHeight;
        }

        public void Update(GameTime gameTime)
        {
            IsActive = _game.IsActive;
            var vp = _game.GraphicsDevice.Viewport;
            if (vp.Width != _lastWidth || vp.Height != _lastHeight)
                Resize(vp.Width, vp.Height);
            _lastWidth = vp.Width;
            _lastHeight = vp.Height;

            CurrentPage.Update(gameTime, IsActive);
        }

        public void Draw(GameTime gameTime)
        {
            CurrentPage.Draw(gameTime);
        }

        #region Message Boxes
        public enum MessageBoxType
        {
            OKMessageBox,
            WarningMessageBox,
            ErrorMessageBox
        }
        public enum MessageBoxButtons
        {
            Ok,
            OkCancel,
            YesNo,
            YesNoCancel,
            OkNo,
            OkNoCancel
        }

        public enum MessageBoxResult
        {
            Ok,
            Cancel,
            Yes,
            No
        }

        public void ShowMessageBox(string header, string content,
            MessageBoxType type = MessageBoxType.OKMessageBox,
            MessageBoxButtons buttons = MessageBoxButtons.Ok,
            Action<MessageBoxResult> callback = null)
        {
            if (callback == null) callback = delegate { };
            content = string.Join("\n", ChunksUpto(content, 30));
            GoToPage(type.ToString(), (p) =>
            {
                p.Element<TextBlock>("Header").Text = header;
                p.Element<TextBlock>("ContentT").Text = content;

                new List<string>() { "Ok", "Yes", "No", "Cancel" }.ForEach(a =>
                {
                    p.Element<Button>(a).Visibility = Visibility.Collapsed;
                    p.Element<Button>(a).Click += (c, d) =>
                    {
                        callback((MessageBoxResult)Enum.Parse(typeof(MessageBoxResult), a));
                        GoBack();
                    };
                });

                List<string> visible = new List<string>();

                switch (buttons)
                {
                    case MessageBoxButtons.Ok:
                        visible.Add("Ok");
                        break;
                    case MessageBoxButtons.OkCancel:
                        visible.Add("Ok");
                        visible.Add("Cancel");
                        break;
                    case MessageBoxButtons.OkNo:
                        visible.Add("Ok");
                        visible.Add("No");
                        break;
                    case MessageBoxButtons.OkNoCancel:
                        visible.Add("Ok");
                        visible.Add("No");
                        visible.Add("Cancel");
                        break;
                    case MessageBoxButtons.YesNo:
                        visible.Add("Yes");
                        visible.Add("No");
                        break;
                    case MessageBoxButtons.YesNoCancel:
                        visible.Add("Yes");
                        visible.Add("No");
                        visible.Add("Cancel");
                        break;
                }

                visible.ForEach(a => p.Element<Button>(a).Visibility = Visibility.Visible);
            });
        }
        static IEnumerable<string> ChunksUpto(string str, int maxChunkSize)
        {
            for (int i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }
        #endregion
    }
}
