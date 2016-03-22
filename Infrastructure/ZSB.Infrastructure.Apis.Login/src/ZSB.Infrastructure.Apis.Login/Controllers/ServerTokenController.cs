using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSB.Infrastructure.Apis.Account.Database;
using ZSB.Infrastructure.Apis.Account.Database.Contexts;
using ZSB.Infrastructure.Apis.Account.Models;

namespace ZSB.Infrastructure.Apis.Account.Controllers
{
    public class ServerTokenController : Controller
    {
        private LoginDB ldb;
        public ServerTokenController(LoginDatabaseContext ctx)
        {
            ldb = new LoginDB(ctx);
        }

        [HttpPost, Route("/Token/Get")]
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

        [HttpPost, Route("/Token/Validate")]
        public async Task<ResponseModelBase<UserInfoResponseModel>> ValidateServerToken([FromBody]ValidateServerTokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of<UserInfoResponseModel>(null, "invalid_request");

            var token = ldb.DBContext.ServerTokens
                .Include(a => a.Owner)
                .ThenInclude(a => a.OwnedProducts)
                .Where(a => a.ServerToken == model.ServerToken).FirstOrDefault();
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
                return ErrorModel.Of<UserInfoResponseModel>(null, "email_not_confirmed");

            var resp = new UserInfoResponseModel(token.Owner);
            //Remove it
            token.Owner.RemoveToken(token);
            ldb.DBContext.ServerTokens.Remove(token);

            await ldb.Save();
            return Models.OkModel.Of(resp);
        }
    }
}
