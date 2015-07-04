using MPTanks.Modding.Unpacker;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using Newtonsoft.Json;
using ICSharpCode.SharpZipLib.Zip;
using System.Drawing;
using System.Text;
using ICSharpCode.SharpZipLib.Core;

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
            header.MapFiles = GetFileNamesOnly(Program.maps).ToArray();
            header.DatabaseUrl = DependencyResolver.GetModUrl(Program.name);
            header.Description = Program.description;
            header.Dependencies = BuildDependencies();

            var headerString = JsonConvert.SerializeObject(header, Formatting.Indented);
            var ms = new MemoryStream();
            var zipFile = new ZipOutputStream(ms);
            zipFile.IsStreamOwner = false;

            WriteFile(zipFile, "mod.json", headerString);

            Archive(zipFile, Program.srcFiles);
            Archive(zipFile, Program.components);
            Archive(zipFile, Program.dlls);
            Archive(zipFile, Program.imageAssets.SelectMany(a => new[] { a, a + ".json" }).ToList());
            Archive(zipFile, Program.soundAssets);
            Archive(zipFile, Program.maps);

            zipFile.Close();
            zipFile.Dispose();

            ms.Seek(0, SeekOrigin.Begin);
            var data = ms.ToArray();
            ms.Dispose();
            return ms.ToArray();
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

        private static void Archive(ZipOutputStream zf, List<string> src)
        {
            foreach (var a in src)
            {
                var data = File.ReadAllBytes(a);
                WriteFile(zf, GetFileNameOnly(a), data);
            }
        }

        private static void WriteFile(ZipOutputStream zf, string name, string data) =>
            WriteFile(zf, name, Encoding.UTF8.GetBytes(data));
        private static void WriteFile(ZipOutputStream zf, string name, byte[] data)
        {
            var f = new ZipEntry(name);
            zf.PutNextEntry(f);
            StreamUtils.Copy(new MemoryStream(data), zf, new byte[4096]);
            zf.CloseEntry();
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