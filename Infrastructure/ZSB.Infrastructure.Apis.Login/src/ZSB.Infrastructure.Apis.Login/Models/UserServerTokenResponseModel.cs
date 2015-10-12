using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Login.Models
{
    public class UserServerTokenResponseModel 
    {
        public string Token { get; set; }
        public UserServerTokenResponseModel(UserServerTokenModel model)
        {
            Token = model.ServerToken;
        }
    }
}
