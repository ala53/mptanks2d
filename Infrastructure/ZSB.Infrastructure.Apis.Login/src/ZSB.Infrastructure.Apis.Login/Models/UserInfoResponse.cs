using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Login.Models
{
    public class UserInfoResponse
    {
        public string Username { get; set; }
        public Guid UniqueId { get; set; }
        public bool IsPremium { get; set; }
        public DateTime AccountCreated { get; set; }

        public UserInfoResponse(UserModel user)
        {
            Username = user.Username;
            UniqueId = user.UniqueId;
            IsPremium = user.IsPremiumAccount;
            AccountCreated = user.AccountCreationDate;
        }
    }
}
