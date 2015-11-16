using System;

namespace ZSB.Drm.Client.Exceptions
{
    public class AccountDetailsIncorrectException : Exception
    {
        public string LocalizableString { get; set; }
        public AccountDetailsIncorrectException(string error = null, string localizable = null)
            : base(error ?? "The entered account details could not be found")
        {
            LocalizableString = localizable ?? "username_or_password_incorrect";    
        }
    }
}
