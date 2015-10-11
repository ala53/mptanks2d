using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.Configuration;

namespace ZSB.Infrastructure.Apis.Login.Backend
{
    public class AccountCreation
    {
        public static async Task SendRegistrationEmail(Models.UserModel model)
        {
            return; //TODO: Get an API key
            var msg = new SendGridMessage();

            msg.From = new System.Net.Mail.MailAddress("register@zsbgames.me", "ZSB's Evil Robotic Overlord");
            msg.To = new[] { new System.Net.Mail.MailAddress(model.EmailAddress, model.Username + " @ ZSB") };
            msg.ReplyTo = new[] { new System.Net.Mail.MailAddress("", "") };
            msg.EnableFooter("Sent with love by ZSB Games",
                "Sent with love by <a href='https://zsbgames.me'>ZSB Games</a>");

            msg.EnableClickTracking();
            msg.EnableOpenTracking();
            

            msg.Subject = "Registration Confirmation for your ZSB Account";
            msg.Html = $@"
    <img src='https://zsbgames.me/logo_small.png' style='display:inline'> 
    <h2 style='display:inline'> &nbsp;&nbsp;ZSB Games // MP Tanks 2D</h2>
    <br/>
    <h2>Hello {model.Username}!</h2>
        <br>
        <p>You just registered for a ZSB Account so you can play MP Tanks.
        But, we need to be sure we can reach you at your email. Just 
        <a href='https://login.zsbgames.me/confirm/{model.UniqueId}/{model.EmailConfirmCode}'>click here</a> 
        and you'll be able to shoot 2d-beta bullets in no time!
        <br>       
        <br>
        If this wasn't you, 
        <a href='https://login.zsbgames.me/disavow/{model.UniqueId}/{model.EmailConfirmCode}'>click here</a> 
        to disavow the account.</p>
        <br>
        <hr/><a href='[unsubscribe]'>Hurt our feelings (unsubscribe)</a>
".Replace("\t", "").Replace("\n", "");

            msg.Text = $@"
    Hello {model.Username}!
        You just registered for a ZSB Account so you can play MP Tanks.
        But, we need to be sure we can reach you at your email. Just click
        the link below and you'll be able to shoot 2d-beta bullets in no time!
        <newline>
        https://login.zsbgames.me/confirm/{model.UniqueId}/{model.EmailConfirmCode}        
        <newline>
        <newline>
        If this wasn't you, click the link below to disavow the account.
        <newline>
        https://login.zsbgames.me/disavow/{model.UniqueId}/{model.EmailConfirmCode}
        <newline>
        <newline>
        Unsubscribe by clicking below
        <newline>
        [unsubscribe]
".Replace("\t", "").Replace("\n", "").Replace("<newline>", "\n");
            
            var transport = new Web(Startup.Configuration["Data:SendGridAPIKey"]);
            await transport.DeliverAsync(msg);
        }
    }
}
