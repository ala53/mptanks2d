using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Account.Models
{
    public class ChangeUsernameRequestModel
    {
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewUsername { get; set; }
    }
}
