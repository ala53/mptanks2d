using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.ProductKey.Generator
{
    class ProductKeyStorageModel : TableEntity
    {
        public string DisplayName { get; set; }
        public string ProductName { get; set; }
        public Guid ProductId { get; set; }
        public string EditionName { get; set; }
        public Guid EditionId { get; set; }
        public string Key { get; set; }
        public string Prefix { get; set; }
        public DateTime KeyCreationDate { get; set; }

        public bool HasBeenRedeemed { get; set; }
        public Guid RedeemerAccountId { get; set; }
        public string RedeemerAccountEmailAddress { get; set; }
        public DateTime RedemptionDate { get; set; }
    }
}
