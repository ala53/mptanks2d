using MPTanks.ModCompiler.Packer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.ModCompiler
{
    static class DependencyResolver
    {
        /// <summary>
        /// Tries to resolve the mod using the ZSB API
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public static bool TryResolve(string dep)
        {
            return false;
        }

        public static bool ModExists(string name)
        {
            return true;
        }

        public static int ParseVersionMajor(string versionTag)
        {
            return int.Parse(versionTag.Split('.')[0]);
        }
        public static int ParseVersionMinor(string versionTag)
        {
            return int.Parse(versionTag.Split('.')[1]);
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
