using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Framework.Configuration;
using Newtonsoft.Json.Linq;

namespace ZSB.Infrastructure.Apis.Login.Backend
{
    public static class EmailSender
    {
        public static async Task SendRegistrationEmail(Models.UserModel model)
        {
            var apiKey = Startup.Configuration["Data:SendGridAPIKey"];
            await Task.Delay(1000); //Simulate the network delay on the request to sendgrid
            var request = new HttpRequestMessage(HttpMethod.Post,
                "https://api.sendgrid.com/api/mail.send.json");

            request.Headers.Add("Authorization", "Bearer " + apiKey);
            //Set the parameters
            var param = new Dictionary<string, string>();
            param["to"] = model.EmailAddress;
            param["toname"] = model.Username;
            param["subject"] = " ";
            param["from"] = "noreply@zsbgames.me";
            param["fromname"] = "ZSB Games";
            param["replyto"] = "support@zsbgames.me";
            param["html"] = " ";

            //Create X-smtp api header
            var config = new JObject();
            config.Add("filters", new JObject());
            config["filters"]["templates"] = new JObject();
            config["filters"]["templates"]["settings"] = new JObject();
            config["filters"]["templates"]["settings"]["enable"] = 1;
            config["filters"]["templates"]["settings"]["template_id"] = "813dc962-f5e9-4f21-ab88-fd2f236b76ba";

            config["to"] = new JArray(model.EmailAddress);

            config["sub"] = new JObject();
            config["sub"]["-username-"] = new JArray(model.Username);
            config["sub"]["-userId-"] = new JArray(model.UniqueId.ToString());
            config["sub"]["-emailConfirmCode-"] = new JArray(model.EmailConfirmCode.ToString());
            param["x-smtpapi"] = config.ToString();
            //And set request body
            request.Content = new FormUrlEncodedContent(param);
            //And send it
            using (var response = await new HttpClient().SendAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                    throw new Exception("email_send_failed");

            }
        }
        public static async Task SendDeletionEmail(Models.UserModel model)
        {
            var apiKey = Startup.Configuration["Data:SendGridAPIKey"];
            await Task.Delay(1000); //Simulate the network delay on the request to sendgrid
            var request = new HttpRequestMessage(HttpMethod.Post,
                "https://api.sendgrid.com/api/mail.send.json");

            request.Headers.Add("Authorization", "Bearer " + apiKey);
            //Set the parameters
            var param = new Dictionary<string, string>();
            param["to"] = model.EmailAddress;
            param["toname"] = model.Username;
            param["subject"] = " ";
            param["from"] = "noreply@zsbgames.me";
            param["fromname"] = "ZSB Games";
            param["replyto"] = "support@zsbgames.me";
            param["html"] = " ";

            //Create X-smtp api header
            var config = new JObject();
            config.Add("filters", new JObject());
            config["filters"]["templates"] = new JObject();
            config["filters"]["templates"]["settings"] = new JObject();
            config["filters"]["templates"]["settings"]["enable"] = 1;
            config["filters"]["templates"]["settings"]["template_id"] = "15938f38-cfbb-4b88-b403-2e5cf1721263";

            config["to"] = new JArray(model.EmailAddress);

            config["sub"] = new JObject();
            config["sub"]["-username-"] = new JArray(model.Username);
            config["sub"]["-userId-"] = new JArray(model.UniqueId.ToString());
            config["sub"]["-emailConfirmCode-"] = new JArray(model.EmailConfirmCode.ToString());
            param["x-smtpapi"] = config.ToString();
            //And set request body
            request.Content = new FormUrlEncodedContent(param);
            //And send it
            using (var response = await new HttpClient().SendAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                    throw new Exception("email_send_failed");

            }
        }
    }
}
