using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSB.Infrastructure.Apis.Login.Models;

namespace ZSB.Infrastructure.Apis.Login.Controllers
{
    [Route("/account")]
    public class ForgotPasswordController : Controller
    {
        [HttpPost, Route("password/forgot/request")]
        public ResponseModelBase SendForgotPasswordMessage([FromBody]ForgotPasswordRequestModel model)
        {

        }

        [HttpPost, Route("password/forgot/change")]
        public ResponseModelBase ChangeForgottenPassword([FromBody]ForgotPasswordDoChangeModel model)
        {

        }
    }
}
