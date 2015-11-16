using System;
using System.Collections.Generic;
using System.Text;

namespace ZSB.Drm.Client.Models
{
    public class PublicUserInfo
    {
        public string Username { get; set; }
        public Guid UniqueId { get; set; }
        public DateTime AccountCreated { get; set; }
        public UserOwnedProductResponseModel[] OwnedProducts { get; set; }

        public class UserOwnedProductResponseModel
        {
            public DateTime RedemptionDate { get; set; }
            public Guid ProductId { get; set; }
            public string ProductName { get; set; }
            public Guid EditionId { get; set; }
            public string EditionName { get; set; }
            public string DisplayName { get; set; }
            /// <summary>
            /// Null unless this is a <see cref="FullUserInfo"/> instance.
            /// (The product key of the redeemed product)
            /// </summary>
            public string ProductKey { get; set; }
        }

        public bool Owns(string productId) => Owns(new Guid(productId));
        public bool Owns(Guid productId)
        {
            foreach (var product in OwnedProducts)
                if (product.ProductId == productId) return true;
            return false;
        }
        public bool Owns(string productId, string editionId) => Owns(new Guid(productId), new Guid(editionId));
        public bool Owns(Guid productId, Guid editionId)
        {
            foreach (var product in OwnedProducts)
                if (product.ProductId == productId && product.EditionId == editionId) return true;
            return false;
        }
    }
}
