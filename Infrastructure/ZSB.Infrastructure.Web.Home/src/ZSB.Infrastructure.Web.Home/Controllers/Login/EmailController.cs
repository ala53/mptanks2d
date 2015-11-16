using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSB.Infrastructure.Web.Home.Rest;

namespace ZSB.Infrastructure.Web.Home.Controllers
{
    public class EmailController : Controller
    {
        [HttpGet, Route("/EmailConfirmation/Resend")]
        public IActionResult Resend()
        {
            return View("Resend/Request");
        }
        [HttpPost, Route("/EmailConfirmation/Resend")]
        public async Task<IActionResult> ConfirmResend(string EmailAddress)
        {
            var response = await Rest.RestHelper.DoPostDynamic(
                Startup.LoginServerAddress + "email/resend", new
                {
                    EmailAddress = EmailAddress
                });

            ViewBag.Error = response == null || response.Error;

            if (response == null)
                ViewBag.Message = this.Localize("unknown_error");
            else ViewBag.Message = this.Localize(response.Message);

            return View("Resend/Confirm");
        }

        [HttpGet, Route("/EmailConfirmation/Confirm/{accountId}/{emailConfirmCode}")]
        public async Task<IActionResult> Confirm(Guid accountId, Guid emailConfirmCode)
        {
            var response = await Rest.RestHelper.DoGetDynamic(
                Startup.LoginServerAddress + $"email/confirm/{accountId}/{emailConfirmCode}");

            ViewBag.Error = response == null || response.Error;
            if (response != null)
                ViewBag.ErrorMessage = this.Localize(response.Message);

            return View("Confirm");
        }

        [HttpGet, Route("/EmailConfirmation/Disavow/{accountId}/{emailConfirmCode}")]
        public IActionResult DisavowPage(Guid accountId, Guid emailConfirmCode)
        {
            return View("DisavowCheck");
        }

        [HttpPost, Route("/EmailConfirmation/Disavow/{accountId}/{emailConfirmCode}")]
        public async Task<IActionResult> Disavow(Guid accountId, Guid emailConfirmCode)
        {
            var response = await Rest.RestHelper.DoGetDynamic(
                Startup.LoginServerAddress + $"email/disavow/{accountId}/{emailConfirmCode}");

            ViewBag.Error = response == null || response.Error;
            if (response != null)
                ViewBag.ErrorMessage = this.Localize(response.Message);

            return View("Disavow");
        }
    }
}
