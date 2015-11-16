using System;
using System.Collections.Generic;
using System.Text;

namespace ZSB.Drm.Client.Exceptions
{
    public class PersistentStorageInvalidException : Exception
    {
        public PersistentStorageInvalidException(Exception inner) :
            base(LocalizationHandler.Localize("persistent_storage_invalid"), inner)
        { }
    }
}
