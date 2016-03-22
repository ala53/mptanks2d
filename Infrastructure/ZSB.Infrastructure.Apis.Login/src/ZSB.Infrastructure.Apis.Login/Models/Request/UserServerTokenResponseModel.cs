using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Account.Models
{
    public class UserServerTokenResponseModel
    {
        [Required]
        public string Token { get; set; }
        public UserServerTokenResponseModel(UserServerTokenModel model)
        {
            Token = model.ServerToken;
        }
    }
}
