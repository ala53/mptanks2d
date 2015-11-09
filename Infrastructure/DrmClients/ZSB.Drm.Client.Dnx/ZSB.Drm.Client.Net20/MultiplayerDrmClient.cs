using System;
using System.Collections.Generic;
using System.Text;
using ZSB.Drm.Client.Results;

namespace ZSB.Drm.Client
{
    public class MultiplayerDrmClient
    {
        internal MultiplayerDrmClient()
        {

        }

        public static string GetServerToken() => GetServerTokenAsync().Wait();
        public static Promise<string> GetServerTokenAsync()
        {

        }

        public static ValidateServerTokenResult ValidateServerToken(string token) =>
            ValidateServerTokenAsync(token).Wait();
        public static Promise<ValidateServerTokenResult> ValidateServerTokenAsync(string token)
        {

        }
    }
}
