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
        public EmptyKeys.UserInterface.Engine _engine;
        private ContentManager _fontContentManager, _soundAndImagesContentManager;
        public UserInterfacePage CurrentPage => _pages.Count > 0 ? _pages.Peek() : null;
        public object State
        {
            get { return CurrentPage.StateObject; }
            set { UpdateState(value); }
        }
        public bool IsActive { get; set; }
        /// <summary>
        /// UNUSED
        /// </summary>
        public TimeSpan PageTransitionTime { get; set; }

        public SpriteFont DefaultFont { get; private set; }

        public UserInterface(Game game)
        {
            _game = game;
            _engine = new MonoGameEngine(_gd, 0, 0);
            _fontContentManager = new ContentManager(_game.Content.ServiceProvider, "assets/ui/imgs");
            _soundAndImagesContentManager = new ContentManager(_game.Content.ServiceProvider, "");
            SpriteFont font = _fontContentManager.Load<SpriteFont>("JHUF_12_Regular");
            DefaultFont = font;
            FontManager.DefaultFont = EmptyKeys.UserInterface.Engine.Instance.Renderer.CreateFont(font);
            PageTransitionTime = TimeSpan.FromMilliseconds(500);
            Empty();
        }
        public void GoToPageIfNotThere(string page, Action<UserInterfacePage> generator, Action<UserInterfacePage, dynamic> stateChangeHandler = null, dynamic state = null)
        {
            if (!IsOnPage(page))
                GoToPage(page, generator, stateChangeHandler, null);
        }

        public void GoToPage(string page, Action<UserInterfacePage> generator, Action<UserInterfacePage, dynamic> stateChangeHandler = null, dynamic state = null)
        {
            var pg = new UserInterfacePage(page);
            if (stateChangeHandler == null)
                stateChangeHandler = delegate { };
            pg.UserInterface = this;
            pg.Page.Resize(_currentWidth, _currentHeight);
            pg.Generator = generator;
            pg.StateChangeHandler = stateChangeHandler;
            pg.StateObject = state;
            _pages.Push(pg);
            FontManager.Instance.LoadFonts(_fontContentManager);
            //Hack: EmptyKeys stores the names for fonts as <FontName>
            //while it stores images and sounds as <folderName>/<fileName>.
            //So, we have to load from a different base directory.
            ImageManager.Instance.LoadImages(_soundAndImagesContentManager);
            SoundManager.Instance.LoadSounds(_soundAndImagesContentManager);

            generator(pg);
            if (state != null)
                stateChangeHandler(pg, state);
        }

        public void UpdateState<T>(T newState) where T : class
        {
            CurrentPage.StateObject = newState;
            CurrentPage.StateChangeHandler(CurrentPage, newState);
        }

        private bool _hasPendingCopy;
        private void CopyCurrentPage()
        {
            _hasPendingCopy = true;
        }

        private void __CopyPage()
        {

            var oldPg = _pages.Pop();
            var pg = new UserInterfacePage(oldPg.Name);
            pg.UserInterface = this;
            pg.Page.Resize(_currentWidth, _currentHeight); //This dead locks when called from another thread
            pg.Generator = oldPg.Generator;
            pg.StateChangeHandler = oldPg.StateChangeHandler;
            pg.StateObject = oldPg.StateObject;
            pg.Generator(pg);
            pg.StateChangeHandler(pg, pg.StateObject);
            _pages.Push(pg);

            FontManager.Instance.LoadFonts(_fontContentManager);
            ImageManager.Instance.LoadImages(_soundAndImagesContentManager);
            SoundManager.Instance.LoadSounds(_soundAndImagesContentManager);
        }

        public void GoBack()
        {
            if (_pages.Count > 1)
            {
                _pages.Pop();
                CopyCurrentPage();
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
        public bool IsOnEmpty() => CurrentPage == null ?
            false : CurrentPage.Name.Equals("emptypage", StringComparison.InvariantCultureIgnoreCase);

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

            if (_hasPendingCopy)
            {
                _hasPendingCopy = false;
                __CopyPage();
            }

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
            OkNoCancel,
            None
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
            content = string.Join("\n", SplitString(content, 30));
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
                    case MessageBoxButtons.None:
                        break;
                }

                visible.ForEach(a => p.Element<Button>(a).Visibility = Visibility.Visible);
            });
        }
        public static string SplitStringIntoLines(string stringToSplit, int maximumLineLength)
        {
            return string.Join("\n", SplitString(stringToSplit, maximumLineLength));
        }
        public static IEnumerable<string> SplitString(string stringToSplit, int maximumLineLength)
        {
            var words = stringToSplit.Split(' ').Concat(new[] { "" });
            return
                words
                    .Skip(1)
                    .Aggregate(
                        words.Take(1).ToList(),
                        (a, w) =>
                        {
                            var last = a.Last();
                            while (last.Length > maximumLineLength)
                            {
                                a[a.Count() - 1] = last.Substring(0, maximumLineLength);
                                last = last.Substring(maximumLineLength);
                                a.Add(last);
                            }
                            var test = last + " " + w;
                            if (test.Length > maximumLineLength)
                            {
                                a.Add(w);
                            }
                            else
                            {
                                a[a.Count() - 1] = test;
                            }
                            return a;
                        });
        }
        #endregion
    }
}
