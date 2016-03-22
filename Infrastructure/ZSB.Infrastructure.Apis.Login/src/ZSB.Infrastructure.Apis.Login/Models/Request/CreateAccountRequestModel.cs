using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Account.Models
{
    public class CreateAccountRequestModel
    {
        [Required]
        public int ChallengeId { get; set; }
        [Required]
        public string ChallengeAnswer { get; set; }

        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
