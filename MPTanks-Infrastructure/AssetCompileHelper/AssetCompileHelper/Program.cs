using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetCompileHelper
{
    class Program
    {
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

            foreach (var file in files)
                cmdArgs += String.Format("/build:\"{0}\" ", file);
            
            var prc = Process.Start(@"C:\Program Files (x86)\MSBuild\MonoGame\v3.0\Tools\MGCB.exe",
                cmdArgs);
            prc.WaitForExit();

            if (prc.ExitCode != 0) ok = false;

            return ok ? 0 : -2;
        }
    }
}
