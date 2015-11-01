using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Login.Models
{
    public class ForgotPasswordDoChangeModel
    {
        public Guid ConfirmationCode { get; set; }
        public string EmailAddress { get; set; }
        public string NewPassword { get; set; }
    }
}
