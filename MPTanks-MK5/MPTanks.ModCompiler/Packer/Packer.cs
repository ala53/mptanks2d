using MPTanks.Modding.Unpacker;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using Newtonsoft.Json;
using ICSharpCode.SharpZipLib.Zip;

namespace MPTanks.ModCompiler.Packer
{
    public static class Packer
    {
        public static byte[] Pack()
        {
            var header = new ModHeader();
            header.Name = Program.name;
            header.Major = DependencyResolver.ParseVersionMajor(Program.version);
            header.Minor = DependencyResolver.ParseVersionMinor(Program.version);
            header.Tag = DependencyResolver.ParseVersionTag(Program.version);
            header.CodeFiles = GetFileNamesOnly(Program.srcFiles).ToArray();
            header.ComponentFiles = GetFileNamesOnly(Program.components).ToArray();
            header.DLLFiles = GetFileNamesOnly(Program.dlls).ToArray();
            header.ImageFiles = GetFileNamesOnly(Program.imageAssets).ToArray();
            header.SoundFiles = GetFileNamesOnly(Program.soundAssets).ToArray();
            header.DatabaseUrl = DependencyResolver.GetModUrl(Program.name);
            header.Description = Program.description;
            header.Dependencies = BuildDependencies();

            var headerString = JsonConvert.SerializeObject(header);
            var ms = new MemoryStream();
            var zipFile = new ZipFile(ms);
            

            return null;
        }

        private static ModDependency[] BuildDependencies()
        {
            return Program.dependencies.Select(a => new ModDependency
            {
                DatabaseUrl = DependencyResolver.GetModUrl(DependencyResolver.ParseDependencyName(a)),
                Major = DependencyResolver.ParseVersionMajor(DependencyResolver.ParseDependencyVersion(a)),
                Minor = DependencyResolver.ParseVersionMinor(DependencyResolver.ParseDependencyVersion(a)),
                ModName = DependencyResolver.ParseDependencyName(a)
            }).ToArray();
        }

        private static List<string> GetFileNamesOnly(List<string> input) =>
            input.Select(a => new FileInfo(a).Name).ToList();
        private static string GetFileNameOnly(string input)
        {
            var fi = new FileInfo(input);
            return fi.Name;
        }
    }
}