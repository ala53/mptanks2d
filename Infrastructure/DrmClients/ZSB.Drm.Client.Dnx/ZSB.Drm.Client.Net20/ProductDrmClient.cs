using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZSB.Drm.Client.Exceptions;

namespace ZSB.Drm.Client
{
    public class ProductDrmClient
    {
        internal ProductDrmClient() { }
        public bool Redeem(string productKey) => RedeemAsync(productKey).Result;

        public Task<bool> RedeemAsync(string productKey)
        {
            if (!DrmClient._initialized) throw new NotInitializedException();
            return Task.Run(() => { return true; });
        }

        public void SendGiftRequestEmail(string productKey, string recipientEmail) =>
            SendGiftRequestEmailAsync(productKey, recipientEmail).Wait();
        public Task SendGiftRequestEmailAsync(string productKey, string recipientEmail)
        {
            return Task.Run(() => { });
        }
    }
}
