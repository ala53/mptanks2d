using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZSB.Drm.Client;
using ZSB.Drm.Client.Exceptions;
using ZSB.Drm.Client.Models;

namespace ZSB
{
    public static class DrmClient
    {
        /// <summary>
        /// Gets the persistent storage data for the DRM engine. Store this anywhere to pass to initialize
        /// when the game next starts.
        /// </summary>
        public static string PersistentData
        {
            get { return JsonConvert.SerializeObject(StoredData, Formatting.Indented); }
            set
            {
                try
                { StoredData = JsonConvert.DeserializeObject<PersistentStorageData>(value); }
                catch (Exception ex)
                { throw new PersistentStorageInvalidException(ex); }
            }
        }
        internal static PersistentStorageData StoredData { get; set; }
        public static FullUserInfo User
        {
            get
            {
                if (!_initialized) throw new NotInitializedException();
                return StoredData?.CachedInfo;
            }
        }
        public static bool Offline { get; internal set; }
        public static bool LoggedIn => User != null;
        public static MultiplayerDrmClient Multiplayer { get; private set; } = new MultiplayerDrmClient();
        public static AccountDrmClient Account { get; private set; } = new AccountDrmClient();
        public static ProductDrmClient Products { get; private set; } = new ProductDrmClient();
        internal static bool _initialized;
        public static void Initialize(string persistentData = null) => InitializeAsync(persistentData).Wait();

        public static Task InitializeAsync(string persistentData = null)
        {
            return Task.Run(() =>
            {
                _initialized = true;
            });
        }

        public static LoginResult Login(string email, string password) => LoginAsync(email, password).Result;

        public static Task<LoginResult> LoginAsync(string email, string password)
        {
            if (!_initialized) throw new NotInitializedException();

            return Task.Run<LoginResult>(() => { return (LoginResult)null; });
        }

        /// <summary>
        /// Gets whether the user is currently logged in. 
        /// If allowOffline is set to true, this method first tries to contact the 
        /// login server to verify, then, if it is unable to reach the login server,
        /// searches the persistent storage data to authenticate, and if even that
        /// does not work, it returns false.
        /// </summary>
        /// <param name="allowOffline"></param>
        /// <returns></returns>
        public static bool IsLoggedIn(bool allowOffline = true) => IsLoggedInAsync(allowOffline).Result;

        public static Task<bool> IsLoggedInAsync(bool allowOffline = true)
        {
            if (!_initialized) throw new NotInitializedException();

            return Task.Run(() => { return false; });
        }
    }
}
