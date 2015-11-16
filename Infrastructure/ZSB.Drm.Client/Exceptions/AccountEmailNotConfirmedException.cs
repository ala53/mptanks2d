using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZSB.Drm.Client.Exceptions
{
    public class AccountEmailNotConfirmedException : Exception
    {
        public AccountEmailNotConfirmedException() :
            base(LocalizationHandler.Localize("email_not_confirmed"))
        { }
    }
}
