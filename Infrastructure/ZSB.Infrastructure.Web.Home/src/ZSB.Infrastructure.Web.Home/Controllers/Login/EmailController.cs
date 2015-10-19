using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Web.Home.Controllers
{
    [Route("/Login/Email")]
    public class EmailController : Controller
    {
        [HttpGet, Route("Resend/Request")]
        public IActionResult Resend()
        {

            return View("Resend/Request");
        }
        [HttpPost, Route("Resend/Confirm")]
        public async Task<IActionResult> ConfirmResend([FromBody]Models.EmailResendRequestModel model)
        {
            if (ModelState.IsValid)
            {
                throw new Exception();
            }

            return View("Resend/Confirm");
        }

        [HttpGet, Route("Confirm/{accountId}/{emailConfirmCode}")]
        public async Task<IActionResult> Confirm(Guid accountId, Guid emailConfirmCode)
        {
            var response = await Rest.RestHelper.DoGetDynamic(
                Startup.Configuration["Data:LoginServerAddress"] + $"email/confirm/{accountId}/{emailConfirmCode}");

            ViewBag.Error = response == null || response.Error;
            if (response != null)
                ViewBag.ErrorMessage = Rest.ResponseHelper.Get(response.Message);

            return View("Confirm");
        }
        [HttpGet, Route("Disavow/{accountId}/{emailConfirmCode}")]
        public async Task<IActionResult> Disavow(Guid accountId, Guid emailConfirmCode)
        {
            var response = await Rest.RestHelper.DoGetDynamic(
                Startup.Configuration["Data:LoginServerAddress"] + $"email/disavow/{accountId}/{emailConfirmCode}");

            ViewBag.Error = response == null || response.Error;
            if (response != null)
                ViewBag.ErrorMessage = Rest.ResponseHelper.Get(response.Message);

            return View("Disavow");
        }
    }
}
