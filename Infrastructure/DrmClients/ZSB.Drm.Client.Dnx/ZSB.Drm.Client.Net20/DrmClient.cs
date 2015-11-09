using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ZSB.Drm.Client;
using ZSB.Drm.Client.Exceptions;
using ZSB.Drm.Client.Results;

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
        public static FullUserInfo UserInfo
        {
            get
            {
                if (!_initialized) throw new NotInitializedException();
                return StoredData?.CachedInfo;
            }
        }
        public static bool Offline { get; internal set; }
        public static bool LoggedIn => UserInfo != null;
        public static MultiplayerDrmClient Multiplayer { get; private set; } = new MultiplayerDrmClient();
        private static bool _initialized;
        public static void Initialize(string persistentData = null) => InitializeAsync(persistentData).Wait();

        public static Promise<bool> InitializeAsync(string persistentData = null)
        {
            return null;
        }

        public static LoginResult Login(string email, string password) => LoginAsync(email, password).Wait();

        public static Promise<LoginResult> LoginAsync(string email, string password)
        {
            if (!_initialized) throw new NotInitializedException();

            var promise = new Promise<LoginResult>();
            promise.Status = "Not started";
            ThreadPool.QueueUserWorkItem((state) =>
            {
                try
                {
                    promise.Succeeded = true;
                }
                catch (Exception ex)
                {
                    promise.Failed = true;
                    promise.Status = "Errored";
                    promise.State = ex;
                }
            });

            return promise;
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
        public static bool IsLoggedIn(bool allowOffline = true) => IsLoggedInAsync(allowOffline).Wait();

        public static Promise<bool> IsLoggedInAsync(bool allowOffline = true)
        {
            if (!_initialized) throw new NotInitializedException();

        }

        /// <summary>
        /// Sends a "forgot my password" email to the person at the specified email address. Useful if you want
        /// to add a "forgot my password" link to your login page.
        /// </summary>
        /// <param name="address"></param>
        public static void SendForgotPasswordEmail(string address) =>
            SendForgotPasswordEmailAsync(address).Wait();
        
        public static Promise<bool> SendForgotPasswordEmailAsync(string address)
        {

        }
    }
}
