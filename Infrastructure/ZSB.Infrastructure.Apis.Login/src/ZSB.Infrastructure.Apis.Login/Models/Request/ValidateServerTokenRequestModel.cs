using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Account.Models
{
    public class ValidateServerTokenRequestModel
    {
        [Required]
        public string ServerToken { get; set; }
    }
}
