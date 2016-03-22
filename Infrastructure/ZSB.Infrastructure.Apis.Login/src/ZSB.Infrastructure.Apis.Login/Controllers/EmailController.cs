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
    public class EmailController : Controller
    {
        private LoginDB ldb;
        public EmailController(LoginDatabaseContext dbContext)
        {
            ldb = new LoginDB(dbContext);
        }

        [HttpPost, Route("/Email/Change")]
        public async Task<ResponseModelBase> ChangeEmail([FromBody]ChangeEmailRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");

            UserModel usr;
            if (await ldb.ValidateAccount(model.EmailAddress, model.Password))
                return ErrorModel.Of("username_or_password_incorrect");

            //retrieve the user
            usr = await ldb.FindByEmailAddress(model.EmailAddress);
            if (usr == null)
                return ErrorModel.Of("user_not_found");

            //make sure that the email address isn't in use
            var other = await ldb.FindByEmailAddress(model.NewEmailAddress);
            if (other != null)
                return ErrorModel.Of("email_address_in_use");

            usr.EmailAddress = model.NewEmailAddress;
            usr.UniqueConfirmationCode = Guid.NewGuid();
            usr.EmailConfirmationSent = DateTime.Now;
            usr.IsEmailConfirmed = false;

            Task.WaitAll(
                ldb.UpdateUser(usr),
                Backend.EmailSender.SendEmail(usr, Backend.EmailSender.RegistrationTemplate)
            );

            return Models.OkModel.Of("email_address_changed");
        }

        [HttpPost, Route("/Email/Resend")]
        public async Task<ResponseModelBase> ResendConfirmationEmail([FromBody]ResendConfirmationRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");

            UserModel usr;

            if ((usr = await ldb.FindByEmailAddress(model.EmailAddress)) == null)
                return ErrorModel.Of("user_not_found");

            await ldb.ChangeConfirmCode(usr);

            await Backend.EmailSender.SendEmail(usr, Backend.EmailSender.RegistrationTemplate);

            return Models.OkModel.Of("email_confirmation_code_resent");
        }

        [HttpGet, Route("/Email/Confirm/{accountId}/{confirmCode}")]
        public async Task<ResponseModelBase> ConfirmAccount(Guid accountId, Guid confirmCode)
        {
            var account = await ldb.FindByUniqueId(accountId);
            if (account == null)
                return ErrorModel.Of("user_not_found");

            if (account.IsEmailConfirmed)
                return ErrorModel.Of("email_already_confirmed");

            if (account.UniqueConfirmationCode != confirmCode)
                return ErrorModel.Of("email_confirmation_code_incorrect");

            //Regenerate the code so the link doesn't work anymore
            await ldb.ChangeConfirmCode(account);
            account.IsEmailConfirmed = true;

            await ldb.UpdateUser(account);

            return Models.OkModel.Of("email_confirmed");
        }
        [HttpGet, Route("/Email/Disavow/{accountId}/{confirmCode}")]
        public async Task<ResponseModelBase> DisavowAccount(Guid accountId, Guid confirmCode)
        {
            var account = await ldb.FindByUniqueId(accountId);
            //Delete the account
            if (account == null)
                return ErrorModel.Of("user_not_found");

            if (account.IsEmailConfirmed)
                return ErrorModel.Of("email_already_confirmed");

            if (account.UniqueConfirmationCode != confirmCode)
                return ErrorModel.Of("email_confirmation_code_incorrect");

            //Delete the account: they disavowed it
            await ldb.DeleteUser(account);

            return Models.OkModel.Of("account_deleted");
        }
    }
}
