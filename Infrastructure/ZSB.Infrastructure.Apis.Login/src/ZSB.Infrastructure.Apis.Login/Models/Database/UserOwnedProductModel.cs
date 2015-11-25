using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Account.Models
{
    public class UserOwnedProductModel
    {
        public ProductEdition Edition => Backend.ProductDatabase.GetProduct(ProductId, EditionId).Result;
        public DateTime RedemptionDate { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName => Edition.Product.ProductName;
        public Guid EditionId { get; set; }
        public string EditionName => Edition.EditionName;
        public string DisplayName => ProductName + " (" + EditionName + ")";
        public string ProductKey { get; set; }
        public UserModel Owner { get; set; }
    }
}
