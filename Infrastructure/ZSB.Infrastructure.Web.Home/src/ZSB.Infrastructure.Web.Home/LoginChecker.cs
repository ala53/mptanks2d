using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Extensions;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Web.Home
{
    public static class LoginChecker
    {
        public class LoginResponseInfo
        {
            public bool LoggedIn { get; set; }
            public bool Error { get; set; }
            public UserInfoResponse Data { get; set; }

        }

        public class UserInfoResponse
        {
            public string Username { get; set; }
            public Guid UniqueId { get; set; }
            public bool IsPremium { get; set; }
            public DateTime AccountCreated { get; set; }
        }

        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions { CompactOnMemoryPressure = true });

        public static bool IsLoggedIn(HttpRequest request) => CheckLogin(request).Result != null;
        public static bool IsLoggedIn(string sessionKey) => CheckLogin(sessionKey).Result != null;
        public static async Task<bool> IsLoggedInAsync(HttpRequest request) => await CheckLogin(request) != null;
        public static async Task<bool> IsLoggedInAsync(string sessionKey) => await CheckLogin(sessionKey) != null;

        public static async Task<LoginResponseInfo> CheckLogin(HttpRequest request)
        {
            if (!request.Cookies.ContainsKey("__ZSB_login_sessionkey__"))
                return null;

            return await CheckLogin(request.Cookies["__ZSB_login_sessionKey__"].First());
        }

        public static async Task<LoginResponseInfo> CheckLogin(string sessionKey)
        {
            //First checks the cache, then checks the server
            object responseInfo;
            if (_cache.TryGetValue(sessionKey, out responseInfo))
                return (LoginResponseInfo)responseInfo;

            //Do check with the server
            var resp = await Rest.RestHelper.DoPost<UserInfoResponse>(
                Startup.LoginServerAddress + "key/validate/info",
                new { SessionKey = sessionKey });

            if (resp == null || resp.Error)
                //DO NOT CACHE, but return not logged in
                return new LoginResponseInfo { LoggedIn = false, Error = true };

            LoginResponseInfo queried = new LoginResponseInfo
            {
                LoggedIn = resp.Data != null, //if not null, then logged in
                Error = false,
                Data = resp.Data
            };

            //add to cache
            _cache.Set(sessionKey, queried,
                new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(10),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(45)
                });

            //And return
            return queried;
        }

        /// <summary>
        /// Marks an existing login as invalid before the expiration (e.g. because the user logged out)
        /// </summary>
        /// <param name="sessionKey"></param>
        public static void MarkInvalid(string sessionKey)
        {
            _cache.Remove(sessionKey);
        }

        //Extensions

        public static bool NotLoggedIn(this Controller controller)
        {
            return !IsLoggedIn(controller.Request);
        }

        public static IActionResult SendToLogin(this Controller controller)
        {
            return controller.RedirectToAction("/Login/" + controller.Request.GetEncodedUrl());
        }
    }
}
