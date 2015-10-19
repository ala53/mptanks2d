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
        public IActionResult RequestCreateAccount()
        {
            return View("Create/Request");
        }

        [HttpPost, Route("Created")]
        public IActionResult ConfirmCreateAccount()
        {
            return View("Create/Confirm");
        }
    }
}
