using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Framework.Configuration;
using Newtonsoft.Json.Linq;

namespace ZSB.Infrastructure.Apis.Account.Backend
{
    public static class EmailSender
    {
        public static readonly string RegistrationTemplate = "813dc962-f5e9-4f21-ab88-fd2f236b76ba";
        public static readonly string DeletionTemplate = "15938f38-cfbb-4b88-b403-2e5cf1721263";
        public static readonly string ForgotPasswordTemplate = "b8cbeea4-9f50-409f-80de-cb9717e45134";
        public static readonly string ProductKeyRedeemedTemplate = "7e6f8785-9a0b-42f8-beb2-cc1b1e59132f";
        public static readonly string ProductKeyGiftRequestedTemplate = "850359d7-5ea3-4514-9f18-58dc88210147";
        public static readonly string ProductKeyGiftedTemplate = "5a7c4efe-7185-44e6-bf90-634464282ca9";
        public static async Task SendEmail(Models.UserModel model, string template, Dictionary<string, string> substitutions = null)
        {
            if (substitutions == null) substitutions = new Dictionary<string, string>();

            var apiKey = Startup.Configuration["Data:SendGridAPIKey"];
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
            config["filters"]["templates"]["settings"]["template_id"] = template;

            config["to"] = new JArray(model.EmailAddress);

            config["sub"] = new JObject();
            config["sub"]["-username-"] = new JArray(model.Username);
            config["sub"]["-userId-"] = new JArray(model.UniqueId.ToString());
            config["sub"]["-emailConfirmCode-"] = new JArray(model.UniqueConfirmationCode.ToString());

            foreach (var sub in substitutions)
                config["sub"][sub.Key] = new JArray(sub.Value);
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
