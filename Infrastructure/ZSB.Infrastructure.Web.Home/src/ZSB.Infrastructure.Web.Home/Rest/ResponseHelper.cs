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
        private static Dictionary<string, JObject> _langs = new Dictionary<string, JObject>();
        public static string Get(string msg, string lang = "en")
        {
            if (!_langs.ContainsKey(lang))
                Load(lang, lang);

            JToken output;
            if (_langs[lang].TryGetValue(msg, out output))
                return output.ToString();

            return _langs[lang]["string_not_found"].ToString();
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
