using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Web.Home.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet, Route("/Login")]
        public IActionResult Login()
        {
            return View("Login");
        }
        [HttpPost, Route("/Login")]
        public IActionResult DoLogin()
        {
            return View("Login");
        }
        [HttpGet, Route("/Logout")]
        public IActionResult Logout()
        {
            return View("Logout");
        }
    }
}
