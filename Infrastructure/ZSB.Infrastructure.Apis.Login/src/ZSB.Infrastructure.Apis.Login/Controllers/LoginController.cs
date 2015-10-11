using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ZSB.Infrastructure.Apis.Login.Database;
using ZSB.Infrastructure.Apis.Login.Models;

namespace ZSB.Infrastructure.Apis.Login.Controllers
{
    [Route("/")]
    public class LoginController : Controller
    {
        private LoginDB ldb;
        const int ServerTokenValidityInSeconds = 60;
        public LoginController(Database.Contexts.LoginDatabaseContext ctx)
        {
            ldb = new LoginDB(ctx);
        }
        [HttpPost, Route("login")]
        public ResponseModelBase DoLogin([FromBody]LoginInfoRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");

            try
            {
                var resp = ldb.DoLogin(model.Email, model.Password);
                if (resp == null)
                    return ErrorModel.Of("unknown_error");
                return OkModel.Of(resp);
            }
            catch (ArgumentException e)
            {
                return ErrorModel.Of(e.Message);
            }
        }

        [HttpPost, Route("logout")]
        public ResponseModelBase DoLogout([FromBody]AuthenticatedRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");

            var session = ldb.FindBySessionKey(model.SessionKey);
            if (session == null)
                return ErrorModel.Of("not_logged_in");
            ldb.RemoveSession(session);

            return OkModel.Of("logged_out");
        }

        [HttpPost, Route("validate/key")]
        public ResponseModelBase<bool> ValidateSessionKey([FromBody]AuthenticatedRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of(false, "invalid_request");

            var session = ldb.FindBySessionKey(model.SessionKey);
            if (session == null)
                return ErrorModel.Of(false);
            //and tell the client that the session key is true
            return OkModel.Of(true);
        }

        [HttpPost, Route("refresh")]
        public ResponseModelBase<bool> RefreshSessionKey([FromBody]AuthenticatedRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of(false, "invalid_request");

            var session = ldb.FindBySessionKey(model.SessionKey);
            if (session == null)
                return ErrorModel.Of(false, "not_logged_in"); //Auth failed
            session.ExpiryDate = DateTime.UtcNow + ldb.LoginLength;
            ldb.DBContext.Sessions.Update(session);
            ldb.Save();

            return OkModel.Of(true);
        }

        [HttpPost, Route("token/get")]
        public ResponseModelBase<UserServerTokenModel> CreateServerToken([FromBody]AuthenticatedRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of<UserServerTokenModel>(null, "invalid_request");

            var session = ldb.FindBySessionKey(model.SessionKey);
            if (session == null)
                return ErrorModel.Of<UserServerTokenModel>(null, "not_logged_in"); //Auth failed

            var token = new UserServerTokenModel();
            token.ExpiryDate = DateTime.UtcNow + TimeSpan.FromMinutes(2);
            token.User = session.Owner;
            token.ServerToken = Guid.NewGuid().ToString("N");

            session.Owner.ActiveServerTokens.Add(token);

            ldb.DBContext.ServerTokens.Add(token);
            ldb.UpdateUser(session.Owner);

            return OkModel.Of(token);
        }

        [HttpPost, Route("token/validate")]
        public ResponseModelBase<UserInfoResponse> ValidateServerToken([FromBody]ValidateServerTokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of<UserInfoResponse>(null, "invalid_request");

            var token = ldb.DBContext.ServerTokens.Where(a => a.ServerToken == model.ServerToken).FirstOrDefault();
            if (token == null)
                return ErrorModel.Of<UserInfoResponse>(null, "token_not_found");
            if (DateTime.Now > token.ExpiryDate)
            {
                //Remove it
                token?.User.ActiveServerTokens.Remove(token);
                ldb.DBContext.ServerTokens.Remove(token);
                ldb.Save();
                return ErrorModel.Of<UserInfoResponse>(null, "token_expired");
            }

            if (!token.User.IsEmailConfirmed)
                return ErrorModel.Of<UserInfoResponse>(null, "account_email_not_confirmed");

            var resp = new UserInfoResponse(token.User);
            //Remove it
            token?.User.ActiveServerTokens.Remove(token);
            ldb.DBContext.ServerTokens.Remove(token);
            ldb.Save();

            return OkModel.Of(resp);
        }
    }
}
