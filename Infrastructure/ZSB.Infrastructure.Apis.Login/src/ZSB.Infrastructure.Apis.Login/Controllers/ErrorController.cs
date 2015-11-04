using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSB.Infrastructure.Apis.Login.Models;

namespace ZSB.Infrastructure.Apis.Login.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error")]
        public ErrorModel Error()
        {
            return ErrorModel.Of("unknown_error");
        }
    }
}
