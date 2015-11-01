using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Web.Home.Controllers
{
    [Route("Login/Account")]
    public class AccountController : Controller
    {
        [HttpGet, Route("Create")]
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

            return View("~/Views/Account/Create/Request");
        }

        [HttpPost, Route("Create")]
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
                ViewBag.Message = Rest.ResponseHelper.Get("unknown_error");
            else ViewBag.Message = Rest.ResponseHelper.Get(postback.Message);

            if (ViewBag.Error)
            {
                //It failed, start over and redo the challenge page
                ViewBag.EmailAddress = EmailAddress;
                ViewBag.Username = Username;
                ViewBag.ChallengeId = ChallengeId;
                ViewBag.ChallengeQuestion = ChallengeQuestion;
                ViewBag.Password = Password;

                return View("~/Views/Account/Create/Request");
            }
            else
            {
                //It completed, display confirmation screen
                return View("~/Views/Account/Create/Confirm");
            }
        }

    }
}
