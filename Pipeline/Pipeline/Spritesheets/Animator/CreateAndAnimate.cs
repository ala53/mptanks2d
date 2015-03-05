using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pipeline.Spritesheets.Animator
{
    public partial class CreateAndAnimate : Form
    {
        private Bitmap bmp;
        private Bitmap sourceBmp;
        private Bitmap[] animFrames;
        public CreateAndAnimate()
        {
            InitializeComponent();
        }

        private MemoryStream sourceMs;
        private void button1_Click(object sender, EventArgs e)
        {
            var fod = new OpenFileDialog();
            fod.ShowDialog();

            if (fod.FileName != null && fod.FileName != "")
            {
                if (sourceBmp != null) 
                    sourceBmp.Dispose();

                if (sourceMs != null)
                    sourceMs.Dispose();

                sourceMs = new MemoryStream(System.IO.File.ReadAllBytes(fod.FileName));

                sourceBmp = new Bitmap(sourceMs);
                RegenerateSprites();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            RegenerateSprites();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            RegenerateSprites();
        }

        private void RegenerateSprites()
        {
            if (sourceBmp == null)
                return;
            if (_timer != null && _timer.Enabled) //Stop animations
                _timer.Stop();

            if (bmp != null)
                bmp.Dispose();

            if (animFrames != null)
            {
                foreach (var fr in animFrames)
                    if (fr != null) fr.Dispose();
                animFrames = null;
            }
            var width = (int)spritesWideSelector.Value;
            var height = (int)spritesTallSelector.Value;
            var blockX = sourceBmp.Width / width;
            var blockY = sourceBmp.Height / height;
            animFrames = new Bitmap[width * height];

            bmp = new Bitmap(sourceBmp);
            var grp = Graphics.FromImage(bmp);
            grp.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            grp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            var fSize = blockX / 4;

            var frameNum = 1;
            if (!checkBox1.Checked)
                for (var y = 0; y < height; y++)
                    for (var x = 0; x < width; x++)
                    {
                        DrawStr(frameNum, new Point(blockX * x, blockY * y), grp, fSize);
                        frameNum++;
                    }
            else
                for (var x = 0; x < width; x++)
                    for (var y = 0; y < height; y++)
                    {
                        DrawStr(frameNum, new Point(blockX * x, blockY * y), grp, fSize);
                        frameNum++;
                    }

            grp.Dispose();
            framesPrv.Image = bmp;

            frameNum = 0;
            //Split and build animation frames
            if (!checkBox1.Checked)
                for (var y = 0; y < height; y++)
                    for (var x = 0; x < width; x++)
                    {
                        var bm = new Bitmap(blockX, blockY);
                        var gr = Graphics.FromImage(bm);
                        gr.DrawImage(sourceBmp, new Rectangle(0, 0, blockX, blockY),
                            new Rectangle(x * blockX, y * blockY, blockX, blockY), GraphicsUnit.Pixel);
                        gr.Dispose();
                        animFrames[frameNum] = bm;
                        frameNum++;
                    }
            else
                for (var x = 0; x < width; x++)
                    for (var y = 0; y < height; y++)
                    {
                        var bm = new Bitmap(blockX, blockY);
                        var gr = Graphics.FromImage(bm);
                        gr.DrawImage(sourceBmp, new Rectangle(0, 0, blockX, blockY),
                            new Rectangle(x * blockX, y * blockY, blockX, blockY), GraphicsUnit.Pixel);
                        gr.Dispose();
                        animFrames[frameNum] = bm;
                        frameNum++;
                    }
        }

        private void DrawStr(int number, Point pt, Graphics gr, float size = 14)
        {
            var GP = new GraphicsPath(FillMode.Winding);
            GP.AddString("F" + number, new FontFamily("arial"),
                (int)System.Drawing.FontStyle.Regular, size, pt, StringFormat.GenericDefault);
            gr.FillPath(Brushes.White, GP);
            gr.DrawPath(Pens.Black, GP);
        }

        private Timer _timer;
        private void prvPlay_Click(object sender, EventArgs e)
        {
            if (_timer != null && _timer.Enabled)
            {
                prvPlay.Text = "Play";
                _timer.Stop();
                return;
            }

            prvPlay.Text = "Stop";

            if (_timer != null)
                _timer.Dispose();
            frame = 0;
            _timer = new Timer();
            if (frameRateSelector.Value > 30)
            {
                tickAmount = (double)frameRateSelector.Value / 30;
                _timer.Interval = 1000 / 30;
            }
            else
            {
                tickAmount = 1;
                _timer.Interval = 1000 / (int)frameRateSelector.Value;
            }
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private double frame = 0;
        private double tickAmount = 1;
        void _timer_Tick(object sender, EventArgs e)
        {
            if (frame >= animFrames.Length)
            {
                if (loopAnimBox.Checked)
                {
                    frame = 0;
                }
                else
                {
                    prvPlay.Text = "Play";
                    _timer.Stop();
                    return;
                }
            }

            animPrv.Image = animFrames[(int)frame];
            frame += tickAmount;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            RegenerateSprites();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var sfd = new SaveFileDialog();
                sfd.Filter = "PNG Files|*.png";
                sfd.ShowDialog();
                if (sfd.FileName == null || sfd.FileName == "")
                    return;

                dynamic obj = new ExpandoObject();
                obj.name = textBox1.Text;

                var sprites = new List<Sprite>();
                var width = (int)spritesWideSelector.Value;
                var height = (int)spritesTallSelector.Value;
                var blockX = sourceBmp.Width / width;
                var blockY = sourceBmp.Height / height;
                var frameNum = 0;
                if (!checkBox1.Checked)
                    for (var y = 0; y < height; y++)
                        for (var x = 0; x < width; x++)
                        {
                            sprites.Add(new Sprite()
                            {
                                name = "frame_" + frameNum,
                                x = blockX * x,
                                y = blockY * y,
                                width = blockX,
                                height = blockY
                            });
                            frameNum++;
                        }
                else
                    for (var x = 0; x < width; x++)
                        for (var y = 0; y < height; y++)
                        {
                            sprites.Add(new Sprite()
                            {
                                name = "frame_" + frameNum,
                                x = blockX * x,
                                y = blockY * y,
                                width = blockX,
                                height = blockY
                            });
                            frameNum++;
                        }


                obj.sprites = sprites;

                obj.animations= new Animation[1];

                obj.animations[0] = new Animation() {
                frameRate =(int) frameRateSelector.Value,
                name = textBox2.Text,
                friendly = textBox3.Text
                };
                var animFrames = new List<string>();
                foreach (var spr in sprites)
                    animFrames.Add(spr.name);

                obj.animations[0].frames = animFrames.ToArray();

                var str = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(sfd.FileName + ".json", str);
                sourceBmp.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch
            {
                MessageBox.Show("Save failed!");
            }
        }

        private class Animation
        {
            public string name;
            public string friendly;
            public int frameRate;
            public string[] frames;
        }

        private class Sprite
        {
            public string name;
            public int x;
            public int y;
            public int width;
            public int height;
        }
    }
}
