using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AssetCompileHelper
{
    class Program
    {

        private static Dictionary<string, string> _filesAndMD5s = new Dictionary<string, string>();
        static int Main(string[] args)
        {
            var inDir = args[0];

            bool ok = true;

            List<string> files = new List<string>();
            foreach (var file in System.IO.Directory.GetFiles(inDir))
            {
                FileInfo fi = new FileInfo(file);

                if (fi.Extension.Contains("spritefont") || fi.Extension.Contains("png") ||
                    fi.Extension.Contains("bmp") || fi.Extension.Contains("jpg") ||
                    fi.Extension.Contains("jpeg") || fi.Extension.Contains("tiff") ||
                    fi.Extension.Contains("tif") || fi.Extension.Contains("gif") ||
                    fi.Extension.Contains("wav") || fi.Extension.Contains("ogg"))
                {
                    files.Add(file);
                }
            }

            if (files.Count == 0)
                return -2;

            string cmdArgs = "";
            var needingRecompile = GetAssetsNeedingRecompile(files);
            if (needingRecompile.Length == 0) return 0;
            foreach (var file in needingRecompile)
            {
                cmdArgs += String.Format("/build:\"{0}\" ", file);
                _filesAndMD5s[file] = CalculateMD5Hash(System.IO.File.ReadAllText(file));
            }

            var prc = Process.Start(@"C:\Program Files (x86)\MSBuild\MonoGame\v3.0\Tools\MGCB.exe",
                cmdArgs);
            prc.WaitForExit();

            if (prc.ExitCode != 0) ok = false;
            WriteDictionary();

            return ok ? 0 : -2;
        }

        private static string[] GetAssetsNeedingRecompile(IEnumerable<string> inputFiles)
        {
            if (System.IO.File.Exists("asset_md5s.json"))
                Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    System.IO.File.ReadAllText("asset_md5s.json"));

            List<string> _outFiles = new List<string>();
            foreach (var file in inputFiles)
            {
                if (!_filesAndMD5s.ContainsKey(file))
                {
                    _outFiles.Add(file);
                    continue;
                }
                if (CalculateMD5Hash(System.IO.File.ReadAllText(file)) != _filesAndMD5s[file])
                    _outFiles.Add(file);
            }

            return _outFiles.ToArray();
        }

        private static void WriteDictionary()
        {
            System.IO.File.WriteAllText("asset_md5s.json", Newtonsoft.Json.JsonConvert.SerializeObject(_filesAndMD5s));
        }
        private static string CalculateMD5Hash(string input)
        {
            return CalculateMD5Hash(System.Text.Encoding.UTF8.GetBytes(input));
        }
        private static string CalculateMD5Hash(byte[] inputBytes)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

    }
}
