using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZSB.Drm.Client.Exceptions
{
    public class MultiplayerAuthTokenInvalidException : Exception
    {
        public MultiplayerAuthTokenInvalidException(string message) 
            :base(LocalizationHandler.Localize(message))
        { }
    }
}
