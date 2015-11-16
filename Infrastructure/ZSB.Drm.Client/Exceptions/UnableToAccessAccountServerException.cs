using System;
using System.Collections.Generic;
using System.Text;

namespace ZSB.Drm.Client.Exceptions
{
    /// <summary>
    /// An exception to signal that we cannot reach the account server. This could mean one of 2 things:
    /// (a) you are offline or (b) the account server is offline
    /// </summary>
    public class UnableToAccessAccountServerException : Exception
    {
        public UnableToAccessAccountServerException(Exception inner = null) 
            :base(LocalizationHandler.Localize("timed_out"), inner)
        {
        }
    }
}
