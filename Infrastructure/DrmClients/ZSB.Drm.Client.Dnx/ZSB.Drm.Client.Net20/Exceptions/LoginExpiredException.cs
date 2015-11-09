using System;
using System.Collections.Generic;
using System.Text;

namespace ZSB.Drm.Client.Exceptions
{
    public class LoginExpiredException : Exception
    {
        public LoginExpiredException() :
            base("The current login has expired. Call DrmClient.Login(username, password) to log back in.")
        { }
    }
}
