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
    public class EmailController : Controller
    {
        private LoginDB ldb;
        public EmailController(LoginDatabaseContext dbContext)
        {
            ldb = new LoginDB(dbContext);
        }

        [HttpPost, Route("email/change")]
        public async Task<ResponseModelBase> ChangeEmail([FromBody]ChangeEmailRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");

            UserModel usr;
            if (!ldb.IsAuthorized(model.SessionKey, out usr))
                return ErrorModel.Of("not_logged_in");

            usr.EmailAddress = model.NewEmailAddress;
            usr.EmailConfirmCode = Guid.NewGuid();
            usr.EmailConfirmationSent = DateTime.Now;
            usr.IsEmailConfirmed = false;

            ldb.UpdateUser(usr);

            await Backend.EmailSender.SendRegistrationEmail(usr);

            return OkModel.Of("email_address_changed");
        }

        [HttpGet, Route("confirm/{accountId}/{confirmCode}")]
        public ResponseModelBase ConfirmAccount(Guid accountId, Guid confirmCode)
        {
            var account = ldb.DBContext.Users.Where(a => a.UniqueId == accountId).FirstOrDefault();
            if (account == null)
                return ErrorModel.Of("user_not_found");

            if (account.IsEmailConfirmed)
                return ErrorModel.Of("already_confirmed");

            if (account.EmailConfirmCode != confirmCode)
                return ErrorModel.Of("confirm_code_wrong");

            //Regenerate the code so the link doesn't work anymore
            account.EmailConfirmCode = Guid.NewGuid();
            account.IsEmailConfirmed = true;

            ldb.UpdateUser(account);

            return OkModel.Of("email_confirmed");
        }
        [HttpGet, Route("disavow/{accountId}/{confirmCode}")]
        public ResponseModelBase DisavowAccount(Guid accountId, Guid confirmCode)
        {
            var account = ldb.DBContext.Users.Where(a => a.UniqueId == accountId).FirstOrDefault();
            //Delete the account
            if (account == null)
                return ErrorModel.Of("user_not_found");

            if (account.IsEmailConfirmed)
                return ErrorModel.Of("already_confirmed");

            if (account.EmailConfirmCode != confirmCode)
                return ErrorModel.Of("confirm_code_wrong");

            //Delete the account: they disavowed it
            ldb.DBContext.Remove(account);
            ldb.Save();

            return OkModel.Of("account_disavowed_and_deleted");
        }
    }
}
