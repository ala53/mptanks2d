using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Web.Home.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet, Route("/Login")]
        public IActionResult Login(string to = null)
        {
            ViewBag.Error = false;
            ViewBag.EmailAddress = "";
            return View("Login");
        }
        [HttpPost, Route("/Login")]
        public async Task<IActionResult> DoLogin(string EmailAddress, string Password, string to = null)
        {
            var res = await DoLoginAction(EmailAddress, Password);
            if (res != null) return res;
            ViewBag.RedirectTo = to ?? "/";
            return View("~/Views/RedirectPage");
        }

        private async Task<IActionResult> DoLoginAction(string EmailAddress, string Password)
        {
            ViewBag.Error = false;
            if (EmailAddress == null || EmailAddress == "" || Password == "" || Password == null)
            {
                ViewBag.EmailAddress = EmailAddress;
                ViewBag.Error = true;
                ViewBag.Message = this.Localize("empty_field");
                return View("Login");
            }

            //do rest request
            var response = await Rest.RestHelper.DoPostDynamic(Startup.LoginServerAddress + "login", new
            {
                EmailAddress = EmailAddress,
                Password = Password
            });

            if (response.Error)
            {
                ViewBag.Error = true;
                ViewBag.EmailAddress = EmailAddress;
                ViewBag.Message = this.Localize(response.Message);
                return View("Login");
            }

            Response.Cookies.Append("__ZSB_login_sessionKey__", response.Data.sessionKey.ToString(),
                new Microsoft.AspNet.Http.CookieOptions
                {
                    HttpOnly = false,
                    Expires = DateTime.UtcNow.AddDays(14)
                });

            return null;
        }

        [HttpGet, Route("/Logout")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("__ZSB_login_sessionKey__");
            var response = await Rest.RestHelper.DoPostDynamic(Startup.LoginServerAddress + "logout",
                 new { SessionKey = Request.Cookies["__ZSB_login_sessionKey__"].FirstOrDefault() });

            LoginChecker.MarkInvalid(Request.Cookies["__ZSB_login_sessionKey__"].FirstOrDefault());
            ViewBag.IsLoggingOut = true;
            ViewBag.RedirectTo = "/";
            return View("~/Views/RedirectPage");
        }
    }
}
