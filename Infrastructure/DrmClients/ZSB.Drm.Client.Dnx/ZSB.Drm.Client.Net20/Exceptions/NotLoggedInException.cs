using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZSB.Drm.Client.Exceptions
{
    public class NotLoggedInException : Exception
    {
        public NotLoggedInException() 
            :base(LocalizationHandler.Localize("not_logged_in"))
        { }
    }
}
