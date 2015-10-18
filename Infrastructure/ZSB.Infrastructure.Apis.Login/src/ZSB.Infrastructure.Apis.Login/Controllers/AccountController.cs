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
    /// <summary>
    /// 
    /// </summary>
    [Route("/account")]
    public class AccountController : Controller
    {
        private LoginDB ldb;
        public AccountController(Database.Contexts.LoginDatabaseContext ctx)
        {
            ldb = new LoginDB(ctx);
        }

        [HttpPost, Route("password/change")]
        public async Task<ResponseModelBase> ChangePassword([FromBody]ChangePasswordRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");

            if (!await ldb.ValidateAccount(model.Username, model.OldPassword))
            {
                return ErrorModel.Of("username_or_password_incorrect");
            }

            var user = await ldb.FindByEmailAddress(model.Username, false);
            user.PasswordHashes = await Task.Run(() => PasswordHasher.GenerateHashPermutations(model.NewPassword));
            await ldb.UpdateUser(user);
            return OkModel.Empty;
        }

        [HttpGet, Route("challenge/get")]
        public ResponseModelBase GetValidationTest()
        {
            return OkModel.Of(AccountTests.GetRandomQuestion());
        }

        [HttpGet, Route("challenge/validate/{id}/{answer}")]
        public ResponseModelBase CheckValidationTest(int id, string answer)
        {
            if (AccountTests.ValidateChallenge(id, answer))
                return OkModel.Empty;
            else return ErrorModel.Of("validation_incorrect");
        }

        [HttpPost, Route("register")]
        public async Task<ResponseModelBase> CreateAccount([FromBody]CreateAccountRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");

            var um = new UserModel();
            um.AccountCreationDate = DateTime.UtcNow;
            um.EmailAddress = model.EmailAddress;
            um.EmailConfirmCode = Guid.NewGuid();
            um.EmailConfirmationSent = DateTime.UtcNow;
            um.PasswordHashes = PasswordHasher.GenerateHashPermutations(model.Password);
            um.UniqueId = Guid.NewGuid();
            um.Username = model.Username;

            //And validate the email address
            if (!EmailAddressVerifier.IsValidEmail(model.EmailAddress)) //valid address
                return ErrorModel.Of("email_invalid");
            if (await ldb.FindByEmailAddress(model.EmailAddress, false) != null) //in use
                return ErrorModel.Of("email_in_use");
            //Username
            if (await ldb.FindByUsername(model.Username, false) != null) //also in use
                return ErrorModel.Of("username_in_use");
            //And password
            if (model.Password.Length < 8)
                return ErrorModel.Of("password_too_short");
            //And check the question/answer section
            if (!AccountTests.ValidateChallenge(model.ChallengeId, model.ChallengeAnswer))
                return ErrorModel.Of("validation_incorrect");

            //Send the registration email
            await EmailSender.SendRegistrationEmail(um);
            //Save user in the DB
            await ldb.AddUser(um);

            return OkModel.Of("account_created");
        }
        [HttpGet, Route("delete/confirm/{userId}/{confirmCode}")]
        public async Task<ResponseModelBase> DeleteAccount(Guid userId, Guid confirmCode)
        {
            try
            {
                var usr = await ldb.FindByUniqueId(userId, false);
                if (usr == null)
                    return ErrorModel.Of("user_not_found");

                if (usr.EmailConfirmCode != confirmCode)
                    return ErrorModel.Of("email_confirmation_code_incorrect");

                await ldb.DeleteUser(usr);

                return OkModel.Of("account_deleted");
            }
            catch (Exception ex)
            {
                return ErrorModel.Of(ex.Message);
            }
        }
        [HttpPost, Route("delete/request")]
        public async Task<ResponseModelBase> RequestDeleteAccount([FromBody]AuthenticatedRequestModel model)
        {
            try
            {
                if (!await ldb.Validate(model))
                    return ErrorModel.Of("not_logged_in");

                await EmailSender.SendDeletionEmail(await ldb.FindBySessionKey(model.SessionKey));
                return OkModel.Of("delete_confirmation_email_sent");
            }
            catch (Exception e)
            {
                return ErrorModel.Of(e.Message);
            }
        }
    }
}
