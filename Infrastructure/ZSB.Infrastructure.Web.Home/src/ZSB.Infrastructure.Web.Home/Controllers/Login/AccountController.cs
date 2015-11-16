using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSB.Infrastructure.Web.Home.Rest;

namespace ZSB.Infrastructure.Web.Home.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet, Route("/Register")]
        public async Task<IActionResult> RequestCreateAccount()
        {
            //Get the challenge for them
            var challenge = await Rest.RestHelper.DoGetDynamic(
                Startup.LoginServerAddress + "account/challenge/get");

            ViewBag.ChallengeId = challenge.Data.offset;
            ViewBag.ChallengeQuestion = challenge.Data.question;

            ViewBag.EmailAddress = "";
            ViewBag.Username = "";
            ViewBag.Password = "";
            ViewBag.Error = false;

            return View("Create/Request");
        }

        [HttpPost, Route("/Register")]
        public async Task<IActionResult> ConfirmCreateAccount(
            string EmailAddress, string Username, int ChallengeId,
            string ChallengeQuestion, string ChallengeAnswer,
            string Password, string ConfirmPassword)
        {
            //Post and check
            Rest.RestHelper.Model<dynamic> postback = null;
            if (Password != ConfirmPassword)
                postback = new Rest.RestHelper.Model<dynamic>() { Type = "error", Message = "password_do_not_match" };
            else
                postback = await Rest.RestHelper.DoPostDynamic(Startup.LoginServerAddress + "account/register",
                new
                {
                    EmailAddress = EmailAddress,
                    Username = Username,
                    ChallengeId = ChallengeId,
                    ChallengeAnswer = ChallengeAnswer,
                    Password = Password
                });

            ViewBag.Error = postback == null || postback.Error;

            //And show the error message
            if (postback == null)
                ViewBag.Message = Rest.ResponseHelper.UnknownErrorMessage(Request);
            else ViewBag.Message = this.Localize(postback.Message);

            if (ViewBag.Error)
            {
                //It failed, start over and redo the challenge page
                ViewBag.EmailAddress = EmailAddress;
                ViewBag.Username = Username;
                ViewBag.ChallengeId = ChallengeId;
                ViewBag.ChallengeQuestion = ChallengeQuestion;
                ViewBag.Password = Password;
                ViewBag.ConfirmPassword = ConfirmPassword;

                return View("Create/Request");
            }
            else
            {
                //It completed, display confirmation screen
                return View("Create/Confirm");
            }
        }

        [HttpGet, Route("/Account")]
        public IActionResult AccountPage()
        {
            if (this.NotLoggedIn()) return this.SendToLogin();

            var loginData = this.UserData();
            ViewBag.Data = loginData;
            ViewBag.Username = loginData.Username;
            ViewBag.Error = false;
            ViewBag.EmailAddress = loginData.EmailAddress;

            return View("Account");
        }

        [HttpPost, Route("/Account")]
        public async Task<IActionResult> SaveSettings(string NewEmailAddress, string CurrentPassword, string NewPassword, string ConfirmNewPassword, string NewUsername)
        {
            if (this.NotLoggedIn()) return this.SendToLogin();

            var loginData = this.UserData();
            ViewBag.Error = false;
            ViewBag.Data = loginData;
            ViewBag.Username = loginData.Username;
            ViewBag.EmailAddress = loginData.EmailAddress;
            
            if (loginData.Username != NewUsername)
            {
                //Change Username
                var response = await Rest.RestHelper.DoPostDynamic(Startup.LoginServerAddress + "account/username/change",
                    new
                    {
                        EmailAddress = loginData.EmailAddress,
                        Password = CurrentPassword,
                        NewUsername = NewUsername
                    });

                if (response.Error)
                {
                    ViewBag.Error = true;
                    ViewBag.Message = this.Localize(response.Message);
                    return View("Account");
                }

                ViewBag.Username = NewUsername;
            }

            if (loginData.EmailAddress != NewEmailAddress)
            {
                //Change Email
                var response = await Rest.RestHelper.DoPostDynamic(Startup.LoginServerAddress + "account/password/change",
                    new
                    {
                        EmailAddress = loginData.EmailAddress,
                        Password = CurrentPassword,
                        NewEmailAddress = NewEmailAddress
                    });

                if (response.Error)
                {
                    ViewBag.Error = true;
                    ViewBag.Message = this.Localize(response.Message);
                    return View("Account");
                }

                ViewBag.EmailAddress = NewEmailAddress;
            }

            if (!string.IsNullOrWhiteSpace(NewPassword))
            {
                if (NewPassword != ConfirmNewPassword)
                {
                    ViewBag.Error = true;
                    ViewBag.Message = this.Localize("passwords_do_not_match");
                    return View("Account");
                }
                //Change Password
                var response = await Rest.RestHelper.DoPostDynamic(Startup.LoginServerAddress + "account/password/change",
                    new
                    {
                        EmailAddress = NewEmailAddress,
                        OldPassword = CurrentPassword,
                        NewPassword = NewPassword
                    });

                if (response.Error)
                {
                    ViewBag.Error = true;
                    ViewBag.Message = this.Localize(response.Message);
                    return View("Account");
                }
            }


            //Log them out
            ViewBag.RedirectTo = "/Logout";
            ViewBag.Time = 5;
            ViewBag.Message = "Your settings have been updated. Logging you out.";
            return View("~/Views/RedirectPage");
        }

        [HttpGet, Route("/Account/Owned")]
        public IActionResult OwnedProducts()
        {
            if (this.NotLoggedIn()) return this.SendToLogin();

            var loginData = this.UserData();
            ViewBag.Data = loginData;
            ViewBag.Products = loginData.OwnedProducts;

            return View("Owned");
        }
    }
}
