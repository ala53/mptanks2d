using System;
using System.Collections.Generic;
using System.Text;
using ZSB.Drm.Client.Models;

namespace ZSB.Drm.Client
{
    class PersistentStorageData
    {
        public FullUserInfo CachedInfo { get; set; }
        public string SessionKey { get; set; }
    }
}
