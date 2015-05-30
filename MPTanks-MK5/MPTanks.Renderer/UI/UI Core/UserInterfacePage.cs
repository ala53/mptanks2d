
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Mvvm;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Rendering.UI
{
    public class UserInterfacePage
    {
        public UIRoot Page { get; private set; }
        public dynamic Binder { get; private set; }
        public UserInterface UserInterface { get; internal set; }

        public UserInterfacePage(string pageName)
        {
            //Generate an instance of the page
            Page = (UIRoot)Activator.CreateInstance(Type.GetType("EmptyKeys.UserInterface.Generated." + pageName, true, true), 0, 0);
            Binder = (ViewModelBase)Activator.CreateInstance(Type.GetType("MPTanks.Rendering.UI.Binders." + pageName, true, true));

            Page.DataContext = Binder;
        }

        public virtual void Update(GameTime gameTime)
        {
            Page.UpdateInput(gameTime.ElapsedGameTime.TotalMilliseconds);
            Page.UpdateLayout(gameTime.ElapsedGameTime.TotalMilliseconds);
        }
        public virtual void Draw(GameTime gameTime)
        {
            Page.Draw(gameTime.ElapsedGameTime.TotalMilliseconds);
        }
    }
}
