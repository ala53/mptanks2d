using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZSB.Drm.Client.Models;
using ZSB.Drm.Client.Models.Response;

namespace ZSB.Drm.Client
{
    public class MultiplayerDrmClient
    {
        internal MultiplayerDrmClient()
        {

        }

        public string GetServerToken() => GetServerTokenAsync().Result;
        public Task<string> GetServerTokenAsync()
        {
            return Task.Run(() =>
            {
                DrmClient.EnsureLoggedIn(false);

                var resp = RestClient.DoPost<GetServerTokenResponse>("Token/Get", new
                {
                    SessionKey = DrmClient.SessionKey
                }).Result;

                return resp.Data.Token;
            });
        }

        public PublicUserInfo ValidateServerToken(string token) =>
            ValidateServerTokenAsync(token).Result;
        public Task<PublicUserInfo> ValidateServerTokenAsync(string token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            return Task.Run(() =>
            {
                var resp = RestClient.DoPost<PublicUserInfo>("Token/Validate", new
                {
                    ServerToken = token
                }).Result;

                if (resp.Message == "email_not_confirmed")
                    throw new Exceptions.AccountEmailNotConfirmedException();

                if (resp.Message == "token_not_found" || resp.Message == "token_expired")
                    throw new Exceptions.MultiplayerAuthTokenInvalidException(resp.Message);

                return resp.Data;
            });
        }
    }
}
