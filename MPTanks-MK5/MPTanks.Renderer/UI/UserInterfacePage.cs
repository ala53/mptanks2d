
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
        public UIRoot UserInterface { get; private set; }
        public dynamic Binder { get; private set; }

        public UserInterfacePage(string pageName)
        {
            //Generate an instance of the page
            UserInterface = (UIRoot)Activator.CreateInstance(Type.GetType("EmptyKeys.UserInterface.Generated." + pageName, true, true), 0, 0);
            Binder = (ViewModelBase)Activator.CreateInstance(Type.GetType("MPTanks.Rendering.UI.Binders." + pageName, true, true));

            UserInterface.DataContext = Binder;
        }

        public virtual void Update(GameTime gameTime)
        {
            UserInterface.UpdateInput(gameTime.ElapsedGameTime.TotalMilliseconds);
            UserInterface.UpdateLayout(gameTime.ElapsedGameTime.TotalMilliseconds);
        }
        public virtual void Draw(GameTime gameTime, 
            EmptyKeys.UserInterface.Renderers.Renderer renderer, float opacity = 1)
        {
            UserInterface.Draw(gameTime.ElapsedGameTime.TotalMilliseconds);
        }
    }
}
