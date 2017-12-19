using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using StarEdit.Interface;
using StarEdit.Tools;
using StarEdit.MysqlServices;
using System.IO;

namespace StarEdit.MyComponents
{
    public partial class UCEffectBox : UserControl, IAutoControl
    {
        string urlpath;
        string ext;

        public string Value
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }

        public void SetPath(string urlpath, string ext)
        {
            this.urlpath = urlpath;
            this.ext = ext;
        }

        public UCEffectBox()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            OldDataPack pack =OldDataPackBook.GetPack("cfg_skill_effect");
            if(pack.data.ContainsKey(int.Parse(textBox1.Text)))
            {
                string path = string.Format("{0}/{1}{2}", urlpath, pack.data[int.Parse(textBox1.Text)][1], ext);
                if (File.Exists(path))
                {
                    pictureBox1.Image = Image.FromFile(path);
                    return;
                }
            }
            pictureBox1.Image = null;
        }
    }
}
