using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Web.Home.Rest
{
    public static class ResponseHelper
    {
        public static string UnknownErrorMessage(string lang = "en") => Get(null, lang);
        public static string UnknownErrorMessage(HttpRequest request) => Get(null, GetLanguage(request));
        private static Dictionary<string, JObject> _langs = new Dictionary<string, JObject>();
        public static string Get(string msg, HttpRequest request) => Get(msg, GetLanguage(request));
        public static string Get(string msg, string lang = "en")
        {
            if (msg == null) msg = "unknown_error";
            if (!_langs.ContainsKey(lang))
                Load(lang, lang);

            JToken output;
            if (_langs[lang].TryGetValue(msg, out output))
                return output.ToString();

            return _langs[lang]["string_not_found"].ToString();
        }

        public static string GetLanguage(HttpRequest request)
        {
            return "en"; //todo
        }

        private static void Load(string loadLang, string mappedLang)
        {
            if (File.Exists(Path.Combine(Startup.Environment.ApplicationBasePath, $"responses/responses.{loadLang}.json")))
            {
                var asObj = JObject.Parse(File.ReadAllText(
                    Path.Combine(Startup.Environment.ApplicationBasePath, $"responses/responses.{loadLang}.json")));

                _langs.Add(mappedLang, asObj);
            }
            else
            {
                if (loadLang == "en") throw new Exception("en language is missing!");
                else Load("en", mappedLang);
            }
        }
    }
}

namespace ZSB.Infrastructure.Web.Home
{
    public static class __ControllerExtensionForResponseHelper
    {


        //controller extension
        public static string Localize(this Controller controller, string message)
        {
            return Rest.ResponseHelper.Get(message, controller.Request);
        }
    }
}
