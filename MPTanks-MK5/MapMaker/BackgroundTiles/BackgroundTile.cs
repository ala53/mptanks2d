using Microsoft.Xna.Framework.Graphics;
using MPTanks.Engine.Settings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.MapMaker.BackgroundTiles
{
    class BackgroundTile
    {
        public struct LODDeclaration
        {
            public int SizePx { get; set; }
            public float SizeBlocks { get; set; }
            public LODDeclaration(int px, float blocks)
            {
                SizePx = px;
                SizeBlocks = blocks;
            }
        }

        public static LODDeclaration[] TileLODLevels =
        {
            new LODDeclaration(1024, 128),
            new LODDeclaration(512, 64),
            new LODDeclaration(256, 32),
            new LODDeclaration(128, 16),
            new LODDeclaration(64, 8),
            new LODDeclaration(32, 4)
        };

        public string Name { get; private set; }
        public Dictionary<int, LODLevel> Levels { get; private set; } = new Dictionary<int, LODLevel>();
        public bool IsComposite { get; set; }
        public bool IsSmoothFadeComposite { get; set; }
        public BackgroundTile CompositeTileOne { get; set; }
        public BackgroundTile CompositeTileTwo { get; set; }
        public CompositeType CompositionType { get; set; }

        public enum CompositeType
        {
            /// <summary>
            /// xxxx
            /// xxxx
            /// xxxx
            /// xxxx
            /// </summary>
            None,
            /// <summary>
            /// xxxx
            /// xxxx
            /// yyyy
            /// yyyy
            /// </summary>
            HalfAndHalfHorizontal,
            /// <summary>
            /// xxxx
            /// yyyy
            /// yyyy
            /// yyyy
            /// </summary>
            TwentyFiveSeventyFive,
            /// <summary>
            /// xxyy
            /// xxyy
            /// yyyy
            /// </summary>
            CornerCoveredHalf,
            /// <summary>
            /// xxxy
            /// xxxy
            /// xxxy
            /// yyyy
            /// </summary>
            CornerCoveredQuarter,
            /// <summary>
            /// xxxxx/
            /// xxxx/y
            /// xxx/yy
            /// xx/yyy
            /// x/yyyy
            /// /yyyyy
            /// </summary>
            TriangleSlice
        }

        private static Random _random = new Random();

        public BackgroundTile(Image image, string name)
        {
            var dirToStore = Path.Combine(SettingsBase.ConfigDir, "mapmakertilecache");
            Directory.CreateDirectory(dirToStore);

            //Error checking
            if (image.Width != image.Height)
                throw new Exception("Background tiles must have an identical width and height");
            //Generate LOD
            foreach (var levelDecl in TileLODLevels)
            {
                var img = new Bitmap(levelDecl.SizePx, levelDecl.SizePx);
                var gd = Graphics.FromImage(img);
                gd.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                gd.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gd.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gd.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gd.DrawImage(image, 0, 0, levelDecl.SizePx, levelDecl.SizePx);

                img.Save(
                    Path.Combine(dirToStore, $"tile_{name}_{levelDecl.SizePx}px.png"),
                    System.Drawing.Imaging.ImageFormat.Png);

                Levels.Add(levelDecl.SizePx, new LODLevel
                {
                    Image = img,
                    TempFileName = Path.Combine(dirToStore, $"tile_{name}_{levelDecl.SizePx}px.png")
                });

                gd.Dispose();
            }
        }
        public struct LODLevel
        {
            public Image Image { get; set; }
            public string TempFileName { get; set; }
        }
    }
}
