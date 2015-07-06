using EmptyKeys.UserInterface.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.UI
{
    public abstract class BinderBase : ViewModelBase
    {
        public UserInterfacePage Owner { get; set; }
    }
}
