using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ZSB.Infrastructure.Apis.Login.Database;
using ZSB.Infrastructure.Apis.Login.Models;

namespace ZSB.Infrastructure.Apis.Login.Controllers
{
    public class LoginController : Controller
    {
        private LoginDB ldb;
        const int ServerTokenValidityInSeconds = 60;
        public LoginController(Database.Contexts.LoginDatabaseContext ctx)
        {
            ldb = new LoginDB(ctx);
        }
        [HttpPost, Route("/Login")]
        public async Task<ResponseModelBase> DoLogin([FromBody]LoginInfoRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");

            try
            {
                var resp = await Diagnostic.Log(ldb.DoLogin, model.EmailAddress, model.Password);
                if (resp == null)
                    return ErrorModel.Of("unknown_error");
                return OkModel.Of(new UserSessionResponseModel(resp), "logged_in");
            }
            catch (ArgumentException e)
            {
                return ErrorModel.Of(e.Message);
            }
        }

        [HttpPost, Route("/Logout")]
        public async Task<ResponseModelBase> DoLogout([FromBody]AuthenticatedRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");

            var session = await ldb.GetSessionFromKey(model.SessionKey);
            if (session == null)
                return ErrorModel.Of("not_logged_in");
            await ldb.RemoveSession(session);

            return OkModel.Of("logged_out");
        }
    }
}
