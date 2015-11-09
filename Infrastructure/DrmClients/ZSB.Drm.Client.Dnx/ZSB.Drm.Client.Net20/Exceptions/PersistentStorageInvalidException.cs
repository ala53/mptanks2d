using System;
using System.Collections.Generic;
using System.Text;

namespace ZSB.Drm.Client.Exceptions
{
    public class PersistentStorageInvalidException : Exception
    {
        public PersistentStorageInvalidException(Exception inner) :
            base("The supplied persistent storage data is invalid.", inner)
        { }
    }
}
