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
        [HttpGet, Route("/Product/Redeem")]
        public IActionResult ShowProductRedeemPage()
        {
            if (this.NotLoggedIn()) return this.SendToLogin();
            ViewBag.Error = false;
            return View("Request");
        }
        [HttpPost, Route("/Product/Redeem")]
        public async Task<IActionResult> DoRedeem(string ProductKey)
        {
            if (this.NotLoggedIn()) return this.SendToLogin();

            var response = await Rest.RestHelper.DoPostDynamic(Startup.LoginServerAddress + "product/redeem",
                new
                {
                    SessionKey = this.SessionKey(),
                    ProductKey = ProductKey
                });

            ViewBag.Error = response.Error;
            ViewBag.Message = this.Localize(response.Message);

            if (response.Error)
                return View("Request");
            else return View("Confirm");
        }

        [HttpGet, Route("/Product/Gift/{productKey}/{addressToSendTo}")]
        public async Task<IActionResult> RequestGiftProduct(string productKey, string addressToSendTo)
        {
            if (this.NotLoggedIn()) return this.SendToLogin();

            var request = await Rest.RestHelper.DoPostDynamic(Startup.LoginServerAddress + "product/gift/request",
                new
                {
                    ProductKeyToGift = productKey,
                    EmailAddressToGiftTo = addressToSendTo,
                    SessionKey = this.SessionKey()
                }
                );

            ViewBag.Error = request.Error;
            ViewBag.Message = this.Localize(request.Message);

            return View("Gift/Request");
        }

        [HttpGet("/Product/Gift/Confirm/{userId}/{emailConfirmCode}/{productKey}/{addressToSendTo}")]
        public async Task<IActionResult> ConfirmGiftProduct(Guid userId, Guid emailConfirmCode, string productKey, string addressToSendTo)
        {
            var response = await Rest.RestHelper.DoGetDynamic(Startup.LoginServerAddress +
                "product/gift/confirm/" + userId + "/" + emailConfirmCode + "/" + productKey + "/" + addressToSendTo);

            ViewBag.Error = response.Error;
            ViewBag.Message = this.Localize(response.Message);

            return View("Gift/EmailConfirm");
        }
    }
}
