using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MPTanks.Modding.Unpacker
{
    static class DependencyResolver
    {
        /// <summary>
        /// Resolves a dependency and returns all DLL files that the dependency uses
        /// </summary>
        /// <param name="name"></param>
        /// <param name="verMajor"></param>
        /// <param name="verMinor"></param>
        /// <param name="canBeNewer"></param>
        /// <param name="searchDirs"></param>
        /// <param name="dllDir"></param>
        /// <param name="imageDir"></param>
        /// <param name="soundDir"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public static IEnumerable<string> LoadDependency(string name, int verMajor, int verMinor,
            string dllDir, string mapDir, string assetDir, string caller, bool overwriteExisting)
        {
            List<string> _dependencyDlls = new List<string>();

            if (!ModDatabase.Contains(name))
                return Enumerable.Empty<string>();

            var dbItem = ModDatabase.Get(name);
            bool versionOk = true;
            if (verMajor > dbItem.Major)
                versionOk = false;
            else if (verMajor == dbItem.Major && verMinor < dbItem.Minor)
                versionOk = false;

            if (!versionOk)
                throw new Exception("Could not resolve to an appropriate version of a dependency. All versions are too old.");

            if (IsCircular(dbItem.File, caller))
                throw new Exception($"{name} and {caller} reference each other circularly. Cannot load either.");

            //Resolve and load the dependency
            string errors;
            var module = ModLoader.LoadMod(dbItem.File, dllDir, mapDir, assetDir, out errors, 
                dbItem.UsesWhitelist, overwriteExisting);

            if (errors != null)
                throw new Exception(errors);
            //And then add all the dependencies 
            _dependencyDlls.AddRange(module.Assemblies.Select(a => a.Location));
            _dependencyDlls.AddRange(module.Dependencies.Select(a => a.Location));

            return _dependencyDlls.Distinct();
        }

        private static bool IsCircular(string filename, string caller)
        {
            //Catch circular references
            var head = ModUnpacker.GetHeader(filename);
            foreach (var dep in head.Dependencies)
            {
                if (dep.ModName.ToLower() == caller.ToLower())
                    return true;
            }

            return false;
        }
    }
}
