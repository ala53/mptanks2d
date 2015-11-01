using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSB.Infrastructure.Apis.Login.Models;

namespace ZSB.Infrastructure.Apis.Login.Controllers
{
    [Route("/")]
    public class ErrorController : Controller
    {
        [Route("error")]
        public ErrorModel Error()
        {
            return ErrorModel.Of("unknown_error");
        }
    }
}
