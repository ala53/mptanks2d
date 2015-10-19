using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpPost, Route("EmailSent")]
        public IActionResult ConfirmSendEmail()
        {
            return View("SendEmail/Confirm");
        }

        [HttpGet, Route("ChangePassword/{accountId}/{emailConfirmCode}")]
        public IActionResult ChangePassword(Guid accountId, Guid emailConfirmCode)
        {
            return View("Change/Change");
        }

        [HttpPost, Route("PasswordChanged")]
        public IActionResult ConfirmPasswordChanged()
        {
            return View("Change/Confirm");
        }
    }
}
