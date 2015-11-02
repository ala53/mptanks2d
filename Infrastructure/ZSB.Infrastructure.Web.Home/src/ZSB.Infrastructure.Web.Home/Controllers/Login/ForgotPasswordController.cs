using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSB.Infrastructure.Web.Home.Models;

namespace ZSB.Infrastructure.Web.Home.Controllers
{
    [Route("/Login/ForgotPassword")]
    public class ForgotPasswordController : Controller
    {
        [HttpGet, Route("Request")]
        public IActionResult RequestSendEmail()
        {
            return View("SendEmail/Request");
        }

        [HttpPost, Route("Request")]
        public async Task<IActionResult> ConfirmSendEmail(string EmailAddress)
        {
            var result = await Rest.RestHelper.DoPostDynamic(
                Startup.Configuration["Data:LoginServerAddress"] + "account/password/forgot/request",
                new
                {
                    EmailAddress = EmailAddress
                });

            //Error handling
            ViewBag.Error = result == null || result.Error;
            if (ViewBag.Error)
            {
                if (result == null)
                    ViewBag.Message = Rest.ResponseHelper.Get("unknown_error");
                else ViewBag.Message = Rest.ResponseHelper.Get(result.Message);
            }
            else ViewBag.Message = Rest.ResponseHelper.Get(result.Message);

            return View("SendEmail/Confirm");
        }

        [HttpGet, Route("Change/{accountId}/{emailConfirmCode}")]
        public async Task<IActionResult> ChangePassword(Guid accountId, Guid emailConfirmCode)
        {
            return View("Change/Change");
        }

        [HttpPost, Route("PasswordChanged")]
        public async Task<IActionResult> ConfirmPasswordChanged()
        {
            return View("Change/Confirm");
        }
    }
}
