
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
        public object StateObject { get; set; }

        public UserInterfacePage(string pageName)
        {
            Id = _id++;
            Name = pageName;
            //Generate an instance of the page
            Page = (UIRoot)Activator.CreateInstance(Type.GetType("EmptyKeys.UserInterface.Generated." + pageName, true, true), 0, 0);
        }

        private UserInterfacePage() { }

        public virtual void Update(GameTime gameTime, bool isActiveWindow)
        {
            if (isActiveWindow)
                Page.UpdateInput(gameTime.ElapsedGameTime.TotalMilliseconds);
            Page.UpdateLayout(gameTime.ElapsedGameTime.TotalMilliseconds);

            OnUpdate(this, gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
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
