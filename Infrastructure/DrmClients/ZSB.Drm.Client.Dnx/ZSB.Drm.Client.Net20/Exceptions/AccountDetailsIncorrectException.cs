using System;

namespace ZSB.Drm.Client.Exceptions
{
    public class AccountDetailsIncorrectException : Exception
    {
        public AccountDetailsIncorrectException(string error = null)
            : base(LocalizationHandler.Localize(error ?? "username_or_password_incorrect"))
        {
        }
    }
}
