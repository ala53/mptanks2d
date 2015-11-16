using System;
using System.Collections.Generic;
using System.Text;

namespace ZSB.Drm.Client.Models
{
    public class PublicUserInfo
    {
        public bool Owns(string productId) => Owns(new Guid(productId));
        public bool Owns(Guid productId)
        {
            return false;
        }
    }
}
