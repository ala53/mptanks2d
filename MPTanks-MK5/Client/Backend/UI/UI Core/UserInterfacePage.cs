
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Mvvm;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.UI
{
    public class UserInterfacePage
    {
        public static UserInterfacePage GetEmptyPageInstance()
        {
            return new UserInterfacePage("emptypage");
        }
        public UIRoot Page { get; private set; }
        public UserInterface UserInterface { get; internal set; }
        public string Name { get; private set; }
        public int Id { get; private set; }
        private static int _id;
        public event EventHandler<GameTime> OnUpdate = delegate { };
        internal Action<UserInterfacePage> Generator { get; set; }
        internal Action<UserInterfacePage, dynamic> StateChangeHandler { get; set; }
        private Action _updater = delegate { };
        public object StateObject { get; set; }

        public UserInterfacePage(string pageName)
        {
            Id = _id++;
            Name = pageName;
            //Generate an instance of the page
            Page = (UIRoot)Activator.CreateInstance(Type.GetType("EmptyKeys.UserInterface.Generated." + pageName, true, true), 0, 0);
            Page.EnabledMultiThreadLocking = true;
        }

        private UserInterfacePage() { }

        public void RegisterUpdater(Action updater) => _updater = updater ?? delegate { };

        public virtual void Update(GameTime gameTime, bool isActiveWindow)
        {
            _updater();
            if (isActiveWindow)
                Page.UpdateInput(gameTime.ElapsedGameTime.TotalMilliseconds);

            OnUpdate(this, gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            //There's some weird deadlock going on here (everything just freezes)
            //So: we should investigate it 
            Page.UpdateLayout(gameTime.ElapsedGameTime.TotalMilliseconds);
            Page.Draw(gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        /// <summary>
        /// Gets a field or property from the state in the specified fashion
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T State<T>(string name)
        {
            if (StateObject == null) return default(T);

            var prop = StateObject.GetType().GetProperty(name,
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.GetProperty |
                System.Reflection.BindingFlags.Default |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.IgnoreCase);
            var field = StateObject.GetType().GetProperty(name,
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.GetProperty |
                System.Reflection.BindingFlags.Default |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.IgnoreCase);

            if (prop == null && field == null) return default(T);

            if (prop != null)
            {
                //Type check
                if (typeof(T) != prop.PropertyType && !typeof(T).IsSubclassOf(prop.PropertyType))
                    return default(T); // wrong type
                return (T)prop.GetValue(StateObject);
            }
            else
            {
                //Type check
                if (typeof(T) != field.PropertyType && !typeof(T).IsSubclassOf(field.PropertyType))
                    return default(T); // wrong type

                return (T)field.GetValue(StateObject);
            }
        }

        /// <summary>
        /// Lets you query an element and take a shortcut to avoid repeated calls to this by passing a scoped function in.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="processor"></param>
        /// <returns></returns>
        public T Element<T>(string name, Action<T> processor) where T : UIElement
        {
            var elem = Element<T>(name);
            processor(elem);
            return elem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual T Element<T>(string name) where T : UIElement
        {
            var field = Page.GetType().GetField(name,
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Default |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.IgnoreCase);
            if (field == null) return null;
            if (!field.FieldType.IsSubclassOf(typeof(T)) && field.FieldType != typeof(T)) return null;
            return (T)field.GetValue(Page);
        }
    }
}
