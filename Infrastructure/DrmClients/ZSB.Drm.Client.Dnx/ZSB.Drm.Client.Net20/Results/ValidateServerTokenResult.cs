using System;
using System.Collections.Generic;
using System.Text;

namespace ZSB.Drm.Client.Results
{
    public class ValidateServerTokenResult
    {
        public bool IsTokenValid { get; internal set; }
        public PublicUserInfo UserInfo { get; internal set; }
    }
}
