using System;
using System.Drawing;
using System.Windows.Forms;
using StarEdit.Interface;

namespace StarEdit.MyComponents
{
    public partial class UCBoxIcon : UserControl, IAutoControl
    {
        public UCBoxIcon()
        {
            InitializeComponent();
        }

        public String Value
        {
            get { return textBoxData.Text; }
            set { textBoxData.Text = value; } 
        }

        public string Icon
        {
            set { pictureBox1.Image = Image.FromFile(value); }
        }

        public string Des
        {
            set { label1.Text = value; }
        }
    }
}

