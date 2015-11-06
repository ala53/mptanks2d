using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Account.Backend
{
    public class ProductDatabase
    {
        public class ProductObject
        {
            public string ProductName { get; set; }
            public Guid ProductId { get; set; }
            public string EditionName { get; set; }
            public Guid EditionId { get; set; }
            public string DownloadUrl { get; set; }
        }

        private struct DoubleGuid
        {
            public Guid First, Second;
            public override bool Equals(object obj)
            {
                if (obj is DoubleGuid)
                    if (First == ((DoubleGuid)obj).First && Second == ((DoubleGuid)obj).Second)
                        return true;

                return false;
            }

            public override int GetHashCode()
            {
                return First.GetHashCode() ^ Second.GetHashCode();
            }
        }

        private static Dictionary<DoubleGuid, ProductObject> _products = new Dictionary<DoubleGuid, ProductObject>();

        static ProductDatabase()
        {
            AddProduct(new ProductObject
            {
                DownloadUrl = "https://mptanks.zsbgames.me/download",
                ProductName = "MP Tanks 2D",
                ProductId = new Guid("6de5dd93-2c13-4a28-b722-4a28b79c67b8"),
                EditionName = "ZSB Store Purchase",
                EditionId = new Guid("d26fb367-4cc9-4854-a004-90616c23b6ab")
            });
            AddProduct(new ProductObject
            {
                DownloadUrl = "https://mptanks.zsbgames.me/download",
                ProductName = "MP Tanks 2D",
                ProductId = new Guid("6de5dd93-2c13-4a28-b722-4a28b79c67b8"),
                EditionName = "Humble Widget / Store Purchase",
                EditionId = new Guid("0bcd1355-cc0f-46a3-8d11-d6b3fa12d455")
            });
            AddProduct(new ProductObject
            {
                DownloadUrl = "https://mptanks.zsbgames.me/download",
                ProductName = "MP Tanks 2D",
                ProductId = new Guid("6de5dd93-2c13-4a28-b722-4a28b79c67b8"),
                EditionName = "Steam Purchase",
                EditionId = new Guid("e907a4ab-2b39-4e16-a9fc-5e0bf5872b53")
            });
            AddProduct(new ProductObject
            {
                DownloadUrl = "https://mptanks.zsbgames.me/download",
                ProductName = "MP Tanks 2D",
                ProductId = new Guid("6de5dd93-2c13-4a28-b722-4a28b79c67b8"),
                EditionName = "ZSB Gifted Key",
                EditionId = new Guid("e6c94547-355b-4d39-a5f0-513a3cc3b807")
            });


            AddProduct(new ProductObject
            {
                DownloadUrl = "https://mptanks.zsbgames.me/download",
                ProductName = "MP Tanks 2D Premium Addon",
                ProductId = new Guid("a8e46236-ce79-4480-aeed-28faec2bc0b8"),
                EditionName = "ZSB Store Purchase",
                EditionId = new Guid("d26fb367-4cc9-4854-a004-90616c23b6ab")
            });
            AddProduct(new ProductObject
            {
                DownloadUrl = "https://mptanks.zsbgames.me/download",
                ProductName = "MP Tanks 2D Premium Addon",
                ProductId = new Guid("a8e46236-ce79-4480-aeed-28faec2bc0b8"),
                EditionName = "Humble Widget / Store Purchase",
                EditionId = new Guid("0bcd1355-cc0f-46a3-8d11-d6b3fa12d455")
            });
            AddProduct(new ProductObject
            {
                DownloadUrl = "https://mptanks.zsbgames.me/download",
                ProductName = "MP Tanks 2D Premium Addon",
                ProductId = new Guid("a8e46236-ce79-4480-aeed-28faec2bc0b8"),
                EditionName = "Steam Purchase",
                EditionId = new Guid("e907a4ab-2b39-4e16-a9fc-5e0bf5872b53")
            });
            AddProduct(new ProductObject
            {
                DownloadUrl = "https://mptanks.zsbgames.me/download",
                ProductName = "MP Tanks 2D Premium Addon",
                ProductId = new Guid("a8e46236-ce79-4480-aeed-28faec2bc0b8"),
                EditionName = "ZSB Gifted Key",
                EditionId = new Guid("e6c94547-355b-4d39-a5f0-513a3cc3b807")
            });
        }

        public static void AddProduct(ProductObject obj)
        {
            _products.Add(new DoubleGuid { First = obj.ProductId, Second = obj.EditionId }, obj);
        }

        public static ProductObject GetProduct(Guid product, Guid edition)
        {
            if (_products.ContainsKey(new DoubleGuid { First = product, Second = edition }))
                return _products[new DoubleGuid { First = product, Second = edition }];

            return null;
        }
    }
}
