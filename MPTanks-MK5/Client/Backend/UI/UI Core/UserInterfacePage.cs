
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
        public dynamic Binder { get; private set; }
        public UserInterface UserInterface { get; internal set; }
        public string Name { get; private set; }
        public int Id { get; private set; }
        private static int _id;

        public UserInterfacePage(string pageName)
        {
            Id = _id++;
            Name = pageName;
            //Generate an instance of the page
            Page = (UIRoot)Activator.CreateInstance(Type.GetType("EmptyKeys.UserInterface.Generated." + pageName, true, true), 0, 0);
            var binderType = Type.GetType("MPTanks.Client.Backend.UI.Binders." + pageName, true, true);
            if (binderType != null)
                Binder = Activator.CreateInstance(binderType);
            else Binder = new Binders.EmptyPage();

            if (Binder is BinderBase)
            {
                Binder.Owner = this;
                Binder.Recreated();
            }

            Page.DataContext = Binder;
        }

        private UserInterfacePage() { }

        public virtual void Update(GameTime gameTime, bool isActiveWindow)
        {
            //if (isActiveWindow)
            Page.UpdateInput(gameTime.ElapsedGameTime.TotalMilliseconds);
            Page.UpdateLayout(gameTime.ElapsedGameTime.TotalMilliseconds);

            Binder.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            Page.Draw(gameTime.ElapsedGameTime.TotalMilliseconds);
        }
    }
}
