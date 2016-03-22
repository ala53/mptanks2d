using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Account.Models
{
    public class ChangeEmailRequestModel
    {
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewEmailAddress { get; set; }
    }
}
