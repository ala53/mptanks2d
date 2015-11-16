using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZSB.Drm.Client.Models.Response
{
    class LoginResponse
    {
        public string SessionKey { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
