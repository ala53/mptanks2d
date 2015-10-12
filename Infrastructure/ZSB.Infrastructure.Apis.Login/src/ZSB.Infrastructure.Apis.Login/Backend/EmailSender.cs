using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.Configuration;

namespace ZSB.Infrastructure.Apis.Login.Backend
{
    public static class EmailSender
    {
        public static async Task SendRegistrationEmail(Models.UserModel model)
        {
            var apiKey = Startup.Configuration["Data:SendGridAPIKey"];
            await Task.Delay(1000); //Simulate the network delay on the request to sendgrid
        }
    }
}
