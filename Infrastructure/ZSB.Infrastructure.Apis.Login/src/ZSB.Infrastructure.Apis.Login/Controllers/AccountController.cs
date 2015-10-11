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
    [Route("/")]
    public class AccountController : Controller
    {
        private LoginDB ldb;
        public AccountController(Database.Contexts.LoginDatabaseContext ctx)
        {
            ldb = new LoginDB(ctx);
        }

        [HttpPost, Route("password/change")]
        public ResponseModelBase ChangePassword([FromBody]Models.ChangePasswordRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");

            if (!ldb.ValidateAccount(model.Username, model.OldPassword))
            {
                return ErrorModel.Of("username_or_password_incorrect");
            }

            var user = ldb.GetUser(model.Username);
            user.PasswordHashes = PasswordHasher.GenerateHashPermutations(model.NewPassword);
            ldb.UpdateUser(user);
            return OkModel.Empty;
        }

        [HttpGet, Route("test/get")]
        public ResponseModelBase GetValidationTest()
        {
            return OkModel.Of(AccountTests.GetRandomQuestion());
        }

        [HttpGet, Route("test/validate/{id}/{answer}")]
        public ResponseModelBase CheckValidationTest(int id, string answer)
        {
            if (AccountTests.Validate(id, answer))
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
            if (ldb.GetUser(model.EmailAddress) != null)
                return ErrorModel.Of("email_in_use");
            if (!EmailAddressVerifier.IsValidEmail(model.EmailAddress))
                return ErrorModel.Of("email_invalid");
            //Username
            if (ldb.DBContext.Users.Where(a => a.Username == model.Username).FirstOrDefault() != null)
                return ErrorModel.Of("username_in_use");
            //And password
            if (model.Password.Length < 8)
                return ErrorModel.Of("password_too_short");
            //And check the question/answer section
            if (!AccountTests.Validate(model.ChallengeId, model.ChallengeAnswer))
                return ErrorModel.Of("validation_incorrect");

            //Save user in the DB
            ldb.DBContext.Users.Add(um);
            ldb.Save();

            //Send the registration email
            await AccountCreation.SendRegistrationEmail(um);

            return OkModel.Empty;
        }
    }
}
