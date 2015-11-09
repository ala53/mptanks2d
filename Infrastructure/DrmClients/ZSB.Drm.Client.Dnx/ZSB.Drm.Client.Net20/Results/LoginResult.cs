using System;
using System.Collections.Generic;
using System.Text;

namespace ZSB.Drm.Client.Results
{
    public class LoginResult
    {
        public bool LoginSuccess { get; set; }
        public string Message { get; set; }
        public FullUserInfo FullUserInfo { get; set; }
    }
}
