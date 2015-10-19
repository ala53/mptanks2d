using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Web.Home.Rest
{
    public static class LoginServerConnector
    {
        public static async Task<UserInfoResponseModel> ValidateLogin(string sessionKey)
        {
            // queries /validate/key/info
            return (await RestHelper.DoPost<UserInfoResponseModel>(
                Startup.Configuration["Data:LoginServerAddress"] + "validate/key/info",
                new { SessionKey = sessionKey })).Data;
        }

        public static async Task<bool> CheckIsLoggedIn(string sessionKey)
        {
            return await ValidateLogin(sessionKey) == null ? false : true;
        }
        public class UserInfoResponseModel
        {
            public string Username { get; set; }
            public Guid UniqueId { get; set; }
            public bool IsPremium { get; set; }
            public DateTime AccountCreated { get; set; }

            public UserInfoResponseModel() { }
        }
    }
}
