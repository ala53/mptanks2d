using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSB.Infrastructure.Apis.Login.Backend;
using ZSB.Infrastructure.Apis.Login.Database;
using ZSB.Infrastructure.Apis.Login.Models;

namespace ZSB.Infrastructure.Apis.Login.Controllers
{
    [Route("/account")]
    public class ForgotPasswordController : Controller
    {
        private LoginDB ldb;

        public ForgotPasswordController(Database.Contexts.LoginDatabaseContext dbContext)
        {
            ldb = new LoginDB(dbContext);
        }

        [HttpPost, Route("password/forgot/request")]
        public async Task<ResponseModelBase> SendForgotPasswordMessage([FromBody]ForgotPasswordRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");

            //Find them
            var user = await ldb.FindByEmailAddress(model.EmailAddress, true);

            if (user == null)
                return ErrorModel.Of("user_not_found");

            await EmailSender.SendForgotPasswordEmail(user);

            return OkModel.Of("forgot_password_request_sent");
        }

        [HttpPost, Route("password/forgot/change")]
        public async Task<ResponseModelBase> ChangeForgottenPassword([FromBody]ForgotPasswordDoChangeModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");

            if (model.NewPassword.Length < 8)
                return ErrorModel.Of("password_too_short");
            
            //Change their password
            var user = await ldb.FindByUniqueId(model.UserId, true);
            //validate user
            if (user == null)
                return ErrorModel.Of("user_not_found");
            //and code
            if (user.UniqueConfirmationCode != model.ConfirmationCode)
                return ErrorModel.Of("email_confirmation_code_incorrect");

            user.PasswordHashes = await Task.Run(() => PasswordHasher.GenerateHashPermutations(model.NewPassword));
            user.UniqueConfirmationCode = Guid.NewGuid();
            //Clear all sessions
            ldb.DBContext.Sessions.RemoveRange(user.ActiveSessions);
            user.ActiveSessions.Clear();
            //And login tokens
            ldb.DBContext.ServerTokens.RemoveRange(user.ActiveServerTokens);
            user.ActiveServerTokens.Clear();
            //And save
            await ldb.UpdateUser(user);

            return OkModel.Of("password_changed");
        }
    }
}
