using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSB.Infrastructure.Apis.Account.Backend;
using ZSB.Infrastructure.Apis.Account.Database;
using ZSB.Infrastructure.Apis.Account.Models;

namespace ZSB.Infrastructure.Apis.Account.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private LoginDB ldb;

        public ForgotPasswordController(Database.Contexts.LoginDatabaseContext dbContext)
        {
            ldb = new LoginDB(dbContext);
        }

        [HttpPost, Route("/Account/Password/Forgot/Request")]
        public async Task<ResponseModelBase> SendForgotPasswordMessage([FromBody]ForgotPasswordRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");

            //Find them
            var user = await ldb.FindByEmailAddress(model.EmailAddress);

            if (user == null)
                return ErrorModel.Of("user_not_found");

            await EmailSender.SendEmail(user, EmailSender.ForgotPasswordTemplate);

            return Models.OkModel.Of("forgot_password_request_sent");
        }

        [HttpPost, Route("/Account/Password/Forgot/Change")]
        public async Task<ResponseModelBase> ChangeForgottenPassword([FromBody]ForgotPasswordDoChangeRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");

            if (model.NewPassword.Length < 8)
                return ErrorModel.Of("password_too_short");
            
            //Change their password
            var user = await ldb.FindByUniqueId(model.UserId);
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

            return Models.OkModel.Of("password_changed");
        }
    }
}
