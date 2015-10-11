using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Login.Models
{
    public class LoginInfoRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}