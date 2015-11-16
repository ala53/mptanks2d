using System;
using System.Collections.Generic;
using System.Text;
using ZSB.Drm.Client.Models;

namespace ZSB.Drm.Client
{
    class PersistentStorageData
    {
        private FullUserInfo _cachedInfoBacking;
        public FullUserInfo CachedInfo
        {
            get { return _cachedInfoBacking; }
            set { _cachedInfoBacking = value; DrmClient.RaiseStorageChanged(); }
        }

        private string _sessionKeyBacking;
        public string SessionKey
        {
            get { return _sessionKeyBacking; }
            set { _sessionKeyBacking = value; DrmClient.RaiseStorageChanged(); }
        }
    }
}
