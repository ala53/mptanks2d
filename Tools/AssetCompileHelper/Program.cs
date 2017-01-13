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
        private static string AppDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static string AssetLogFile = Path.Combine(AppDir, "asset_log.json");
        private static string AssetMD5sFile = Path.Combine(AppDir, "asset_md5s.json");

        private static Dictionary<string, string> _filesAndMD5s = new Dictionary<string, string>();

        private static string _logFileData = "";
        static int Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDir);

            Console.WriteLine("Started!");

            var mono = args[0] == "mono";
            var mgcbPath = args[1];
            var inDir = args[2];
            var platform = (args.Length >= 4) ? args[3] : "Windows";
            Console.WriteLine("Input Directory: " + inDir);
            Console.WriteLine("MGCB Path: " + mgcbPath);
            Console.WriteLine("Platform: " + platform);
            Console.WriteLine("Mono: " + mono);

            Write($"Searching folder {inDir}");
            Write($"CWD: {Directory.GetCurrentDirectory()}");

            bool ok = true;

            List<string> files = new List<string>();
            foreach (var file in System.IO.Directory.GetFiles(inDir, "*", SearchOption.AllDirectories))
            {
                FileInfo fi = new FileInfo(file);

                if (fi.Extension.Contains("spritefont") || fi.Extension.Contains("png") ||
                    fi.Extension.Contains("bmp") || fi.Extension.Contains("jpg") ||
                    fi.Extension.Contains("jpeg") || fi.Extension.Contains("tiff") ||
                    fi.Extension.Contains("tif") || fi.Extension.Contains("gif") ||
                    fi.Extension.Contains("wav") || fi.Extension.Contains("ogg") || fi.Extension.EndsWith("fx"))
                {
                    Write($"File found: {file}");
                    files.Add(file);
                }
            }

            if (files.Count == 0)
            {
                Write("FATAL: No files found.");
                File.WriteAllText(AssetLogFile, _logFileData);
                return -2;
            }

            string cmdArgs = "";
            var needingRecompile = GetAssetsNeedingRecompile(files, platform);
            if (needingRecompile.Length == 0)
            {
                Write("No files need recompile.");
                File.WriteAllText(AssetLogFile, _logFileData);
                return 0;
            }

            Write($"{needingRecompile} files require recompile.");
            cmdArgs += $"/incremental /platform:{platform} ";
            foreach (var file in needingRecompile)
            {
                Write($"Recompiling {file}\n");
                cmdArgs += String.Format("/build:\"{0}\" ", file);
                _filesAndMD5s[file] = CalculateMD5Hash(System.IO.File.ReadAllText(file));
            }

            Write("Executing mgcb.exe " + cmdArgs);
            Process prc;
            ProcessStartInfo inf;

            if (mono)
                inf = new ProcessStartInfo("mono", $"\"{mgcbPath}\" {cmdArgs}");
            else
                inf = new ProcessStartInfo(mgcbPath,
                   cmdArgs);
            inf.UseShellExecute = false;
            prc = new Process { StartInfo = inf };
            prc.Start();
            prc.WaitForExit();

            if (prc.ExitCode != 0) ok = false;
            WriteDictionary(platform);

            Write($"Finished compiling (ok: {ok})");
            File.WriteAllText(AssetLogFile, _logFileData);
            if (!ok)
                File.Delete(AssetMD5sFile);
            return ok ? 0 : -2;
        }

        private static void Write(string s)
        {
            _logFileData += s + "\n";
            Console.WriteLine(s);
        }

        private static string[] GetAssetsNeedingRecompile(IEnumerable<string> inputFiles, string platform)
        {
            if (System.IO.File.Exists(AssetMD5sFile))
                _filesAndMD5s = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(
                     System.IO.File.ReadAllText(AssetMD5sFile));

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

            if (!_filesAndMD5s.ContainsKey("__internalPlatformID") || _filesAndMD5s["__internalPlatformID"] != platform)
            {
                _filesAndMD5s.Clear();
                return inputFiles.ToArray();
            }
            return _outFiles.ToArray();
        }

        private static void WriteDictionary(string platform)
        {
            _filesAndMD5s["__internalPlatformID"] = platform;
            System.IO.File.WriteAllText(AssetMD5sFile, Newtonsoft.Json.JsonConvert.SerializeObject(_filesAndMD5s));
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
