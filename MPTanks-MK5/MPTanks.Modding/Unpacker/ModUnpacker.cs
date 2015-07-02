using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Modding.Unpacker
{
    public static class ModUnpacker
    {
        private static ZipEntry GetEntry(string filename, ZipFile zf)
        {
            foreach (ZipEntry ze in zf)
            {
                if (ze.Name.Equals(filename, StringComparison.InvariantCultureIgnoreCase))
                    return ze;
            }
            return null;
        }
        private static byte[] GetData(string filename, ZipFile zf)
        {
            var stream = zf.GetInputStream(GetEntry(filename, zf));
            var bytes = new List<byte>();
            int bt;
            while ((bt = stream.ReadByte()) != -1)
                bytes.Add((byte)bt);
            return bytes.ToArray();
        }

        private static string ReadText(string filename, ZipFile zf)
        {
            return Encoding.UTF8.GetString(GetData(filename, zf));
        }

        private static ZipFile OpenZip(string fileName)
        {
            var zf = new ZipFile(new FileStream(fileName, FileMode.Open, FileAccess.Read));
            zf.IsStreamOwner = true;
            return zf;
        }

        public static ModHeader GetHeader(string modFile)
        {
            var zf = OpenZip(modFile);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ModHeader>(ReadText("mod.json", zf));
            zf.Close();
            return obj;
        }

        public static string[] UnpackDlls(string modFile, string outputDir)
        {
            //we unpack to modName_modMajor_modMinor_assetName.dll
            var header = GetHeader(modFile);
            var zf = OpenZip(modFile);
            var dlls = new List<string>();

            foreach (var dll in header.DLLFiles)
            {
                var path = Path.Combine(outputDir, $"{header.Name}_{header.Major}_{header.Minor}_{dll}.dll");
                if (!File.Exists(path))
                    File.WriteAllBytes(path,
                    GetData(dll, zf));
                dlls.Add(path);
            }
            zf.Close();
            return dlls.ToArray();
        }
        public static string[] UnpackSounds(string modFile, string outputDir)
        {
            //we unpack to modFile_modMajor_modMinor_assetName.ogg
            var header = GetHeader(modFile);
            var zf = OpenZip(modFile);

            var files = new List<string>();

            foreach (var sound in header.SoundFiles)
            {
                var path = Path.Combine(outputDir, $"{header.Name}_{header.Major}_{header.Minor}_{sound}.ogg");
                if (!File.Exists(path))
                    File.WriteAllBytes(path,
                        GetData(sound, zf));
                files.Add(path);
            }
            zf.Close();
            return files.ToArray();
        }
        public static string[] UnpackImages(string modFile, string outputDir)
        {
            //we unpack by modFile_modMajor_modMinor_assetName.png
            var header = GetHeader(modFile);
            var zf = OpenZip(modFile);

            var files = new List<string>();

            foreach (var img in header.ImageFiles)
            {
                var ext = img.Split('.').Last();
                var path = Path.Combine(outputDir, $"{header.Name}_{header.Major}_{header.Minor}_{img}.png");
                if (!File.Exists(path))
                    File.WriteAllBytes(path,
                    GetData(img, zf));
                files.Add(path);
            }
            zf.Close();

            return files.ToArray();
        }
        public static string[] GetSourceCode(string modFile)
        {
            var codePages = new List<string>();
            var header = GetHeader(modFile);
            var zf = OpenZip(modFile);

            foreach (var cf in header.CodeFiles)
            {
                codePages.Add(ReadText(cf, zf));
            }
            zf.Close();
            return codePages.ToArray();
        }

        public static string GetStringFile(string modFile, string internalFileName)
        {
            var zf = OpenZip(modFile);
            var text = ReadText(internalFileName, zf);
            zf.Close();
            return text;
        }

        public static byte[] GetByteArrayFile(string modFile, string internalFileName)
        {
            var zf = OpenZip(modFile);
            var data = GetData(internalFileName, zf);
            zf.Close();
            return data;
        }
    }
}
