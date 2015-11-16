using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZSB.Drm.Client.Models;
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
            return Task.Run<string>(() => { return string.Empty; });
        }

        public ValidateServerTokenResult ValidateServerToken(string token) =>
            ValidateServerTokenAsync(token).Result;
        public Task<ValidateServerTokenResult> ValidateServerTokenAsync(string token)
        {
            return Task.Run<ValidateServerTokenResult>(() => { return (ValidateServerTokenResult)null; });
        }
    }
}
