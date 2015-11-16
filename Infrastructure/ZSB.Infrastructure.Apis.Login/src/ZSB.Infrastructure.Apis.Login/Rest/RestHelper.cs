using Microsoft.AspNet.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Account.Rest
{
    public static class RestHelper
    {
        public class Model<T>
        {
            public string Type { get; set; }
            public bool Error => Type == "error";
            public string Message { get; set; }
            public T Data { get; set; }
        }
        #region Post
        public static async Task<Model<T>> DoPost<T>(string address, object postData) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Post, address);
            request.Content = new StringContent(
                JsonConvert.SerializeObject(postData),
                System.Text.Encoding.UTF8, "application/json");

            using (var resp = await new HttpClient().SendAsync(request))
            {
                if (!resp.IsSuccessStatusCode)
                    return new Model<T>() { Message = "unknown_error", Type = "error" }; //Not logged in / server error

                var asObject =
                    JsonConvert.DeserializeObject<Model<T>>(
                    await resp.Content.ReadAsStringAsync());

                return asObject;
            }

        }
        public static async Task<Model<dynamic>> DoPostDynamic(string address, object postData)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, address);
            request.Content = new StringContent(
                JsonConvert.SerializeObject(postData),
                System.Text.Encoding.UTF8, "application/json");

            using (var resp = await new HttpClient().SendAsync(request))
            {
                if (!resp.IsSuccessStatusCode)
                    return new Model<dynamic>() { Message = "unknown_error", Type = "error" }; //Not logged in / server error

                var asObject =
                    JsonConvert.DeserializeObject<Model<dynamic>>(
                    await resp.Content.ReadAsStringAsync());

                return asObject;
            }
        }
        #endregion
        #region Get
        public static async Task<Model<T>> DoGet<T>(string address) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Get, address);

            using (var resp = await new HttpClient().SendAsync(request))
            {
                if (!resp.IsSuccessStatusCode)
                    return new Model<T>() { Message = "unknown_error", Type = "error" }; //Not logged in / server error

                var asObject =
                    JsonConvert.DeserializeObject<Model<T>>(
                    await resp.Content.ReadAsStringAsync());

                return asObject;
            }

        }
        public static async Task<Model<dynamic>> DoGetDynamic(string address)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, address);

            using (var resp = await new HttpClient().SendAsync(request))
            {
                if (!resp.IsSuccessStatusCode)
                    return new Model<dynamic>() { Message = "unknown_error", Type = "error" }; //Not logged in / server error

                var asObject =
                    JsonConvert.DeserializeObject<Model<dynamic>>(
                    await resp.Content.ReadAsStringAsync());

                return asObject;
            }

        }
        #endregion
    }
}
