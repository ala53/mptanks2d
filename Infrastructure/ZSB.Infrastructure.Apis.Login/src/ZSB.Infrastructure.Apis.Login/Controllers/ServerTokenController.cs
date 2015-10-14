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
    public class ServerTokenController : Controller
    {
        private LoginDB ldb;
        public ServerTokenController(LoginDatabaseContext ctx)
        {
            ldb = new LoginDB(ctx);
        }

        [HttpPost, Route("token/get")]
        public async Task<ResponseModelBase<UserServerTokenResponseModel>> CreateServerToken([FromBody]AuthenticatedRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of<UserServerTokenResponseModel>(null, "invalid_request");

            var session = await ldb.GetSessionFromKey(model.SessionKey);
            if (session == null)
                return ErrorModel.Of<UserServerTokenResponseModel>(null, "not_logged_in"); //Auth failed

            var token = new UserServerTokenModel();
            token.ExpiryDate = DateTime.UtcNow + TimeSpan.FromMinutes(2);
            token.ServerToken = Guid.NewGuid().ToString("N");
            session.Owner.AddToken(token);

            await ldb.UpdateUser(session.Owner);

            return OkModel.Of(new UserServerTokenResponseModel(token));
        }

        [HttpPost, Route("token/validate")]
        public async Task<ResponseModelBase<UserInfoResponseModel>> ValidateServerToken([FromBody]ValidateServerTokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of<UserInfoResponseModel>(null, "invalid_request");

            var token = ldb.DBContext.ServerTokens.Where(a => a.ServerToken == model.ServerToken).FirstOrDefault();
            if (token == null)
                return ErrorModel.Of<UserInfoResponseModel>(null, "token_not_found");
            if (DateTime.Now > token.ExpiryDate)
            {
                //Remove it
                token.Owner.RemoveToken(token);
                await ldb.Save();
                return ErrorModel.Of<UserInfoResponseModel>(null, "token_expired");
            }

            if (!token.Owner.IsEmailConfirmed)
                return ErrorModel.Of<UserInfoResponseModel>(null, "account_email_not_confirmed");

            var resp = new UserInfoResponseModel(token.Owner);
            //Remove it
            token.Owner.RemoveToken(token);
            await ldb.Save();

            return OkModel.Of(resp);
        }
    }
}
