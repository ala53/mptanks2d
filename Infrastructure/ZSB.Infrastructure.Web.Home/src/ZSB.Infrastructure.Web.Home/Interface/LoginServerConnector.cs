using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Web.Home.NewFolder
{
    public static class LoginServerConnector
    {
        private struct Model<T>
        {
            public string Type { get; set; }
            public T Data { get; set; }
        }
        public static async Task<UserInfoResponseModel> ValidateLogin(string sessionKey)
        {
            // queries /validate/key/info
            var request = new HttpRequestMessage(HttpMethod.Post, Startup.Configuration["Data:LoginServerAddress"]);
            request.Content = new StringContent(
                JsonConvert.SerializeObject(new { X = "" }), 
                System.Text.Encoding.UTF8, "application/json");

            using (var resp = await new HttpClient().SendAsync(request))
            {
                if (!resp.IsSuccessStatusCode)
                    return null; //Not logged in / server error

                var asObject = 
                    JsonConvert.DeserializeObject<Model<UserInfoResponseModel>>(
                    await resp.Content.ReadAsStringAsync());

                if (asObject.Type == "error") return null;
                return asObject.Data;
            }
            
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
