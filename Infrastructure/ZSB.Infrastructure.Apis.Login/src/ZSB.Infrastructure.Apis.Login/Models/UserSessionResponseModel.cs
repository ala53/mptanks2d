using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Login.Models
{
    public class UserSessionResponseModel
    {
        public string SessionKey { get; set; }
        public DateTime ExpiryDate { get; set; }
        public UserSessionResponseModel(UserActiveSessionModel model)
        {
            SessionKey = model.SessionKey;
            ExpiryDate = model.ExpiryDate;
        }
    }
}
