using System;
using System.Collections.Generic;
using System.Text;

namespace ZSB.Drm.Client.Exceptions
{
    public class AccountServerException : Exception
    {
        /// <summary>
        /// An exception that occurred somewhere between the client library and the account server.
        /// It could be a bug in the library or it could be a bug in the account server. Either way,
        /// report it to us so we can figure it out. This is (almost) never caused by your code.
        /// </summary>
        /// <param name="inner"></param>
        public AccountServerException(Exception inner = null)
            : base(LocalizationHandler.Localize("unknown_error"), inner)
        { }
    }
}
