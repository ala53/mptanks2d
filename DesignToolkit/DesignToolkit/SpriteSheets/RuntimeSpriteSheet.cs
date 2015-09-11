using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Toolkit.SpriteSheets
{
    class RuntimeSpriteSheet
    {
        public string Name { get; set; } = "Enter name";
        public BitmapImage Image { get; set; }
        public List<RuntimeSprite> Sprites { get; private set; }
        = new List<RuntimeSprite>();
        public List<RuntimeAnimation> Animations { get; private set; }
        = new List<RuntimeAnimation>();

        public RuntimeSprite FindByName(string name) =>
            Sprites.FirstOrDefault(a => a.Name == name);
        public RuntimeAnimation FindAnimByName(string name) =>
            Animations.FirstOrDefault(a => a.Name == name);

        public bool SpriteNameExists(string name)
        {
            foreach (var sprite in Sprites)
                if (sprite.Name.Equals(name))
                    return true;

            return false;
        }
        public bool AnimationNameExists(string name)
        {
            foreach (var anim in Animations)
                if (anim.Name.Equals(name))
                    return true;

            return false;
        }

        public static RuntimeSpriteSheet Load(string JSON) =>
            Load(JSONSpriteSheet.Load(JSON));

        public static RuntimeSpriteSheet Load(JSONSpriteSheet sheet)
        {
            var ss = new RuntimeSpriteSheet();
            foreach (var sprite in sheet.Sprites)
                ss.Sprites.Add(new RuntimeSprite(ss)
                {
                    Name = sprite.Name,
                    Rectangle = new Rect(sprite.X, sprite.Y, sprite.Width, sprite.Height)
                });
            foreach (var anim in sheet.Animations)
                ss.Animations.Add(new RuntimeAnimation(ss)
                {
                    FrameRate = anim.FrameRate,
                    Name = anim.Name,
                    Sprites = anim.Frames.Select(a => ss.Sprites.First(b => b.Name == a)).ToList()
                });

            return ss;
        }

        public byte[] SerializeWithImage()
        {
            var ms = new MemoryStream();
            var oStr = new ZipOutputStream(ms);

            WriteFile(oStr, "info.json", Encoding.UTF8.GetBytes(Serialize().Serialize()));
            WriteFile(oStr, "image.png", SaveImg());

            oStr.Flush();
            oStr.Close();
            oStr.Dispose();

            var arr = ms.ToArray();
            ms.Close();
            ms.Dispose();
            return arr;
        }

        public static RuntimeSpriteSheet LoadWithImage(byte[] savedFile)
        {
            var ms = new MemoryStream(savedFile);
            var zf = new ZipFile(ms);

            var sheet = Load(GetStringFromZip(zf, zf.GetEntry("info.json")));
            sheet.Image = DecodePhoto(GetBytesFromZip(zf, zf.GetEntry("image.png")));

            ms.Dispose();
            return sheet;
        }
        public static BitmapImage DecodePhoto(byte[] byteVal)
        {
            MemoryStream strmImg = new MemoryStream(byteVal);
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.StreamSource = strmImg;
            myBitmapImage.DecodePixelWidth = 200;
            myBitmapImage.EndInit();
            return myBitmapImage;
        }

        private byte[] SaveImg()
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(Image));
            var ms = new MemoryStream();
            encoder.Save(ms);
            var arr = ms.ToArray();
            ms.Dispose();
            return arr;
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
        private static void WriteFile(ZipOutputStream zf, string name, byte[] data)
        {
            var f = new ZipEntry(name);
            zf.PutNextEntry(f);
            StreamUtils.Copy(new MemoryStream(data), zf, new byte[4096]);
            zf.CloseEntry();
        }

        public JSONSpriteSheet Serialize()
        {
            var ss = new JSONSpriteSheet();
            ss.Name = Name;
            ss.Sprites = Sprites.Select(
                a => new JSONSpriteSheet.JSONSprite()
                {
                    Name = a.Name,
                    X = (int)a.Rectangle.X,
                    Y = (int)a.Rectangle.Y,
                    Width = (int)a.Rectangle.Width,
                    Height = (int)a.Rectangle.Height
                }).ToArray();

            ss.Animations = Animations.Select(
                a => new JSONSpriteSheet.JSONAnimation()
                {
                    FrameRate = a.FrameRate,
                    Name = a.Name,
                    Frames = a.Sprites.Select(b => b.Name).ToArray()
                }).ToArray();

            return ss;
        }
    }
    class RuntimeSprite
    {
        public RuntimeSprite(RuntimeSpriteSheet sheet) { Parent = sheet; }
        public RuntimeSpriteSheet Parent { get; private set; }
        public string Name { get; set; } = "Enter title";
        public Rect Rectangle { get; set; }
    }
    class RuntimeAnimation
    {
        public RuntimeAnimation(RuntimeSpriteSheet sheet) { Parent = sheet; }
        public RuntimeSpriteSheet Parent { get; private set; }
        public string Name { get; set; } = "Enter name";
        public float FrameRate { get; set; }
        public List<RuntimeSprite> Sprites { get; set; } = new List<RuntimeSprite>();
    }
}
