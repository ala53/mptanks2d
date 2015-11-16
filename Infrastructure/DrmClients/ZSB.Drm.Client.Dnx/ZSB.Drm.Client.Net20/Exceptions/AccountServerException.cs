using System;
using System.Collections.Generic;
using System.Text;

namespace ZSB.Drm.Client.Exceptions
{
    public class AccountServerException : Exception
    {
        public AccountServerException(Exception inner = null)
            : base("An internal error occurred with the account server", inner)
        { }
    }
}
