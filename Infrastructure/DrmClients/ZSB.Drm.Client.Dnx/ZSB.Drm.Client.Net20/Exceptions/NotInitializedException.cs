using System;
using System.Collections.Generic;
using System.Text;

namespace ZSB.Drm.Client.Exceptions
{
    public class NotInitializedException : Exception
    {
        public NotInitializedException() 
            :base("DrmClient.Initialize()/InitializeAsync() has not been called yet!")
        { }
    }
}
