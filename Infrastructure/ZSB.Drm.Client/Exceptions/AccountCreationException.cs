using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZSB.Drm.Client.Exceptions
{
    public class AccountCreationException : Exception
    {
        public bool ChallengeIncorrect { get; set; }
        public AccountCreationException(string message)
            : base(LocalizationHandler.Localize(message))
        { }
    }
}
