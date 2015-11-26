using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ZSB.Drm.Client
{
    class Rest
    {
        public class Model<T>
        {
            public string Type { get; set; }
            public bool Error => Type == "error";
            public string Message { get; set; }
            public T Data { get; set; }
        }
        public static string UrlBase => "https://login.zsbgames.me/";
        public static Model<object> DoPost(string address, object data) => DoPost<object>(address, data);
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public static Model<T> DoPost<T>(string address, object data)
        {
            //    var client = new RestClient(UrlBase);
            //    var request = new RestRequest(address, Method.POST);
            //    request.RequestFormat = DataFormat.Json;
            //    request.AddBody(data);

            //    var response = client.Execute(request);
            //    var resp = JsonConvert.DeserializeObject<Model<T>>(response.Content);
            //    if (resp.Message == "invalid_request" || resp.Message == "unknown_error")
            //        throw new Exceptions.AccountServerException();

            //    return resp;
            try
            {
                address = UrlBase + address;
                var req = WebRequest.Create(address);
                req.Method = "POST";
                req.ContentType = "application/json";

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
                req.ContentLength = body.Length;

                using (var stream = req.GetRequestStream())
                    stream.Write(body, 0, body.Length);

                HttpWebResponse response = null;
                var rtemp = req.GetResponse();
                response = rtemp as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.RequestTimeout)
                {
                    DrmClient.Offline = true;
                    throw new Exceptions.UnableToAccessAccountServerException();
                }

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exceptions.AccountServerException();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                DrmClient.Offline = false;

                var resp = JsonConvert.DeserializeObject<Model<T>>(responseString);
                if (resp.Message == "invalid_request" || resp.Message == "unknown_error")
                    throw new Exceptions.AccountServerException();

                return resp;
            }
            catch (Exception ex)
            { throw new Exceptions.AccountServerException(ex); }
        }

        public static Model<object> DoGet(string address) => DoGet<object>(address);
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public static Model<T> DoGet<T>(string address)
        {
            address = UrlBase + address;
            try
            {
                var req = WebRequest.Create(address);
                req.Method = "GET";

                var response = (HttpWebResponse)req.GetResponse();

                if (response.StatusCode == HttpStatusCode.RequestTimeout)
                {
                    DrmClient.EnsureInitialized();
                    DrmClient.Offline = true;
                    throw new Exceptions.UnableToAccessAccountServerException();
                }
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exceptions.AccountServerException();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                DrmClient.EnsureInitialized();
                DrmClient.Offline = false;

                var resp = JsonConvert.DeserializeObject<Model<T>>(responseString);
                if (resp.Message == "invalid_request" || resp.Message == "unknown_error")
                    throw new Exceptions.AccountServerException();

                return resp;
            }
            catch (Exception ex)
            { throw new Exceptions.AccountServerException(ex); }
        }
    }
}
