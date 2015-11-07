using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZSB.Infrastructure.Apis.Account.Backend;
using ZSB.Infrastructure.Apis.Account.Database;
using ZSB.Infrastructure.Apis.Account.Models;

namespace ZSB.Infrastructure.Apis.Account.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountController : Controller
    {
        private LoginDB ldb;
        public AccountController(Database.Contexts.LoginDatabaseContext ctx)
        {
            ldb = new LoginDB(ctx);
        }

        [HttpPost, Route("/Account/Password/Change")]
        public async Task<ResponseModelBase> ChangePassword([FromBody]ChangePasswordRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");

            if (!await ldb.ValidateAccount(model.EmailAddress, model.OldPassword))
            {
                return ErrorModel.Of("username_or_password_incorrect");
            }

            if (model.NewPassword.Length < 8)
                return ErrorModel.Of("password_too_short");

            //Change their password
            var user = await ldb.FindByEmailAddress(model.EmailAddress);

            if (user == null)
                return ErrorModel.Of("user_not_found");

            user.PasswordHashes = await Task.Run(() => PasswordHasher.GenerateHashPermutations(model.NewPassword));
            //Clear all sessions
            ldb.DBContext.Sessions.RemoveRange(user.ActiveSessions);
            user.ActiveSessions.Clear();
            //And login tokens
            ldb.DBContext.ServerTokens.RemoveRange(user.ActiveServerTokens);
            user.ActiveServerTokens.Clear();
            //Update
            await ldb.UpdateUser(user);
            return Models.OkModel.Of("password_changed");
        }
        [HttpPost, Route("/Account/Username/Change")]
        public async Task<ResponseModelBase> ChangeUsername([FromBody]ChangeUsernameRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");

            if (!await ldb.ValidateAccount(model.EmailAddress, model.Password))
            {
                return ErrorModel.Of("username_or_password_incorrect");
            }

            if (model.NewUsername.Length < 5)
                return ErrorModel.Of("username_too_short");

            //Change their username
            var user = await ldb.FindByEmailAddress(model.EmailAddress);

            if (user == null)
                return ErrorModel.Of("user_not_found");

            //Check not in use
            var other = await ldb.FindByUsername(model.NewUsername);
            if (other != null)
                return ErrorModel.Of("username_in_use");

            user.Username = model.NewUsername;
            //Update
            await ldb.UpdateUser(user);
            return Models.OkModel.Of("username_changed");
        }

        [HttpGet, Route("/Account/Challenge/Get")]
        public ResponseModelBase GetValidationTest()
        {
            return Models.OkModel.Of(AccountTests.GetRandomQuestion());
        }

        [HttpGet, Route("/Account/Challenge/Validate/{id}/{answer}")]
        public ResponseModelBase CheckValidationTest(int id, string answer)
        {
            if (AccountTests.ValidateChallenge(id, answer))
                return Models.OkModel.Empty;
            else return ErrorModel.Of("validation_incorrect");
        }

        [HttpPost, Route("/Account/Register")]
        public async Task<ResponseModelBase> CreateAccount([FromBody]CreateAccountRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");

            var um = new UserModel();
            um.AccountCreationDate = DateTime.UtcNow;
            um.EmailAddress = model.EmailAddress;
            um.UniqueConfirmationCode = Guid.NewGuid();
            um.EmailConfirmationSent = DateTime.UtcNow;
            um.PasswordHashes = PasswordHasher.GenerateHashPermutations(model.Password);
            um.UniqueId = Guid.NewGuid();
            um.Username = model.Username.Trim();

            //And validate the email address
            if (!EmailAddressVerifier.IsValidEmail(model.EmailAddress)) //valid address
                return ErrorModel.Of("email_invalid");
            if (await ldb.FindByEmailAddress(model.EmailAddress) != null) //in use
                return ErrorModel.Of("email_in_use");
            //Username
            if (await ldb.FindByUsername(model.Username) != null) //also in use
                return ErrorModel.Of("username_in_use");
            if (um.Username.Length < 5)
                return ErrorModel.Of("username_invalid");
            if (!new Regex(@"[a-zA-Z0-9\s_-]").IsMatch(um.Username))
                return ErrorModel.Of("username_invalid");
            //Password
            if (model.Password.ToLower().Contains("password"))
                return ErrorModel.Of("password_too_simple");
            if (model.Password.ToLower().StartsWith("1234"))
                return ErrorModel.Of("password_too_simple");
            if (model.Password.Length < 8)
                return ErrorModel.Of("password_too_short");
            //And check the question/answer section
            if (!AccountTests.ValidateChallenge(model.ChallengeId, model.ChallengeAnswer))
                return ErrorModel.Of("validation_incorrect");

            //Send the registration email
            await EmailSender.SendEmail(um, EmailSender.RegistrationTemplate);
            //Save user in the DB
            await ldb.AddUser(um);

            return Models.OkModel.Of("account_created");
        }
        [HttpGet, Route("/Account/Delete/Confirm/{userId}/{confirmCode}")]
        public async Task<ResponseModelBase> DeleteAccount(Guid userId, Guid confirmCode)
        {
            var usr = await ldb.FindByUniqueId(userId);
            if (usr == null)
                return ErrorModel.Of("user_not_found");

            if (usr.UniqueConfirmationCode != confirmCode)
                return ErrorModel.Of("email_confirmation_code_incorrect");

            await ldb.DeleteUser(usr);

            return Models.OkModel.Of("account_deleted");
        }
        [HttpPost, Route("/Account/Delete/Request")]
        public async Task<ResponseModelBase> RequestDeleteAccount([FromBody]AuthenticatedRequestModel model)
        {
            try
            {
                if (!await ldb.Validate(model))
                    return ErrorModel.Of("not_logged_in");

                await EmailSender.SendEmail(await ldb.FindBySessionKey(model.SessionKey), EmailSender.DeletionTemplate);
                return Models.OkModel.Of("delete_confirmation_email_sent");
            }
            catch (Exception e)
            {
                return ErrorModel.Of(e.Message);
            }
        }
    }
}
