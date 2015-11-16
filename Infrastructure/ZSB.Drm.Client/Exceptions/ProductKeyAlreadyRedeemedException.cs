using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZSB.Drm.Client.Exceptions
{
    public class ProductKeyAlreadyRedeemedException : Exception
    {
        public ProductKeyAlreadyRedeemedException()
            : base(LocalizationHandler.Localize("product_key_already_redeemed"))
        { }
    }
}
