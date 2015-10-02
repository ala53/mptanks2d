using EmptyKeys.UserInterface;
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
        public dynamic ActiveBinder => CurrentPage?.Binder;
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
            GoBack();
        }

        public UserInterfacePage GoToPage(string name)
        {
            var pg = new UserInterfacePage(name);
            pg.UserInterface = this;
            pg.Page.Resize(_currentWidth, _currentHeight);
            _pages.Push(pg);
            FontManager.Instance.LoadFonts(_content);
            ImageManager.Instance.LoadImages(_content);
            SoundManager.Instance.LoadSounds(_content);

            return pg;
        }

        public void UnwindPageStack()
        {
            _pages.Clear();
            GoBack();
        }
        /// <summary>
        /// Goes back 1 page in the "history" list
        /// </summary>
        public void GoBack()
        {
            if (_pages.Count == 0) return;
            _pages.Pop();
            if (_pages.Count == 0)
            {
                //Add a new "empty" page to the list
                GoToPage("emptypage");
            }
            FixPageStack();
        }

        public void Resize(int newWidth, int newHeight)
        {
            foreach (var pg in _pages)
                pg.Page.Resize(newWidth, newHeight);
            _currentWidth = newWidth;
            _currentHeight = newHeight;
        }

        private void FixPageStack()
        {
            if (_pages.Count == 0) GoBack(); //shortcut to recreate empty page
            var p = _pages.Pop();
            _pages.Push(
                CrappyReflectionHackToFixBrokenBindersBreakingThePagesInEmptyKeysBecauseFckLogic(p));
            Resize(_currentWidth, _currentHeight);
        }

        private UserInterfacePage
            CrappyReflectionHackToFixBrokenBindersBreakingThePagesInEmptyKeysBecauseFckLogic(UserInterfacePage page)
        {
            var newPage = new UserInterfacePage(page.Page.GetType().Name);
            ViewModelBase binder = page.Binder;
            ViewModelBase newBinder = newPage.Binder;

            foreach (var property in binder.GetType().GetProperties(
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Public
                ))
            {
                if (property.CanWrite && property.CanRead)
                {
                    var value = property.GetValue(binder);
                    property.SetValue(newBinder, value);
                }
            }

            if (newBinder is BinderBase)
                (newBinder as BinderBase).Owner = newPage;
            return newPage;
        }

        public void Update(GameTime gameTime)
        {
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

        public void ShowMessageBox(string content, string header = "",
            MessageBoxType type = MessageBoxType.OKMessageBox,
            MessageBoxButtons buttons = MessageBoxButtons.Ok,
            Action<MessageBoxResult> callback = null)
        {
            if (callback == null) callback = delegate { };
            content = string.Join("\n", ChunksUpto(content, 30));
            GoToPage(type.ToString());

            CurrentPage.Binder.Header = header;
            CurrentPage.Binder.Content = content;
            CurrentPage.Binder.Buttons = buttons;
            CurrentPage.Binder.OkCommand = new RelayCommand((obj) =>
            {
                GoBack();
                callback(MessageBoxResult.Ok);
            });
            CurrentPage.Binder.CancelCommand = new RelayCommand((obj) =>
            {
                GoBack();
                callback(MessageBoxResult.Cancel);
            });
            CurrentPage.Binder.YesCommand = new RelayCommand((obj) =>
            {
                GoBack();
                callback(MessageBoxResult.Yes);
            });
            CurrentPage.Binder.NoCommand = new RelayCommand((obj) =>
            {
                GoBack();
                callback(MessageBoxResult.No);
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
