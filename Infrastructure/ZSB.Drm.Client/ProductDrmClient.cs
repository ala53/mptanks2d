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
            if (productKey == null) throw new ArgumentNullException(nameof(productKey));
            return Task.Run(() =>
            {
                DrmClient.EnsureLoggedIn();

                var resp = Rest.DoPost("Product/Redeem", new
                {
                    SessionKey = DrmClient.SessionKey,
                    ProductKey = productKey
                });

                if (resp.Message == "product_key_not_found")
                    throw new ProductKeyNotFoundException();

                if (resp.Message == "product_key_already_redeemed")
                    throw new ProductKeyAlreadyRedeemedException();

                return true;
            });
        }

        public void SendGiftRequestEmail(string productKey, string recipientEmail) =>
            SendGiftRequestEmailAsync(productKey, recipientEmail).Wait();
        public Task SendGiftRequestEmailAsync(string productKey, string recipientEmail)
        {
            if (productKey == null) throw new ArgumentNullException(nameof(productKey));
            if (recipientEmail == null) throw new ArgumentNullException(nameof(productKey));
            return Task.Run(() =>
            {
                DrmClient.EnsureLoggedIn(false);
                var resp = Rest.DoPost("Product/Gift/Request", new
                {
                    SessionKey = DrmClient.SessionKey,
                    ProductKeyToGift = productKey,
                    EmailAddressToGiftTo = recipientEmail
                });

                if (resp.Message == "user_not_found")
                    throw new AccountDetailsIncorrectException("user_not_found");

                if (resp.Message == "product_key_not_found")
                    throw new ProductKeyNotFoundException();
            });
        }
    }
}
