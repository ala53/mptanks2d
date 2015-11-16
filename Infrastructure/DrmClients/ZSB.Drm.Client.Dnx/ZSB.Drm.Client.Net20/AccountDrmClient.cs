using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZSB.Drm.Client
{
    public class AccountCreationChallenge
    {
        public int Id { get; set; }
        public string Question { get; set; }
    }
    public class AccountDrmClient
    {
        internal AccountDrmClient()
        {

        }

        /// <summary>
        /// Sends a "forgot my password" email to the person at the specified email address. Useful if you want
        /// to add a "forgot my password" link to your login page.
        /// </summary>
        /// <param name="emailAddress"></param>
        public void SendForgotPasswordEmail(string emailAddress) =>
            SendForgotPasswordEmailAsync(emailAddress).Wait();

        public Task SendForgotPasswordEmailAsync(string emailAddress)
        {
            if (emailAddress == null) throw new ArgumentNullException(nameof(emailAddress));
            return Task.Run(() =>
            {
                var res = RestClient.DoPost("Account/Password/Forgot/Request",
                    new { EmailAddress = emailAddress }).Result;

                if (res.Error)
                    throw new Exceptions.AccountDetailsIncorrectException("user_not_found");
            });
        }

        public AccountCreationChallenge GetChallenge() =>
            GetChallengeAsync().Result;

        public Task<AccountCreationChallenge> GetChallengeAsync()
        {
            return Task.Run(() =>
            {
                var res = RestClient.DoGet<AccountCreationChallenge>("Account/Challenge/Get").Result;
                if (res.Error)
                    throw new Exceptions.InvalidAccountServerResponseException();

                return res.Data;
            });
        }

        public void Create(string username, string emailAddress, string password, AccountCreationChallenge challenge, string response) =>
            CreateAsync(username, emailAddress, password, challenge, response).Wait();

        public Task CreateAsync(string username, string emailAddress, string password, AccountCreationChallenge challenge, string challengeAnswer)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (emailAddress == null) throw new ArgumentNullException(nameof(emailAddress));
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (challenge == null) throw new ArgumentNullException(nameof(challenge));
            if (challengeAnswer == null) throw new ArgumentNullException(nameof(challengeAnswer));
            return Task.Run(() =>
            {
                var resp = RestClient.DoPost("Account/Create", new
                {
                    ChallengeId = challenge.Id,
                    ChallengeAnswer = challengeAnswer,
                    EmailAddress = emailAddress,
                    Username = username,
                    Password = password
                }).Result;

                if (resp.Error)
                    throw new Exceptions.AccountCreationException(resp.Message);
            });
        }
    }
}
