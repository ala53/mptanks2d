using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Account.Models
{
    public class UserInfoResponseModel
    {
        public string Username { get; set; }
        public Guid UniqueId { get; set; }
        public DateTime AccountCreated { get; set; }
        public UserOwnedProductResponseModel[] OwnedProducts { get; set; }

        public UserInfoResponseModel(UserModel user)
        {
            Username = user.Username;
            UniqueId = user.UniqueId;
            AccountCreated = user.AccountCreationDate;
            OwnedProducts = user.OwnedProducts.Select(a => new UserOwnedProductResponseModel(a)).ToArray();
        }

        public class UserOwnedProductResponseModel
        {
            public DateTime RedemptionDate { get; set; }
            public Guid ProductId { get; set; }
            public string ProductName { get; set; }
            public Guid EditionId { get; set; }
            public string EditionName { get; set; }
            public string DisplayName { get; set; }

            public UserOwnedProductResponseModel(UserOwnedProductModel mdl)
            {
                RedemptionDate = mdl.RedemptionDate;
                ProductId = mdl.ProductId;
                ProductName = mdl.ProductName;
                EditionId = mdl.EditionId;
                EditionName = mdl.EditionName;
                DisplayName = mdl.DisplayName;
            }
        }
    }
}
