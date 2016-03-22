using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Account.Models
{
    public class GiftProductRequestModel : AuthenticatedRequestModel
    {
        [Required]
        public string ProductKeyToGift { get; set; }
        [Required]
        public string EmailAddressToGiftTo { get; set; }
    }
}
