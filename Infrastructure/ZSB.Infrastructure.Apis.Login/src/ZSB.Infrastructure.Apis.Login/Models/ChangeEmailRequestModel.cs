using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Login.Models
{
    public class ChangeEmailRequestModel : AuthenticatedRequestModel
    {
        public string NewEmailAddress { get; set; }
    }
}
