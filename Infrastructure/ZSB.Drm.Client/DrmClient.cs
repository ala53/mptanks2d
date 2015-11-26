using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZSB.Drm.Client;
using ZSB.Drm.Client.Exceptions;
using ZSB.Drm.Client.Models;
using ZSB.Drm.Client.Models.Response;

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

        public static event EventHandler<PropertyChangedEventArgs> OnPersistentStorageChanged = delegate { };
        internal static PersistentStorageData StoredData { get; set; } = new PersistentStorageData();
        internal static void RaiseStorageChanged()
        {
            OnPersistentStorageChanged(StoredData, new PropertyChangedEventArgs(nameof(PersistentData)));
        }
        public static FullUserInfo User
        {
            get
            {
                EnsureInitialized();
                return StoredData?.CachedInfo;
            }
        }
        internal static string SessionKey
        {
            get
            {
                EnsureInitialized();
                return StoredData?.SessionKey;
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
                if (persistentData != null) PersistentData = persistentData;
                //Try online login and token refresh -- if logged in
                if (StoredData != null && StoredData.CachedInfo != null && StoredData.SessionKey != null)
                {
                    try
                    {
                        var resp = Rest.DoPost("Key/Refresh", new
                        {
                            SessionKey = StoredData.SessionKey
                        });
                        if (resp.Error)
                        {
                            if (resp.Message == "not_logged_in")
                            {
                                //not logged in, clear cached data
                                StoredData.SessionKey = null;
                                StoredData.CachedInfo = null;
                            }
                        }

                        //And update the user info
                        try { UpdateUserInfo(); }
                        catch (NotLoggedInException) { }
                    }
                    catch (UnableToAccessAccountServerException) { } //we're in offline mode, I guess
                }
                _initialized = true;
            });
        }

        internal static void EnsureLoggedIn(bool allowOffline = true)
        {
            EnsureInitialized();
            if (!IsLoggedIn(allowOffline))
                throw new NotLoggedInException();
        }
        internal static void EnsureInitialized()
        {
            if (!_initialized) throw new NotInitializedException();
        }
        public static LoginResult Login(string email, string password) => LoginAsync(email, password).Result;

        [MethodImpl(MethodImplOptions.NoOptimization)]
        public static Task<LoginResult> LoginAsync(string emailAddress, string password)
        {
            EnsureInitialized();
            if (emailAddress == null) throw new ArgumentNullException(nameof(emailAddress));
            if (password == null) throw new ArgumentNullException(nameof(password));
            return Task.Run(() => LoginAsyncBody(emailAddress, password));
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static LoginResult LoginAsyncBody(string email, string pass)
        {
            Rest.Model<LoginResponse> resp = null;
            //PLEASE, FOR THE LOVE OF GOD, NEVER ENABLE OPTIMIZATION. IT WILL DESTROY THIS CODE
            resp = Rest.DoPost<LoginResponse>("Login", new Dictionary<string, string>()
                    {
                        {"emailAddress", email },
                        {"password", pass }
                    });

            if (resp.Message == "email_not_confirmed")
                throw new AccountEmailNotConfirmedException();

            if (resp.Message == "username_or_password_incorrect")
                throw new AccountDetailsIncorrectException();

            if (!resp.Error)
            {
                StoredData.SessionKey = resp.Data.SessionKey;
                UpdateUserInfo();
            }
            return new LoginResult()
            {
                Expires = resp.Data.ExpiryDate,
                FullUserInfo = User
            };
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
            EnsureInitialized();

            return Task.Run(() =>
            {
                //try the request
                try
                {
                    if (SessionKey == null) return false;
                    var resp = Rest.DoPost<bool>("Key/Validate", new
                    {
                        SessionKey = SessionKey
                    });
                    return resp.Data;
                }
                catch (UnableToAccessAccountServerException)
                {
                    //Check offline
                    if (!allowOffline) return false;
                    if (User != null) return true; //The cached data is correct
                }
                return false;
            });
        }

        public static void UpdateUserInfo() => UpdateUserInfoAsync().Wait();

        public static Task UpdateUserInfoAsync()
        {
            return Task.Run(() =>
            {
                //Redownload the user info from the account server
                var accountData = Rest.DoPost<FullUserInfo>("Key/Validate/Info", new
                {
                    SessionKey = SessionKey
                });

                if (accountData.Data == null) //Welp, something happened that shouldn't have
                    throw new NotLoggedInException();

                StoredData.CachedInfo = accountData.Data;
            });
        }
    }
}
