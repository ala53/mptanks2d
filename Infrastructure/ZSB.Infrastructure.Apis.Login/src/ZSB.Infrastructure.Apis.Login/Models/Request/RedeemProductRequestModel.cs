using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Account.Models
{
    public class RedeemProductRequestModel : AuthenticatedRequestModel
    {
        [Required]
        public string ProductKey { get; set; }
    }
}
