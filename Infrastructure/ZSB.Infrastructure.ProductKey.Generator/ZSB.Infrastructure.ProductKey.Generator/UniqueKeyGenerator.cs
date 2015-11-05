using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.ProductKey.Generator
{
    class UniqueKeyGenerator
    {
        private static RandomNumberGenerator rng = RandomNumberGenerator.Create();
        public static string GenerateUniqueKey(string prefix, string allowedCharacters, int charCount)
        {
            byte[] indices = new byte[charCount - prefix.Length];
            rng.GetBytes(indices);
            string result =
                new string(indices.Select(a => a % allowedCharacters.Length).Select(a => allowedCharacters[a]).ToArray());

            return prefix + "-" + string.Join("-", Split(result, 4));
        }

        static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
    }
}
