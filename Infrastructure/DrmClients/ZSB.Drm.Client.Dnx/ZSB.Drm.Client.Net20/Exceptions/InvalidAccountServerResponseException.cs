using System;
using System.Collections.Generic;
using System.Text;

namespace ZSB.Drm.Client.Exceptions
{
    /// <summary>
    /// The account server responded with an unexpected result that could not be parsed.
    /// </summary>
    public class InvalidAccountServerResponseException : Exception
    {
        public InvalidAccountServerResponseException(Exception inner = null) 
            :base(LocalizationHandler.Localize("invalid_response"), inner)
        { }
    }
}
