using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSB.Infrastructure.Apis.Account.Models;

namespace ZSB.Infrastructure.Apis.Account.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error")]
        public ErrorModel<string> Error()
        {
            return Models.ErrorModel.Of("ERR_HANDLER", "unknown_error");
        }
    }
}
