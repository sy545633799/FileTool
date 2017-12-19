using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using StarEdit.Interface;
using StarEdit.Tools;
using StarEdit.PaintServices;

namespace StarEdit.MyComponents
{
    public partial class UCPictureComboBox : UserControl, IAutoControl
    {
        String pictureSize;
        String url;

        public String PictureSize
        {
            get { return pictureSize; }
            set
            {
                pictureSize = value;
                String[] sizes = pictureSize.Split('*');
                pictureBox1.Width = int.Parse(sizes[0]);
                pictureBox1.Height = int.Parse(sizes[1]);
                this.Height = pictureBox1.Height;
                this.Width = pictureBox1.Width + pictureBox1.Location.X;
            }
        }

        public string Value
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
                bool isselect = false;
                foreach (String s in comboBox1.Items)
                {
                    String[] data = s.Split('.');
                    if (value == data[0])
                    {
                        comboBox1.SelectedItem = s;
                        isselect = true;
                    }
                }
                if (!isselect)
                {
                    comboBox1.SelectedItem = null;
                }
            }
        }

        public void SetPath(string urlpath, string ext)
        {
            List<String> filenames = DirectoryTool.GetAllFileName(urlpath, ext);
            foreach (String file in filenames)
            {
                comboBox1.Items.Add(file);
            }
            Tag = urlpath;
        }

        public UCPictureComboBox()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string path = String.Format("{0}/{1}", Tag.ToString(), comboBox1.SelectedItem.ToString());
                pictureBox1.Image = MapPainter.GetNetImage(path);
                string s = comboBox1.SelectedItem.ToString();
                if (s.Contains(".")) //.分割结构
                {
                    String[] data = s.Split('.');
                    url = data[0];
                }
            }
            else
            {
                pictureBox1.Image = null;
            }
            
        }
    }
}
