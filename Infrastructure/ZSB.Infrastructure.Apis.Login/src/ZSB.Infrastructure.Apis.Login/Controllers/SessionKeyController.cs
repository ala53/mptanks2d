using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSB.Infrastructure.Apis.Login.Database;
using ZSB.Infrastructure.Apis.Login.Database.Contexts;
using ZSB.Infrastructure.Apis.Login.Models;

namespace ZSB.Infrastructure.Apis.Login.Controllers
{
    [Route("/")]
    public class SessionKeyController : Controller
    {
        private LoginDB ldb;
        public SessionKeyController(LoginDatabaseContext ctx)
        {
            ldb = new LoginDB(ctx);
        }
        [HttpPost, Route("validate/key")]
        public async Task<ResponseModelBase<bool>> ValidateSessionKey([FromBody]AuthenticatedRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of(false, "invalid_request");

            var session = await ldb.FindBySessionKey(model.SessionKey);
            if (session == null)
                return OkModel.Of(false);
            //and tell the client that the session key is true
            return OkModel.Of(true);
        }
        [HttpPost, Route("validate/key/info")]
        public async Task<ResponseModelBase<UserInfoResponseModel>> ValidateSessionKeyWithInfo([FromBody]AuthenticatedRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of<UserInfoResponseModel>(null, "invalid_request");

            var session = await ldb.FindBySessionKey(model.SessionKey);
            if (session == null) 
                return OkModel.Of<UserInfoResponseModel>(null);
            //and tell the client that the session key is true
            return OkModel.Of(new UserInfoResponseModel(session));
        }

        [HttpPost, Route("refresh")]
        public async Task<ResponseModelBase<bool>> RefreshSessionKey([FromBody]AuthenticatedRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of(false, "invalid_request");

            var session = await ldb.GetSessionFromKey(model.SessionKey);
            if (session == null)
                return ErrorModel.Of(false, "not_logged_in"); //Auth failed
            session.ExpiryDate = DateTime.UtcNow + ldb.LoginLength;
            await Task.Run(() => ldb.DBContext.Sessions.Update(session));
            await ldb.Save();

            return OkModel.Of(true);
        }
    }
}
