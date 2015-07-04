using MPTanks.ModCompiler.Packer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MPTanks.ModCompiler
{
    static class DependencyResolver
    {
        /// <summary>
        /// Tries to resolve the mod using the ZSB API (check if the dependency exists)
        /// Name:MAJOR.MINOR TAG
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public static bool ModExists(string name)
        {
            return true;
        }

        public static bool IsNameValid(string name)
        {
            return new Regex("^[a-zA-Z0-9-]*$").IsMatch(name) && name.Length >= 4 && name.Length <= 64;
        }

        public static string GetModUrl(string name)
        {
            if (!IsNameValid(name))
            {
                Console.WriteLine($"Tha name \"{name}\" is invalid. It must be 4-64 characters, using A-z, 0-9, and - (dash).");
            }
            return $"https://mods.mptanks.zsbgames.me/api/info/{name.ToLower()}";
        }

        public static string ParseDependencyName(string name) => name.Split(':')[0];
        public static string ParseDependencyVersion(string name) => name.Split(':')[1];

        public static int ParseVersionMajor(string versionTag)
        {
            return int.Parse(versionTag.Split('.')[0]);
        }
        public static int ParseVersionMinor(string versionTag)
        {
            return int.Parse(versionTag.Split('.')[1].Split(' ')[0]);
        }
        public static string ParseVersionTag(string versionTag)
        {
            if (versionTag.Split(' ').Length > 1)
                return versionTag.Split(' ')[1];
            else return string.Empty;
        }

        public static bool IsValidVersion(string version)
        {
            try
            {
                ParseVersionMajor(version);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
