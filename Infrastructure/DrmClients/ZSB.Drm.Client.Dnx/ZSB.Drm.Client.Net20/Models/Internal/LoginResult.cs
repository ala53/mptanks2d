using System;
using System.Collections.Generic;
using System.Text;

namespace ZSB.Drm.Client.Models
{
    public class LoginResult
    {
        public FullUserInfo FullUserInfo { get; set; }
        public DateTime Expires { get; set; }
    }
}
