using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSB.Infrastructure.Apis.Account.Database;
using ZSB.Infrastructure.Apis.Account.Database.Contexts;
using ZSB.Infrastructure.Apis.Account.Models;

namespace ZSB.Infrastructure.Apis.Account.Controllers
{
    public class SessionKeyController : Controller
    {
        private LoginDB ldb;
        public SessionKeyController(LoginDatabaseContext ctx)
        {
            ldb = new LoginDB(ctx);
        }
        [HttpPost, Route("/Key/Validate")]
        public async Task<ResponseModelBase<bool>> ValidateSessionKey([FromBody]AuthenticatedRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of(false, "invalid_request");

            var session = await ldb.FindBySessionKey(model.SessionKey);
            if (session == null)
                return Models.OkModel.Of(false);
            //and tell the client that the session key is true
            return Models.OkModel.Of(true);
        }
        [HttpPost, Route("/Key/Validate/Info")]
        public async Task<ResponseModelBase<UserInfoResponseModel>> ValidateSessionKeyWithInfo([FromBody]AuthenticatedRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of<UserInfoResponseModel>(null, "invalid_request");

            var session = await ldb.FindBySessionKey(model.SessionKey);
            if (session == null) 
                return Models.OkModel.Of<UserInfoResponseModel>(null);
            //and tell the client that the session key is true
            return Models.OkModel.Of(new UserInfoResponseModel(session));
        }

        [HttpPost, Route("/Key/Refresh")]
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

            return Models.OkModel.Of(true);
        }
    }
}
