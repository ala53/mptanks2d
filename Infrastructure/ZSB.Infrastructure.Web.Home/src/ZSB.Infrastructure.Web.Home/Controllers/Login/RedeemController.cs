using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http.Extensions;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ZSB.Infrastructure.Web.Home.Controllers.Login
{
    public class RedeemController : Controller
    {
        [HttpGet, Route("/Redeem")]
        public IActionResult ShowProductRedeemPage()
        {
            if (this.NotLoggedIn()) return this.SendToLogin();

            return View("Request");
        }
        [HttpPost, Route("/Redeem")]
        public async Task<IActionResult> DoRedeem(string ProductKey)
        {
            if (this.NotLoggedIn()) return this.SendToLogin();

            var response = Rest.RestHelper.DoPostDynamic(Startup.LoginServerAddress + "product/redeem",
                new
                {

                });

            return View("Confirm");
        }
    }
}
