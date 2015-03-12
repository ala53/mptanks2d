namespace Pipeline.Spritesheets.Animator
{
    partial class CreateAndAnimate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.spritesWideSelector = new System.Windows.Forms.NumericUpDown();
            this.spritesTallSelector = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.frameRateSelector = new System.Windows.Forms.NumericUpDown();
            this.framesPrv = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.animPrv = new System.Windows.Forms.PictureBox();
            this.prvPlay = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.loopAnimBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.spritesWideSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spritesTallSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frameRateSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.framesPrv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.animPrv)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(281, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Select file";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // spritesWideSelector
            // 
            this.spritesWideSelector.Location = new System.Drawing.Point(15, 58);
            this.spritesWideSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spritesWideSelector.Name = "spritesWideSelector";
            this.spritesWideSelector.Size = new System.Drawing.Size(67, 20);
            this.spritesWideSelector.TabIndex = 1;
            this.spritesWideSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spritesWideSelector.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // spritesTallSelector
            // 
            this.spritesTallSelector.Location = new System.Drawing.Point(98, 58);
            this.spritesTallSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spritesTallSelector.Name = "spritesTallSelector";
            this.spritesTallSelector.Size = new System.Drawing.Size(58, 20);
            this.spritesTallSelector.TabIndex = 2;
            this.spritesTallSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spritesTallSelector.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Sprites across x Sprites vertical";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(15, 85);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(278, 20);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "Name of sheet";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(15, 111);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(278, 20);
            this.textBox2.TabIndex = 5;
            this.textBox2.Text = "Codename for animation";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(15, 137);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(278, 20);
            this.textBox3.TabIndex = 6;
            this.textBox3.Text = "Friendly name of animation";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(223, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Framerate";
            // 
            // frameRateSelector
            // 
            this.frameRateSelector.Location = new System.Drawing.Point(226, 58);
            this.frameRateSelector.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.frameRateSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.frameRateSelector.Name = "frameRateSelector";
            this.frameRateSelector.Size = new System.Drawing.Size(67, 20);
            this.frameRateSelector.TabIndex = 7;
            this.frameRateSelector.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // framesPrv
            // 
            this.framesPrv.Location = new System.Drawing.Point(299, 12);
            this.framesPrv.Name = "framesPrv";
            this.framesPrv.Size = new System.Drawing.Size(275, 203);
            this.framesPrv.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.framesPrv.TabIndex = 9;
            this.framesPrv.TabStop = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(218, 400);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // animPrv
            // 
            this.animPrv.Location = new System.Drawing.Point(299, 221);
            this.animPrv.Name = "animPrv";
            this.animPrv.Size = new System.Drawing.Size(275, 203);
            this.animPrv.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.animPrv.TabIndex = 11;
            this.animPrv.TabStop = false;
            // 
            // prvPlay
            // 
            this.prvPlay.Location = new System.Drawing.Point(498, 400);
            this.prvPlay.Name = "prvPlay";
            this.prvPlay.Size = new System.Drawing.Size(75, 23);
            this.prvPlay.TabIndex = 12;
            this.prvPlay.Text = "Play";
            this.prvPlay.UseVisualStyleBackColor = true;
            this.prvPlay.Click += new System.EventHandler(this.prvPlay_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(15, 164);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(119, 17);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Animation is vertical";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // loopAnimBox
            // 
            this.loopAnimBox.AutoSize = true;
            this.loopAnimBox.Location = new System.Drawing.Point(412, 404);
            this.loopAnimBox.Name = "loopAnimBox";
            this.loopAnimBox.Size = new System.Drawing.Size(50, 17);
            this.loopAnimBox.TabIndex = 14;
            this.loopAnimBox.Text = "Loop";
            this.loopAnimBox.UseVisualStyleBackColor = true;
            // 
            // CreateAndAnimate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 435);
            this.Controls.Add(this.loopAnimBox);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.prvPlay);
            this.Controls.Add(this.animPrv);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.framesPrv);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.frameRateSelector);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.spritesTallSelector);
            this.Controls.Add(this.spritesWideSelector);
            this.Controls.Add(this.button1);
            this.Name = "CreateAndAnimate";
            this.Text = "CreateAndAnimate";
            ((System.ComponentModel.ISupportInitialize)(this.spritesWideSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spritesTallSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frameRateSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.framesPrv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.animPrv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown spritesWideSelector;
        private System.Windows.Forms.NumericUpDown spritesTallSelector;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown frameRateSelector;
        private System.Windows.Forms.PictureBox framesPrv;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox animPrv;
        private System.Windows.Forms.Button prvPlay;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox loopAnimBox;
    }
}