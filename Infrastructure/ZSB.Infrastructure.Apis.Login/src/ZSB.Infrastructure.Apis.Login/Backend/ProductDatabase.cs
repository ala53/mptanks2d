using Microsoft.Framework.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSB.Infrastructure.Apis.Account.Models;

namespace ZSB.Infrastructure.Apis.Account.Backend
{
    public class ProductDatabase
    {

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

        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions
        {
            CompactOnMemoryPressure = true,
            ExpirationScanFrequency = TimeSpan.FromMinutes(1)
        });

        static ProductDatabase()
        {
        }

        public static async Task<ProductEdition> GetProduct(Guid product, Guid edition)
        {
            var combinedKey = new DoubleGuid { First = product, Second = edition };
            object result;
            //Check the cache
            if (_cache.TryGetValue(combinedKey, out result))
                return (ProductEdition)result;

            //Not in cache
            //Do a request against the drmdev server for more info
            var rest = await Rest.RestHelper.DoGet<ProductEdition>(
                Startup.Configuration["Data:DrmDevServerAddress"] +
                $"{Startup.Configuration["Data:DrmDevServerInternalAPIKey"]}/product/by/uniqueid/{product}/{edition}");

            if (rest.Error) return null;

            //Add to cache
            _cache.Set(combinedKey, rest.Data, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            });

            return rest.Data;
        }
    }
}
