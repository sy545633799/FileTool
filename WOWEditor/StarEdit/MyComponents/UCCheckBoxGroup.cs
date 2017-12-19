using System;
using System.Drawing;
using System.Windows.Forms;
using StarEdit.Interface;

namespace StarEdit.MyComponents
{
    public partial class UCCheckBoxGroup : UserControl, IAutoControl
    {
        private string info;

        public String Value
        {
            get
            {
                int value = 0;
                foreach (Control c in this.Controls)
                {
                    int tagvalue = int.Parse(c.Tag.ToString());
                    CheckBox cb = c as CheckBox;
                    if (cb.Checked)
                        value += tagvalue;
                }
                return value.ToString();
            }
            set
            {
                int index = int.Parse(value);
                foreach (Control c in this.Controls)
                {
                    int tagvalue = int.Parse(c.Tag.ToString());
                    CheckBox cb = c as CheckBox;
                    cb.Checked = ((index & tagvalue) == tagvalue);
                }
            }
        }

        public string Info
        {
            set
            {
                info = value;
                String[] checkInfo = info.Split('|');
                this.Controls.Clear();
                int count = 0;
                bool b = false; 
                foreach (String s in checkInfo)
                {
                    string[] checkdata = s.Split('.');
                    CheckBox cb = new CheckBox();
                    cb.AutoSize = true;
                    cb.Text = checkdata[1];
                    cb.Tag = checkdata[0];
                    int y = count / 6 * 30;
                    if (cb.Text.Contains("[NL]"))
                    {
                        b = true;
                        y = 21;
                    }
                    cb.Location = new Point((count % 6) * 98, y);
                    this.Controls.Add(cb);
                    count++;
                }
                int param = 21;
                if (b)
                {
                    param = 42;
                }
                this.Height = (count - 1) / 6 * 30 + param;
            }
        }

        public UCCheckBoxGroup()
        {
            InitializeComponent();
        }
    }
}
