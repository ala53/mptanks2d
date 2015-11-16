using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Account.Models
{
    public class ProductObject
    {
        public string KeyPrefix { get; set; }
        public string ProductName { get; set; }
        public Guid ProductId { get; set; }
        /// <summary>
        /// The short url to use for the product. Must be a-z 0-9, _, and -
        /// E.g. mp-tanks
        /// Which means:
        /// https://buy.zsbgames.me/mp-tanks -> https://www.zsbgames.me/buy/mp-tanks
        /// https://download.zsbgames.me/mp-tanks -> https://www.zsbgames.me/download/mp-tanks
        /// https://dl.zsbgames.me/mp-tanks -> https://www.zsbgames.me/download/mp-tanks
        /// </summary>
        public string ShortUrl { get; set; }
        public List<ProductEdition> Editions { get; set; }
        public DateTime CreationDate { get; set; }

        //Web purchase info
        public ProductEdition EditionToSell { get; set; }
        public decimal PriceBaseUsd { get; set; }
        public decimal SalePercentageUsd { get; set; }
        public decimal CurrentPriceUsd { get; set; }

        //Info for showing statistics
        public int TotalSales { get; set; }
        public decimal TotalAmountOwedUsd { get; set; }

        public int DownloadsSinceLastBillingPeriod { get; set; }
        public int DownloadsTotal { get; set; }
        public string DownloadUrl { get; set; }
        public string HeartbeatUrl { get; set; }
        public string BackupFileObjectName { get; set; }
        public bool ShouldDoDownloadFailover { get; set; }
    }
    public class ProductEdition
    {
        public string EditionName { get; set; }
        public Guid EditionId { get; set; }
    }
}
