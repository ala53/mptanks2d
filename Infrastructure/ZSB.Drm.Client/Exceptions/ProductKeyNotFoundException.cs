using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZSB.Drm.Client.Exceptions
{
    public class ProductKeyNotFoundException : Exception
    {
        public ProductKeyNotFoundException()
            : base(LocalizationHandler.Localize("product_key_not_found"))
        { }
    }
}
