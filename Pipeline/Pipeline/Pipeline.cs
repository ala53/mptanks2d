using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pipeline
{
    public partial class Pipeline : Form
    {
        public Pipeline()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var cfe = new Spritesheets.Animator.CreateAndAnimate();
            cfe.ShowDialog();
        }
    }
}
