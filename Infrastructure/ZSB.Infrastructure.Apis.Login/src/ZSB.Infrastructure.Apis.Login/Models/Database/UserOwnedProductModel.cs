using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Account.Models
{
    public class UserOwnedProductModel
    {
        public DateTime RedemptionDate { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public Guid EditionId { get; set; }
        public string EditionName { get; set; }
        public string DisplayName => ProductName + "(" + EditionName + ")";
        public string ProductKey { get; set; }
        public UserModel Owner { get; set; }
    }
}
