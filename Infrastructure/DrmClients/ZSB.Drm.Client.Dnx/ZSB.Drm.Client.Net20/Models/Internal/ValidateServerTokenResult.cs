using System;
using System.Collections.Generic;
using System.Text;

namespace ZSB.Drm.Client.Models
{
    public class ValidateServerTokenResult
    {
        public bool IsValid { get; internal set; }
        public PublicUserInfo User { get; internal set; }
    }
}
