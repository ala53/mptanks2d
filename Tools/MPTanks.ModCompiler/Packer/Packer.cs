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
using System.Runtime.InteropServices;
using NAudio.Wave;

namespace MPTanks.ModCompiler.Packer
{
    public static class Packer
    {
        public static byte[] Pack()
        {
            var ms = new MemoryStream();
            //We write in front of the header so that the zip can't be read
            //or flagged by AV software
            ms.WriteByte(7);
            ms.WriteByte(0);
            ms.WriteByte(7);
            var zipFile = new ZipOutputStream(ms);
            zipFile.IsStreamOwner = false;
            zipFile.SetLevel(9);

            var header = new ModHeader();
            header.Name = Program.name;
            header.Major = DependencyResolver.ParseVersionMajor(Program.version);
            header.Minor = DependencyResolver.ParseVersionMinor(Program.version);
            header.Tag = DependencyResolver.ParseVersionTag(Program.version);
            header.CodeFiles = Archive(zipFile, Program.srcFiles);

            header.ImageFiles = ArchiveImages(zipFile, Program.imageAssets);

            header.ComponentFiles = ArchiveComponents(zipFile, Program.components);
            header.DLLFiles = Archive(zipFile, Program.dlls);
            header.SoundFiles = ArchiveAudio(zipFile, Program.soundAssets);
            header.MapFiles = Archive(zipFile, Program.maps);
            header.DatabaseUrl = DependencyResolver.GetModUrl(Program.name);
            header.Description = Program.description;
            header.Dependencies = BuildDependencies();

            var headerString = JsonConvert.SerializeObject(header, Formatting.Indented);

            WriteFile(zipFile, "mod.json", headerString);

            zipFile.Close();
            zipFile.Dispose();

            ms.Seek(0, SeekOrigin.Begin);
            var data = ms.ToArray();
            ms.Dispose();
            return ms.ToArray();
        }
        private static string[] ArchiveAudio(ZipOutputStream zf, List<string> src)
        {
            foreach (var file in src)
            {
                WriteFile(zf, GetFileNameOnly(file), ConvertAudio(file));
            }

            return GetFileNamesOnly(src).Select(a => a).ToArray();
        }

        private static string[] ArchiveImages(ZipOutputStream zf, List<string> src)
        {
            var assets = new List<string>();
            var assetNames = new List<string>();

            foreach (var fl in src)
            {
                var fi = new FileInfo(fl);
                if (fi.Extension.Equals(".ssjson", StringComparison.OrdinalIgnoreCase))
                {
                    var fs = new FileStream(fl, FileMode.Open, FileAccess.Read);
                    var zif = new ZipFile(fs);

                    var info = GetBytesFromZip(zif, zif.GetEntry("info.json"));
                    var img = GetBytesFromZip(zif, zif.GetEntry("image.png"));

                    var saveFile = GetFileNameOnly(fl).Replace(".ssjson", "");
                    WriteFile(zf, saveFile + ".png", ConvertImage(img));
                    WriteFile(zf, saveFile + ".png.json", info);

                    assetNames.Add(saveFile);
                    assetNames.Add(saveFile + ".json");

                    fs.Dispose();
                    zif.Close();
                }
                else
                {
                    //it's a normal sprite sheet
                    assets.Add(fl);
                    assets.Add(fl + ".json");

                    WriteFile(zf, GetFileNameOnly(fl), ConvertImage(fl));
                    WriteFile(zf, GetFileNameOnly(fl) + ".json", File.ReadAllBytes(fl + ".json"));

                    assetNames.Add(GetFileNameOnly(fl));
                    assetNames.Add(GetFileNameOnly(fl) + ".json");
                }
            }

            return assetNames.ToArray();
        }

        private static WaveStream GetAudioStreamByExtension(string name)
        {
            var ext = name.Split('.').Last().ToLower();
            switch (ext)
            {
                case "flac":
                    return new NAudio.Flac.FlacReader(name);
                case "ogg":
                    return new NAudio.Vorbis.VorbisWaveReader(name);
                case "wma":
                    return new NAudio.WindowsMediaFormat.WMAFileReader(name);
                default:
                    return new MediaFoundationReader(name);
            }
        }
        private static byte[] ConvertAudio(string audio)
        {
            Console.WriteLine($"Converting {audio} to <<NO CONVERSION>>");
            return File.ReadAllBytes(audio);
            /*
            //Convert all to WAVE
            try
            {
                using (var reader = GetAudioStreamByExtension(audio))
                {
                    using (WaveStream stream = WaveFormatConversionStream.CreatePcmStream(reader))
                    {
                        using (var ms = new MemoryStream())
                        {
                            var writer = new WaveFileWriter(ms, stream.WaveFormat);
                            reader.CopyTo(writer);

                            return ms.ToArray();
                        }
                    }
                }
            }
            catch
            {
                return File.ReadAllBytes(audio);
            }
            */
        }

        private static byte[] ConvertImage(string img)
        {
            Console.WriteLine($"{img} converted to PNG");
            return ConvertImage(File.ReadAllBytes(img));
        }

        private static byte[] ConvertImage(byte[] img)
        {
            //load img
            var ms = new MemoryStream(img);

            //Get the pixels
            var bmp = new Bitmap(ms);
            //for now, leave as BMP
            var strm = new MemoryStream();
            bmp.Save(strm, System.Drawing.Imaging.ImageFormat.Png);
            var bArr = strm.ToArray();
            ms.Dispose();
            strm.Dispose();
            bmp.Dispose();
            return bArr;
            /*
            //Requires Install-Package ManagedSquish
            //But it doesn't work right
            var bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var bits = new byte[bmp.Width * bmp.Height * 4];
            

            Marshal.Copy(bmData.Scan0, bits, 0, bits.Length);
            //Unlock the source
            bmp.UnlockBits(bmData);
            //Swizzle [A]RGB to RGB[A]
            for (var i = 0; i < bits.Length; i += 4)
            {
                var a = bits[i];
                var b = bits[i + 3];
                //bits[i] = b;
                //bits[i + 3] = a;
            }

            var flags = ManagedSquish.SquishFlags.ColourIterativeClusterFit | ManagedSquish.SquishFlags.Dxt5;
            var mem = ManagedSquish.Squish.GetStorageRequirements(bmp.Width, bmp.Height, flags);
            var output = Marshal.AllocHGlobal(mem);

            IntPtr input = Marshal.AllocHGlobal(bits.Length);
            Marshal.Copy(bits, 0, input, bits.Length);

            ManagedSquish.Squish.CompressImage(input, bmp.Width, bmp.Height, output, flags);
            bmp.Dispose();

            var dds = new byte[mem];

            Marshal.FreeHGlobal(input);
            Marshal.Copy(output, dds, 0, mem);

            Marshal.FreeHGlobal(output);


            return dds;
            */
        }

        private static string[] ArchiveComponents(ZipOutputStream zf, List<string> src)
        {
            foreach (var a in src)
            {
                var result = Encoding.UTF8.GetBytes(
                    BodyBuilder.ProcessObject(a,
                        JsonConvert.DeserializeObject<Engine.Serialization.GameObjectComponentsJSON>(
                            File.ReadAllText(a)
                            )));
                WriteFile(zf, GetFileNameOnly(a), result);
            }

            return GetFileNamesOnly(src).ToArray();
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

        private static string[] Archive(ZipOutputStream zf, List<string> src)
        {
            foreach (var a in src)
            {
                var data = File.ReadAllBytes(a);
                WriteFile(zf, GetFileNameOnly(a), data);
            }

            return src.Select(a => GetFileNameOnly(a)).ToArray();
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
        private static string GetStringFromZip(ZipFile zf, ZipEntry ze) =>
            Encoding.UTF8.GetString(GetBytesFromZip(zf, ze));
        private static byte[] GetBytesFromZip(ZipFile zf, ZipEntry ze)
        {
            byte[] ret = null;

            if (ze != null)
            {
                Stream s = zf.GetInputStream(ze);
                ret = new byte[ze.Size];
                s.Read(ret, 0, ret.Length);
                s.Dispose();
            }

            return ret;
        }
    }
}