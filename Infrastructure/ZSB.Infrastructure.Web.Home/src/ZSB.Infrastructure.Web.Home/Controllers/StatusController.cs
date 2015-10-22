using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Web.Home.Controllers
{
    [Route("/Status")]
    public class StatusController : Controller
    {
        [Route("404")]
        public IActionResult Show404Page()
        {
            return View("404");
        }
    }
}
