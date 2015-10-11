using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Login.Models
{
    public class CreateAccountRequestModel
    {
        public int ChallengeId { get; set; }
        public string ChallengeAnswer { get; set; }

        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
